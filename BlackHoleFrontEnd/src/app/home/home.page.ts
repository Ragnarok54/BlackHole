import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {

  public snapshots: ConversationSnapshot[];

  constructor(private router: Router) {
    this.snapshots = [
      new ConversationSnapshot("1", "Radu Popescu", "Salutare", new Date()),
      new ConversationSnapshot("2", "Alex Costescu", "Un mesaj", new Date()),
      new ConversationSnapshot("3", "Gabriela Dincu", "Pa pa", new Date()),

    ];
  }
}
