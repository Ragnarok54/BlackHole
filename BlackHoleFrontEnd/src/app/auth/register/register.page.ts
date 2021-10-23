import { Component, OnInit } from '@angular/core';
import { Common } from 'src/app/common/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit{
  constructor() { }

  ngOnInit() {
  }

  public Common(){
    return Common;
  }
}
