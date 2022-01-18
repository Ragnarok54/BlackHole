import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavParams } from '@ionic/angular';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.page.html',
  styleUrls: ['./conversation.page.scss'],
  providers: [
    NavParams
  ]
})
export class ConversationPage implements OnInit {
  public conversationId: string;


  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      if (params['conversationId']) {
        this.conversationId = params['conversationId'];
      }
    });
  }

  ngOnInit() { }

  // ionViewWillEnter(){

  // }

  // initData(){

  // }
}
