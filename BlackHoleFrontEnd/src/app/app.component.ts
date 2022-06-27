import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { SplashScreen } from '@capacitor/splash-screen';
import { Capacitor } from '@capacitor/core';
import { RtcService } from './services/rtc.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  public showModal: boolean;
  public isIos: boolean;

  constructor(private authService: AuthService, private router: Router, private rtcService: RtcService) {
    this.isIos = Capacitor.getPlatform() == 'ios';
    
    if (Capacitor.isPluginAvailable('splash-screen')){
      SplashScreen.hide();
    }
  }

  onLogout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }
}
