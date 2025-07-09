export interface Rating {
  id: number;
  messageId: number;
  value: number;
}

export interface Message {
  id: number;
  conversationId: number;
  author: 'User' | 'Bot';
  content: string;
  rating?: Rating;
}

export interface Conversation {
  id: number;
  created: string;
  messages: Message[];
}
