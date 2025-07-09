import { AfterViewInit, Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { ChatService } from './chat.service';
import { Message, Conversation } from './models';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    MatIconModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  conversations: Conversation[] = [];
  conversationId?: number;
  conversationTitle?: string;
  input = '';
  isGenerating = false;
  streamSub?: Subscription;
  @ViewChild('messageList') messageList?: ElementRef<HTMLDivElement>;

  constructor(private chat: ChatService) {}

  ngOnInit(): void {
     const stored = localStorage.getItem('conversationIds');
    const ids: number[] = stored ? JSON.parse(stored) : [];
    if (ids.length) {
      let remaining = ids.length;
      ids.forEach(id => {
        this.chat.getConversation(id).subscribe({
          next: convo => {
            this.conversations.push(convo);
            if (--remaining === 0) this.startNewConversation(ids);
          },
          error: () => {
            if (--remaining === 0) this.startNewConversation(ids);
          }
        });
      });
    } else {
      this.startNewConversation(ids);
    }
  }

  private startNewConversation(ids: number[]) {
    this.chat.createConversation().subscribe(convo => {
      this.conversationId = convo.id;
      this.conversations.push(convo);
      ids.push(convo.id);
      localStorage.setItem('conversationIds', JSON.stringify(ids));
      this.scrollToBottom();
    });
  }

  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    const el = this.messageList?.nativeElement;
    if (el) {
      el.scrollTop = el.scrollHeight;
    }
  }

  send() {
    if (!this.input.trim()) return;
    const content = this.input;
    this.input = '';
    const convo = this.conversations[this.conversations.length - 1];
    const proceed = () => {
      const userMsg: Message = {
        id: 0,
        conversationId: this.conversationId!,
        author: 'User',
        content
      };
      convo.messages.push(userMsg);
      let botMsg: Message = {
        id: 0,
        conversationId: this.conversationId!,
        author: 'Bot',
        content: ''
      };
      convo.messages.push(botMsg);
      this.isGenerating = true;
      this.scrollToBottom();
      this.streamSub = this.chat
        .sendMessage(this.conversationId!, content)
        .subscribe({
        next: chunk => {
          botMsg.content += chunk;
          this.scrollToBottom();
        },
        error: () => {
          this.isGenerating = false;
          this.chat.getConversation(this.conversationId!).subscribe(c => {
            convo.messages = c.messages || [];
            this.scrollToBottom();
          });
          alert('Failed to send message.');
        },
        complete: () => {
          this.isGenerating = false;
          this.chat.getConversation(this.conversationId!).subscribe(c => {
            convo.messages = c.messages || [];
            convo.title = c.title;    
          this.scrollToBottom();
        });
        }
      });
    };

    if (!this.conversationId) {
      this.chat.createConversation().subscribe(c => {
        this.conversationId = c.id;
        convo.id = c.id;
        convo.created = c.created;
        convo.title = c.title;
        const stored = localStorage.getItem('conversationIds');
        const ids: number[] = stored ? JSON.parse(stored) : [];
        ids.push(c.id);
        localStorage.setItem('conversationIds', JSON.stringify(ids));
        proceed();
      });
    } else {
      proceed();
    }
  }

  stop() {
    this.streamSub?.unsubscribe();
    this.streamSub = undefined;
    this.isGenerating = false;
  }

  rate(message: Message, value: number) {
    if (!message.id) return;
    this.chat.setRating(message.id, value).subscribe(r => message.rating = r);
  }
}
