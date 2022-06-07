import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IncomingCallPage } from './incoming-call.page';

const routes: Routes = [
  {
    path: '',
    component: IncomingCallPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IncomingCallPageRoutingModule {}
