import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { NewConversationPageRoutingModule } from './new-conversation-routing.module';

import { NewConversationPage } from './new-conversation.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NewConversationPageRoutingModule
  ],
  declarations: [NewConversationPage]
})
export class NewConversationPageModule {}
