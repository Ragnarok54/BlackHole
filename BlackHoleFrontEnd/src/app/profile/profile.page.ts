import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
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
  public profile: User = new User();
  
  constructor(private router: Router, private authService: AuthService, private toastService: ToastService, private convService: ConversationService) { }
  
  ngOnInit(): void {
    this.loadProfileData();
  }

  onEdit(form: NgForm){
    this.isLoading = true;
    var model = new User({
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      picture: this.pictureBase64
    });

    this.authService.edit(model).pipe(first()).subscribe(
      () => {
        this.loadProfileData();
        this.isLoading = false;
        this.toastService.createToast('Profile updated', 'success', 'bottom');
      },
      () => {
        this.toastService.createToast('Edit failed', 'danger', 'bottom');
     
        this.isLoading = false;
      }
    )
    
  }

  loadProfileData() {
    this.convService.getUser(this.authService.currentUserValue().userId)
    .pipe(first())
    .subscribe(
      (data: User) => {
        this.profile = data;
      }
    );
  }

  logout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }

  public Common(){
    return Common;
  }

  onDocumentUpload(event) {
    const reader = new FileReader();

    reader.readAsDataURL(event.target.files.item(0));

    reader.onload = () => {
      this.pictureBase64 = reader.result.toString().split('base64,').pop();
    }
  }
}
