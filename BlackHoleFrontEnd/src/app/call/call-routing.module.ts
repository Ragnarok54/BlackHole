import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CallPage } from './call.page';

const routes: Routes = [
  {
    path: '',
    component: CallPage
  },  {
    path: 'incoming-call',
    loadChildren: () => import('./incoming-call/incoming-call.module').then( m => m.IncomingCallPageModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CallPageRoutingModule {}
