import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_URL, MESSAGE_URL } from 'src/environments/environment';
import { Message } from '../models/message/message';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private http: HttpClient) { }

  sendMessage(text: string, conversationId: string){
    var payload = new Message();
    payload.text = text;
    payload.conversationId = conversationId;

    return this.http.post(API_URL + MESSAGE_URL, payload);
  }

  getSnapshots(count: number, skip: number){
    return this.http.get(API_URL + "/conversations?count=" + count + "&skip=" + skip);
  }
}
