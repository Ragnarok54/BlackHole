import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IonModal, ModalController } from '@ionic/angular';
import Peer from 'peerjs';
import { User } from 'src/app/models/user/user';
import { ConversationService } from 'src/app/services/conversation.service';
import { RtcService } from 'src/app/services/rtc.service';

@Component({
  selector: 'app-incoming-call',
  templateUrl: './incoming-call.page.html',
  styleUrls: ['./incoming-call.page.scss'],
})
export class IncomingCallPage{
  public caller: User;

  constructor(private router: Router, private rtcService: RtcService, conversationService: ConversationService, private modalController: ModalController) { 
    conversationService.getUser(this.rtcService.mediaCall.peer).subscribe(
      (data: User) => {
        this.caller = data;
      }
    );
  }

  async acceptCall(){
    this.router.navigateByUrl('call');
    await this.rtcService.answerCallAsync();
  }

  async declineCall() {
    await this.rtcService.declineCallAsync();
    this.modalController.dismiss();
  }
}
