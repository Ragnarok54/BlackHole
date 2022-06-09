import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { filter } from 'rxjs/operators';
import { RtcService } from '../services/rtc.service';

@Component({
  selector: 'app-call',
  templateUrl: './call.page.html',
  styleUrls: ['./call.page.scss'],
})
export class CallPage implements OnInit {
  @ViewChild('localVideo') localVideo: ElementRef<HTMLVideoElement>;
  @ViewChild('remoteVideo') remoteVideo: ElementRef<HTMLVideoElement>;
  
  public calling: boolean = true;

  constructor(private rtcService: RtcService) { 
  }

  ngOnInit() {
    this.rtcService.localStream.pipe(filter(res => !!res)).subscribe(stream => this.localVideo.nativeElement.srcObject = stream);
    this.rtcService.remoteStream.pipe(filter(res => !!res)).subscribe(stream => this.remoteVideo.nativeElement.srcObject = stream);
    this.rtcService.calling.subscribe(value => this.calling = value);
  }

  toggleMicrophone() {
    this.rtcService.toggleMicrophone();
  }

  toggleVideo() {
    this.rtcService.toggleVideo();
  }

  endCall(){
    this.rtcService.declineCallAsync();
  }
}
