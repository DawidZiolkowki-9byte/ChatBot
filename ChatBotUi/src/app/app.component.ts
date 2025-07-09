import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { ChatService } from './chat.service';
import { Message } from './models';

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
export class AppComponent implements OnInit {
  messages: Message[] = [];
  conversationId?: number;
  input = '';
  isGenerating = false;
  streamSub?: Subscription;

  constructor(private chat: ChatService) {}

  ngOnInit(): void {
    this.chat.createConversation().subscribe(convo => {
      this.conversationId = convo.id;
      this.messages = convo.messages || [];
    });
  }

  send() {
    if (!this.conversationId || !this.input.trim()) return;
    const content = this.input;
    this.input = '';
    const userMsg: Message = {
      id: 0,
      conversationId: this.conversationId,
      author: 'User',
      content
    };
    this.messages.push(userMsg);
    let botMsg: Message = {
      id: 0,
      conversationId: this.conversationId,
      author: 'Bot',
      content: ''
    };
    this.messages.push(botMsg);
    this.isGenerating = true;
    this.streamSub = this.chat.sendMessage(this.conversationId, content).subscribe({
      next: chunk => {
        botMsg.content += chunk;
      },
      error: () => {
        this.isGenerating = false;
      },
      complete: () => {
        this.isGenerating = false;
      }
    });
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
