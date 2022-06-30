import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterUser } from '../models/user/registerUser';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';
import { first, map } from 'rxjs/operators';
import { Common } from '../shared/common';
import { ConversationService } from '../services/conversation.service';
import { User } from '../models/user/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit {
  public isLoading: boolean = false;
  public pictureBase64: string;

  public profileForm: NgForm;

  constructor(private router: Router, private authService: AuthService, private toastService: ToastService, private convService: ConversationService) { }

  ngOnInit() {
    this.convService.getUser(this.authService.currentUserValue().userId)
    .pipe(first())
    .subscribe(
      (data: User) => {
        this.profileForm.value.firstName = data.firstName;
        this.profileForm.value.lastName = data.lastName;
        this.profileForm.value.phoneNumber = data.phoneNumber;
        this.profileForm.value.phoneNumber = data.phoneNumber;
      }
    );
  }

  onRegister(registerForm: NgForm){
    this.isLoading = true;
    var model = new RegisterUser(registerForm.value.firstName,
                                 registerForm.value.lastName,
                                 registerForm.value.phoneNumber,
                                 registerForm.value.password,
                                 this.pictureBase64);
                          
    this.authService.register(model).pipe(first()).subscribe(
      data =>{
        if(data){
          this.toastService.createToast('Register succesful. Please log in.', 'success', 'bottom');
        }
        else{
          this.toastService.createToast('Register failed', 'danger', 'bottom');
        }
        this.isLoading = false;
      },
      () => {
        this.toastService.createToast('Register failed', 'danger', 'bottom');
     
        this.isLoading = false;
      }
    )
    
  }

  logout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }
  public Common(){
    return Common;
  }
}
