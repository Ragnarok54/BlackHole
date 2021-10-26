import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Common } from 'src/app/shared/common';
import { RegisterUser } from 'src/app/models/user/registerUser';
import { AuthService } from '../auth.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit{
  public isLoading: boolean;

  public Common(){
    return Common;
  }
  
  constructor(private authService: AuthService, private router: Router, private toastService: ToastService) { }

  ngOnInit() {
  }


  onRegister(registerForm: NgForm){
    this.isLoading = true;
    var model = new RegisterUser(registerForm.value.firstName,
                                 registerForm.value.lastName,
                                 registerForm.value.phoneNumber,
                                 registerForm.value.password);
                          
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
}
