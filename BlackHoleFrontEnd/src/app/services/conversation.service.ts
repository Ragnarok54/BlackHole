import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { first, map } from 'rxjs/operators';
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

  sendMessage(text: string, conversationId: string, repliedMessage: Message = null): Observable<Message> {
    var payload = new BaseMessage();
    payload.text = text;
    payload.conversationId = conversationId;
    payload.repliedMessage = repliedMessage;

    return this.http.post(Common.MESSAGE_URL, payload)
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

    this.http.get(Common.CONVERSATIONS_URL + "?count=" + count + "&skip=" + skip).pipe(first()).subscribe(
      (data: ConversationSnapshot[]) => {
        this.snapshots.next(data);
      }
    );
  }

  getSnapshots(): Observable<ConversationSnapshot[]>{
    return this.snapshots.asObservable();
  }

  getMessages(conversationId: string, skip: number, count: number){
    return this.http.get(`${Common.CONVERSATION_MESSAGES_URL}/${conversationId}/${skip}/${count}`);
  }

  getDetails(conversationId: string){
    return this.http.get(`${Common.CONVERSATION_DETAILS_URL}/${conversationId}`);
  }

  getContacts(query: string){
    return this.http.get(`${Common.CONTACTS_URL}${query}`);
  }

  createConversation(userIds: string[]){
    var body = new ConversationModel();
    body.name = Common.newConversationName;
    body.userIds = userIds;

    return this.http.post(`${Common.CONVERSATION_ADD_URL}`, body);
  }

  conversationSeen(conversationId: string){
    this.http.put(`${Common.CONVERSATION_SEEN_URL}/${conversationId}`, null).pipe(first()).subscribe(
      () => {
        this.snapshots.value.find(s => s.conversationId == conversationId).lastMessage.seen = true;
      }
    );
  }
}
