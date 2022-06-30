import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { InfiniteScrollCustomEvent, IonContent, IonInfiniteScroll, IonInfiniteScrollContent, IonList, IonRouterOutlet, NavParams } from '@ionic/angular';
import { Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Message } from '../models/message/message';
import { ConversationService } from '../services/conversation.service';
import { RtcService } from '../services/rtc.service';
import { ConversationModel } from '../models/conversation/conversationModel';
import { Common } from '../shared/common';
import { StatusService } from '../services/status.service';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.page.html',
  styleUrls: ['./conversation.page.scss'],
  providers: [
    NavParams
  ]
})
export class ConversationPage {
  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;
  @ViewChild(IonContent) content: IonContent;
  
  public isMobile: boolean;
  private messagesToDisplay: number = 25;
  public conversationId: string;
  public conversation: ConversationModel;
  public messages: Message[] = [];
  public repliedMessage: Message = null;
  public currentUserId = this.authService.currentUserValue().userId;
  public isOnline: any = [];
  public otherConversationCounter: number = 0;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private conversationService: ConversationService,
              private authService: AuthService,
              private rtcService: RtcService,
              private ionRouterOutlet: IonRouterOutlet,
              private statusService: StatusService,
              private chatService: ChatService) {
    this.isMobile = Common.IS_MOBILE;

    this.route.paramMap.subscribe(params => {
      this.conversationId = params.get('conversationId');
    });

    this.chatService.retrieveMappedObject().subscribe(
      (message: Message) => {
        if (message.conversationId == this.conversationId) {
          this.messages.push(message);
        } else {
          this.otherConversationCounter++;
          this.conversationService.conversationSeen(this.conversationId);
        }
      }
    )
  }

  ionViewWillEnter() {
    this.conversationService.getDetails(this.conversationId).pipe(
      map(
        (data: ConversationModel) => {
          this.conversation = data;
        }
      )
    ).subscribe(
      () => {
        this.statusService.activeUsers.subscribe(list => {
          this.conversation.users.forEach(
            (user) => {
              this.isOnline[user.userId] = list.has(user.userId) ? list.get(user.userId) : false;
            }
          )
        });
      }
    );

    this.ionRouterOutlet.swipeGesture = true;
    this.infiniteScroll.disabled = true;

    this.conversationService.getMessages(this.conversationId, this.messages.length, this.messagesToDisplay)
      .pipe(first())
      .subscribe(
        (data: Message[]) => {
          // Reverse array for displaying messages
          this.messages = this.messages.concat(data.reverse());
          
          // Data has been received, send the seen request
          this.conversationService.conversationSeen(this.conversationId);
          this.infiniteScroll.disabled = this.messages.length != this.messagesToDisplay;
          setTimeout(() => {
            this.content.scrollToBottom().then(() => this.infiniteScroll.disabled = this.messages.length != this.messagesToDisplay);
          }, 75);
        }
      );
  }

  getPhoto(userId) {
    var user = this.conversation.users.find(u => u.userId == userId);

    return `data:image/jpg;base64,${user.picture}`;
  }

  onSend(textCtrl) {
    var text = textCtrl.value.toString().trimEnd().trimLeft();

    if (text.length > 0) {
      this.conversationService.sendMessage(textCtrl.value, this.conversationId, this.repliedMessage)
        .subscribe( 
          (data) => {
            this.repliedMessage = null;
            this.messages.push(data);
            textCtrl.reset();
            this.content.scrollToBottom();
          }
        );
    }
  }

  loadMessages() {
    this.conversationService.getMessages(this.conversationId, this.messages.length, this.messagesToDisplay)
      .pipe(first())
      .subscribe(
        (data: Message[]) => {
          this.messages = data.reverse().concat(this.messages);
          this.infiniteScroll.complete();

          // App logic to determine if all data is loaded and disable the infinite scroll
          if (data.length !== this.messagesToDisplay) {
            this.infiniteScroll.disabled = true;
          }
        }
      );
  }

  async call(){
    await this.rtcService.callAsync(this.conversation.users[0].userId, true);
    this.router.navigateByUrl('call');
  }

  reply(message: Message) {
    this.repliedMessage = message;
  }

  cancelReply() {
    this.repliedMessage = null;
  }
}
