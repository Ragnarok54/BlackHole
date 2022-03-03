import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';
import { ConversationService } from '../services/conversation.service';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {

  public snapshots: ConversationSnapshot[];

  constructor(private router: Router, private conversationService: ConversationService) { }

  ionViewWillEnter(){
    this.fetchData();
  }

  fetchData(){
    this.conversationService.getSnapshots(100, 0).pipe(first()).subscribe(
      (data: ConversationSnapshot[]) => {
        this.snapshots = data;
      }
    )
  }
}
