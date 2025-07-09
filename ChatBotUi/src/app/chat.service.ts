import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Message, Rating, Conversation } from './models';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private api = '/api';

  constructor(private http: HttpClient) {}

  createConversation(): Observable<Conversation> {
    return this.http.post<Conversation>(`${this.api}/conversations`, {});
  }

  getConversation(id: number): Observable<Conversation> {
    return this.http.get<Conversation>(`${this.api}/conversations/${id}`);
  }

  sendMessage(conversationId: number, content: string): Observable<string> {
    return new Observable(observer => {
      fetch(`${this.api}/messages`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ conversationId, content })
      }).then(res => {
        const reader = res.body?.getReader();
        if (!reader) { observer.complete(); return; }
        const decoder = new TextDecoder();
        function read() {
          reader.read().then(({ done, value }) => {
            if (done) { observer.complete(); return; }
            observer.next(decoder.decode(value));
            read();
          }).catch(err => observer.error(err));
        }
        read();
      }).catch(err => observer.error(err));
      return () => {};
    });
  }

  setRating(messageId: number, value: number): Observable<Rating> {
    return this.http.post<Rating>(`${this.api}/ratings`, { messageId, value });
  }
}
