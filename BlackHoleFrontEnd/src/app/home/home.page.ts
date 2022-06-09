import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Capacitor } from '@capacitor/core';
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

  public isMobile: boolean;
  public snapshots: ConversationSnapshot[];
  public contacts: Contact[];
  
  constructor(private router: Router, private conversationService: ConversationService, private chatService: ChatService) {
    this.isMobile = Capacitor.getPlatform() == 'ios';
  }

  ngOnInit() {
    this.conversationService.getSnapshots().subscribe(
      (data: ConversationSnapshot[]) => {
        this.snapshots = data;
      }
    );
  }

  openModal(){
    this.popover.dismiss();
    this.modal.present();
    this.searchContacts(null);
  }

  createConversation(){
    var participants = this.contacts.filter(c => c.isSelected).map(c => c.userId);

    this.conversationService.createConversation(participants).pipe(first()).subscribe(
      () => {
        this.modal.dismiss();
        this.conversationService.refreshSnapshots();
      }
    );
  }

  searchContacts(event){
    var value = event != undefined ? event.detail.value : '';
    this.conversationService.getContacts(value).pipe(first()).subscribe(
      (data: Contact[]) => {
        this.contacts = data;
      }
    );
  }
}
