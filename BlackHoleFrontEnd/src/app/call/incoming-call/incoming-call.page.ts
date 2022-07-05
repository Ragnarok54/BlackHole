import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IonModal, ModalController } from '@ionic/angular';
import Peer from 'peerjs';
import { User } from 'src/app/models/user/user';
import { ConversationService } from 'src/app/services/conversation.service';
import { RtcService } from 'src/app/services/rtc.service';
import { StatusService } from 'src/app/services/status.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-incoming-call',
  templateUrl: './incoming-call.page.html',
  styleUrls: ['./incoming-call.page.scss'],
})
export class IncomingCallPage{
  public caller: User;
  public picture: string;

  constructor(private router: Router, private rtcService: RtcService, conversationService: ConversationService, private modalController: ModalController, private statusService: StatusService) { 
    conversationService.getUser(this.rtcService.mediaCall.peer).subscribe(
      (data: User) => {
        this.caller = data;
        conversationService.getUserPicture(this.caller.userId).pipe(first()).subscribe(
          (picture: string) => {
            this.picture = picture == null ? null : `data:image/jpg;base64,${picture}`;
          }
        )
      }
    );
  }

  async acceptCall(){
    this.router.navigateByUrl('call');
    await this.rtcService.answerCallAsync();
    this.modalController.dismiss();
  }

  async declineCall() {
    //await this.rtcService.declineCallAsync();
    
    this.statusService.reject(this.caller.userId);
    this.modalController.dismiss();
  }
}
