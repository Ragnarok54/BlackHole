import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { IonModal, IonPopover } from '@ionic/angular';
import { first } from 'rxjs/operators';
import { Contact } from '../models/conversation/contact';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';
import { BaseMessage } from '../models/message/baseMessage';
import { ChatService } from '../services/chat.service';
import { ConversationService } from '../services/conversation.service';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild(IonModal) modal: IonModal;
  @ViewChild(IonPopover) popover: IonPopover;

  public snapshots: ConversationSnapshot[];
  public contacts: Contact[];
  
  constructor(private router: Router, private conversationService: ConversationService, private chatService: ChatService) {
    chatService.retrieveMappedObject().subscribe(
      (receivedObj: BaseMessage) => {
        var oldSnapshot = this.snapshots.find(s => s.conversationId == receivedObj.conversationId);
        this.snapshots = this.snapshots.filter(s => s.conversationId != receivedObj.conversationId);
        oldSnapshot.lastMessage.time = receivedObj.time;
        oldSnapshot.lastMessage.text = receivedObj.text;
        //oldSnapshot.lastMessage.highlight = true;
        this.snapshots.unshift(oldSnapshot);
      }
    )
  }

  openModal(){
    this.popover.dismiss();
    this.modal.present();
    this.searchContacts(null);
  }

  ionViewWillEnter(){
    this.fetchData();
  }

  fetchData(){
    this.conversationService.getSnapshots(100, 0).pipe(first()).subscribe(
      (data: ConversationSnapshot[]) => {
        this.snapshots = data;
      }
    )
  }

  createConversation(){
    var participants = this.contacts.filter(c => c.isSelected).map(c => c.userId);

    this.conversationService.createConversation(participants).pipe(first()).subscribe(
      () => {
        this.modal.dismiss();
        this.fetchData();
      }
    );
  }

  searchContacts(query: string){
    this.conversationService.getContacts(query).pipe(first()).subscribe(
      (data: Contact[]) => {
        this.contacts = data;
      }
    );
  }
}
