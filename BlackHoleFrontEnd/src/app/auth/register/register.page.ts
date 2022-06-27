import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Common } from 'src/app/shared/common';
import { RegisterUser } from 'src/app/models/user/registerUser';
import { AuthService } from '../../services/auth.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage {
  public isLoading: boolean;
  private pictureBase64 : string;

  public Common(){
    return Common;
  }
  
  constructor(private authService: AuthService, private router: Router, private toastService: ToastService) { }

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
          this.router.navigateByUrl('/auth');
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

  onDocumentUpload(files) {
    const reader = new FileReader();

    reader.readAsDataURL(files.item(0));

    reader.onload = () => {
      this.pictureBase64 = reader.result.toString().split('base64,').pop();
    }
  }
}
