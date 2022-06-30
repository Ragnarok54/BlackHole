import { Component, OnInit } from '@angular/core';
import { ConversationService } from 'src/app/services/conversation.service';
import { first } from 'rxjs/operators';
import { Contact } from 'src/app/models/conversation/contact';
import { ModalController } from '@ionic/angular';
import { Router } from '@angular/router';
import { Common } from 'src/app/shared/common';

@Component({
  selector: 'app-new-conversation',
  templateUrl: './new-conversation.page.html',
})
export class NewConversationPage implements OnInit {
  public contacts: Contact[];
  public groupName: string = "New Group Chat";
  public isMobile: boolean = Common.IS_MOBILE;
  
  constructor(public modalController: ModalController, private conversationService: ConversationService, private router: Router) { }

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

    this.conversationService.createConversation(participants, this.groupName)
      .pipe(first())
      .subscribe(
        () => {
          this.modalController.dismiss();
          this.conversationService.refreshSnapshots();
        },
        (error) => {
          this.modalController.dismiss();
          this.router.navigateByUrl(`/chat/${error.error}`);
        }
    );
  }

  shouldDisplayName() {
    return this.contacts ? this.contacts.filter(c => c.isSelected).length > 1 : false;
  }
}
