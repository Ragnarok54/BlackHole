import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth/auth.service';
import { ChatService } from './services/chat.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router, private chatService: ChatService) {}
  
  ngOnInit(): void { }

  onLogout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }
}
