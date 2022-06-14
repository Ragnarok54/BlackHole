import { Component, OnInit } from '@angular/core';
import { ConversationService } from 'src/app/services/conversation.service';
import { first } from 'rxjs/operators';
import { Contact } from 'src/app/models/conversation/contact';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-new-conversation',
  templateUrl: './new-conversation.page.html',
})
export class NewConversationPage implements OnInit {
  public contacts: Contact[];

  constructor(private modalController: ModalController, private conversationService: ConversationService) { }

  ngOnInit() {
    this.searchContacts(null);
  }

  searchContacts(event){
    var value = event != undefined ? event.detail.value : '';
    this.conversationService.getContacts(value).pipe(first()).subscribe(
      (data: Contact[]) => {
        this.contacts = data;
      }
    );
  }

  createConversation(){
    var participants = this.contacts.filter(c => c.isSelected).map(c => c.userId);

    this.conversationService.createConversation(participants).pipe(first()).subscribe(
      () => {
        this.modalController.dismiss();
        this.conversationService.refreshSnapshots();
      }
    );
  }
}
