import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NewConversationPage } from './new-conversation.page';

const routes: Routes = [
  {
    path: '',
    component: NewConversationPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewConversationPageRoutingModule {}
