import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { first, map } from 'rxjs/operators';
import { ConversationModel } from '../models/conversation/conversationModel';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';
import { BaseMessage } from '../models/message/baseMessage';
import { Message } from '../models/message/message';
import { User } from '../models/user/user';
import { Common } from '../shared/common';
import { ChatService } from './chat.service';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  private snapshots: BehaviorSubject<ConversationSnapshot[]> = new BehaviorSubject<ConversationSnapshot[]>([]);

  constructor(private http: HttpClient, chatService: ChatService) {
    chatService.retrieveMappedObject().subscribe(
      (receivedObj: BaseMessage) => {
        var oldSnapshot = this.snapshots.value.find(s => s.conversationId == receivedObj.conversationId);
        var tempSnapshots = this.snapshots.value.filter(s => s.conversationId != receivedObj.conversationId);
        if (oldSnapshot) {
          oldSnapshot.lastMessage = receivedObj;
          tempSnapshots.unshift(oldSnapshot);
  
          this.snapshots.next(tempSnapshots);
        } else {
          this.refreshSnapshots();
        }
      }
    );
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
              data.seen = true;
              var snapshot = this.snapshots.value.find(s => s.conversationId == conversationId);

              if (snapshot) {
                snapshot.lastMessage = data;
              }
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

  getUser(userId: string){
    return this.http.get(`${Common.USER_URL}/${userId}`);
  }

  getContacts(query: string){
    return this.http.get(`${Common.CONTACTS_URL}${query}`);
  }

  getPicture(conversationId: string) {
    return this.http.get(`${Common.CONVERSATION_PICTURE_URL}/${conversationId}`, {responseType: 'text'});
  }

  getUserPicture(userId: string) {
    return this.http.get(`${Common.USER_PICTURE_URL}/${userId}`, {responseType: 'text'});
  }

  createConversation(userIds: string[], name: string = null){
    var body = new ConversationModel();
    body.name = name;
    body.users = userIds.map(u => new User({ userId: u }));

    return this.http.post(`${Common.CONVERSATION_ADD_URL}`, body);
  }

  conversationSeen(conversationId: string){
    this.http.put(`${Common.CONVERSATION_SEEN_URL}/${conversationId}`, null).pipe(first()).subscribe(
      () => {
        var snapshot = this.snapshots.value.find(s => s.conversationId == conversationId);

        if (snapshot){
          snapshot.lastMessage.seen = true;
        }
      }
    );
  }
}
