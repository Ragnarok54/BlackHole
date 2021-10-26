import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { first } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.page.html',
  styleUrls: ['./auth.page.scss'],
})
export class AuthPage implements OnInit {

  _isLoading: boolean;
  _didSubmit: boolean;
  _hidePassword: boolean;
  
  constructor(private authService: AuthService, private router: Router, private toastCtrl: ToastController) {
    if(this.authService.isAuthenticated()){
      router.navigateByUrl('/car');
    }
  }

  ngOnInit() {
  }

  ionViewWillEnter(){
    if (this.authService.isAuthenticated()) {
      this.router.navigateByUrl('/home');
    }
  }

  onSubmit(loginForm: NgForm) {
    if (loginForm.valid) {
      this._isLoading = true;

      this.authService.login(loginForm.value.phoneNumber, loginForm.value.password)
      .pipe(first())
      .subscribe(
        () => {
          if(this.authService.isAuthenticated()){
            this.router.navigateByUrl("/home");
          }
          this._isLoading = false;
          loginForm.reset();
        },
        () => {
          this.toastCtrl.create({
            message: 'Incorrect phone and password combination',
            duration: 5000,
            position: 'bottom',
            color: 'danger',
            buttons: ['Dismiss']
          }).then((el) => el.present());
          
          this._isLoading = false;
        }
      );
    }    
  }

}
