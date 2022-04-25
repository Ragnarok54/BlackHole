import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { first, map } from 'rxjs/operators';
import { CONTACTS_URL, CONVERSATIONS_URL, CONVERSATION_ADD_URL, CONVERSATION_MESSAGES_URL, CONVERSATION_NAME_URL, CONVERSATION_SEEN_URL, MESSAGE_URL } from 'src/environments/environment';
import { ConversationModel } from '../models/conversation/conversationModel';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';
import { BaseMessage } from '../models/message/baseMessage';
import { Message } from '../models/message/message';
import { Common } from '../shared/common';
import { ChatService } from './chat.service';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  private snapshots: BehaviorSubject<ConversationSnapshot[]> = new BehaviorSubject<ConversationSnapshot[]>([]);

  constructor(private http: HttpClient, private chatService: ChatService) {
    chatService.retrieveMappedObject().subscribe(
      (receivedObj: BaseMessage) => {
        var oldSnapshot = this.snapshots.value.find(s => s.conversationId == receivedObj.conversationId);
        var tempSnapshots = this.snapshots.value.filter(s => s.conversationId != receivedObj.conversationId);
        
        oldSnapshot.lastMessage.time = receivedObj.time;
        oldSnapshot.lastMessage.text = receivedObj.text;
        oldSnapshot.lastMessage.seen = false;
        tempSnapshots.unshift(oldSnapshot);

        this.snapshots.next(tempSnapshots);
      }
    )
    this.refreshSnapshots();
   }

  sendMessage(text: string, conversationId: string): Observable<Message> {
    var payload = new BaseMessage();
    payload.text = text;
    payload.conversationId = conversationId;

    return this.http.post(MESSAGE_URL, payload)
      .pipe(first())
        .pipe(
          map(
            (data: Message) => {
              this.snapshots.value.find(s => s.conversationId == conversationId).lastMessage = data;
              return data;
            }
          )
        );
  }

  refreshSnapshots(){
    var count = 100;
    var skip = 0;

    this.http.get(CONVERSATIONS_URL + "?count=" + count + "&skip=" + skip).pipe(first()).subscribe(
      (data: ConversationSnapshot[]) => {
        this.snapshots.next(data);
      }
    );
  }

  getSnapshots(): Observable<ConversationSnapshot[]>{
    return this.snapshots.asObservable();
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

  conversationSeen(conversationId: string){
    this.http.put(`${CONVERSATION_SEEN_URL}/${conversationId}`, null).pipe(first()).subscribe(
      () => {
        this.snapshots.value.find(s => s.conversationId == conversationId).lastMessage.seen = true;
      }
    );
  }
}
