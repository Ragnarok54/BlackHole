import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { ChatService } from './services/chat.service';
import { RtcService } from './services/rtc.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  private showModal: boolean;

  constructor(private authService: AuthService, private router: Router, private chatService: ChatService, private rtcService: RtcService) {
    rtcService.incomingCall.subscribe(
      (data) => {
        this.showModal = data;
      }
    );
  }
  
  ngOnInit(): void { }

  onLogout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }

  acceptCall(): void {
    this.showModal = false;
    this.router.navigateByUrl('call');
    this.rtcService.answerCall();
  }

  declineCall(): void {
    this.showModal = false;
    this.rtcService.declineCall();
  }
}
