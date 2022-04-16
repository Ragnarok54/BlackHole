import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CONTACTS_URL, CONVERSATIONS_URL, CONVERSATION_ADD_URL, CONVERSATION_MESSAGES_URL, CONVERSATION_NAME_URL, MESSAGE_URL } from 'src/environments/environment';
import { ConversationModel } from '../models/conversation/conversationModel';
import { BaseMessage } from '../models/message/baseMessage';
import { Common } from '../shared/common';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private http: HttpClient) { }

  sendMessage(text: string, conversationId: string){
    var payload = new BaseMessage();
    payload.text = text;
    payload.conversationId = conversationId;

    return this.http.post(MESSAGE_URL, payload);
  }

  getSnapshots(count: number, skip: number){
    return this.http.get(CONVERSATIONS_URL + "?count=" + count + "&skip=" + skip);
  }

  getMessages(conversationId: string, skip: number, count: number){
    return this.http.get(`${CONVERSATION_MESSAGES_URL}/${conversationId}/${skip}/${count}`);
  }

  getName(conversationId: string){
    return this.http.get(`${CONVERSATION_NAME_URL}/${conversationId}`);
  }

  getContacts(query: string){
    return this.http.get(`${CONTACTS_URL}/${query}`);
  }

  createConversation(userIds: string[]){
    var body = new ConversationModel();
    body.name = Common.newConversationName;
    body.userIds = userIds;

    return this.http.post(`${CONVERSATION_ADD_URL}`, body);
  }
}
