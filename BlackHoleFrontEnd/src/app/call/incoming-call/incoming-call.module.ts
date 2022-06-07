import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { IncomingCallPageRoutingModule } from './incoming-call-routing.module';

import { IncomingCallPage } from './incoming-call.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    IncomingCallPageRoutingModule
  ],
  declarations: [IncomingCallPage]
})
export class IncomingCallPageModule {}
