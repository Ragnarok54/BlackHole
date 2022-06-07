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
export class AppComponent {
  public showModal: boolean;

  constructor(private authService: AuthService, private router: Router, private chatService: ChatService, private rtcService: RtcService) {
  }

  onLogout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }
}
