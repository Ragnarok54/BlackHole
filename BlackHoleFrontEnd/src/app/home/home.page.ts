import { AfterContentInit, AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Capacitor } from '@capacitor/core';
import { IonModal, IonPopover, IonRouterOutlet, ModalController } from '@ionic/angular';
import { first } from 'rxjs/operators';
import { Contact } from '../models/conversation/contact';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';
import { ConversationService } from '../services/conversation.service';
import { NewConversationPage } from './new-conversation/new-conversation.page';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild(IonPopover) popover: IonPopover;

  public isMobile: boolean;
  public snapshots: ConversationSnapshot[];
  public contacts: Contact[];
  public pictures: Map<string, string> = new Map();

  constructor(private router: Router, private conversationService: ConversationService, private ionRouterOutlet: IonRouterOutlet, private modalController: ModalController) {
    this.isMobile = Capacitor.getPlatform() == 'ios';
  }

  ionViewWillEnter () {
    this.ionRouterOutlet.swipeGesture = false;

    this.conversationService.getSnapshots().subscribe(
      (data: ConversationSnapshot[]) => {
        this.snapshots = data;
        this.snapshots.forEach(
          (snapshot) => {
            this.conversationService.getPicture(snapshot.conversationId).pipe(first()).subscribe(
              (picture: string) => {
                this.pictures.set(snapshot.conversationId, picture == null ? picture : `data:image/jpg;base64,${picture}`);
              }
            );
          }
        )
      }
    );

    this.conversationService.refreshSnapshots();
  }

  
  async openModal() {
    var modal = this.modalController.create({
      component: NewConversationPage,
      mode: 'ios'
    }
  );
    await this.popover.dismiss();
    (await modal).present();
  }

  searchContacts(event){
    var value = event != undefined ? event.detail.value : '';
    this.conversationService.getContacts(value).pipe(first()).subscribe(
      (data: Contact[]) => {
        this.contacts = data;
      }
    );
  }

  navigate(snapshot: ConversationSnapshot){
    this.router.navigateByUrl(`/chat/${snapshot.conversationId}`);
  }

}
