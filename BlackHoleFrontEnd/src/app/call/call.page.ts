import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { IonRouterOutlet } from '@ionic/angular';
import { filter } from 'rxjs/operators';
import { User } from '../models/user/user';
import { ConversationService } from '../services/conversation.service';
import { RtcService } from '../services/rtc.service';

@Component({
  selector: 'app-call',
  templateUrl: './call.page.html',
  styleUrls: ['./call.page.scss'],
})
export class CallPage {
  @ViewChild('localVideo') localVideo: ElementRef<HTMLVideoElement>;
  @ViewChild('remoteVideo') remoteVideo: ElementRef<HTMLVideoElement>;
  
  public calling: boolean = true;
  public caller: User;
  public microphoneOn: boolean = true;
  public videoOn: boolean = true;
  public answered: boolean = false;

  constructor(private router: Router, private rtcService: RtcService, private routerOutlet: IonRouterOutlet, conversationService: ConversationService) { 
    conversationService.getUser(this.rtcService.mediaCall.peer).subscribe(
      (data: User) => {
        this.caller = data;
      }
    );
  }

  ionViewWillEnter() {
    this.routerOutlet.swipeGesture = false;

    this.rtcService.localStream.pipe(filter(res => !!res)).subscribe(stream => this.localVideo.nativeElement.srcObject = stream);
    this.rtcService.remoteStream.pipe(filter(res => !!res)).subscribe(
      stream =>{
        this.answered = true;
        this.remoteVideo.nativeElement.srcObject = stream;
      }
    );
    this.rtcService.calling.subscribe(value => this.calling = value);
  }

  toggleMicrophone() {
    this.microphoneOn = !this.microphoneOn;
    this.rtcService.toggleMicrophone();
  }

  toggleVideo() {
    this.videoOn = !this.videoOn;
    this.rtcService.toggleVideo();
  }

  endCall(){
    this.rtcService.endCall();
    this.router.navigateByUrl('');
  }
}
