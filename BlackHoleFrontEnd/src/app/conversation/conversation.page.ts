import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavParams } from '@ionic/angular';
import { first } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { ConversationSnapshot } from '../models/conversation/conversationSnapshot';
import { Message } from '../models/message/message';
import { ChatService } from '../services/chat.service';
import { ConversationService } from '../services/conversation.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.page.html',
  styleUrls: ['./conversation.page.scss'],
  providers: [
    NavParams
  ]
})
export class ConversationPage {
  public conversationId: string;

  constructor(private route: ActivatedRoute, private conversationService: ConversationService, private chatService: ChatService, private auth: AuthService) {
    this.route.paramMap.subscribe(params => {
      this.conversationId = params.get('conversationId');
    });
    this.x();
  }

  async x(){
    await this.chatService.connect(this.auth.currentUserValue().token);

    this.chatService.retrieveMappedObject().subscribe(
      (receivedObj: Message) => {
        debugger;
        console.log(receivedObj);
      }
    );
  }

  onSend(textCtrl) {
    //text = text.trimEnd();
    if (textCtrl.value.length > 0){
      this.conversationService.sendMessage(textCtrl.value, this.conversationId)
                              .pipe(first())
                              .subscribe(); 
    }
  }
}
