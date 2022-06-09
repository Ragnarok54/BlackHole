import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IonModal } from '@ionic/angular';
import Peer from 'peerjs';
import { RtcService } from 'src/app/services/rtc.service';

@Component({
  selector: 'app-incoming-call',
  templateUrl: './incoming-call.page.html',
  styleUrls: ['./incoming-call.page.scss'],
})
export class IncomingCallPage{

  constructor(private router: Router, private rtcService: RtcService) { }

  async acceptCall(){
    this.router.navigateByUrl('call');
    await this.rtcService.answerCallAsync();
  }

  async declineCall(){
    await this.rtcService.declineCallAsync();
  }
}
