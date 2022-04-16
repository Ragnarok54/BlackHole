import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { InfiniteScrollCustomEvent, IonContent, IonInfiniteScroll, IonInfiniteScrollContent, IonList, NavParams } from '@ionic/angular';
import { Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { Message } from '../models/message/message';
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
  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;
  @ViewChild(IonContent) content: IonContent;

  private messagesToDisplay: number = 25;
  public conversationId: string;
  public conversationName: string = 'daw';
  public messages: Message[] = [];
  public currentUserId = this.authService.currentUserValue().userId;

  constructor(private route: ActivatedRoute, private conversationService: ConversationService, private authService: AuthService) {
    this.route.paramMap.subscribe(params => {
      this.conversationId = params.get('conversationId');
    });
  }

  ionViewWillEnter() {
    this.infiniteScroll.disabled = true;
    this.conversationService.getName(this.conversationId).pipe(
      map(
        (data: string) => {
          this.conversationName = data;
        }
      )
    ).subscribe();

    this.conversationService.getMessages(this.conversationId, this.messages.length, this.messagesToDisplay)
      .pipe(
        map(
          (data: Message[]) => {
            // reverse array for displaying messages
            this.messages = this.messages.concat(data.reverse());
            setTimeout(() => {
              this.content.scrollToBottom().then(() => this.infiniteScroll.disabled = this.messages.length != this.messagesToDisplay);
            }, 100);
          }))
      .subscribe();
  }

  onSend(textCtrl: FormControl) {
    var text = textCtrl.value.toString().trimEnd().trimLeft();

    if (text.length > 0) {
      this.conversationService.sendMessage(textCtrl.value, this.conversationId)
        .pipe(first())
        .subscribe(
          (data: Message) => {
            this.messages.push(data);
            textCtrl.reset();
          }
        );
    }
  }

  loadMessages() {
    this.conversationService.getMessages(this.conversationId, this.messages.length, this.messagesToDisplay)
      .pipe(
        map(
          (data: Message[]) => {
            this.messages = data.reverse().concat(this.messages);
            this.infiniteScroll.complete();

            // App logic to determine if all data is loaded and disable the infinite scroll
            if (data.length !== this.messagesToDisplay) {
              this.infiniteScroll.disabled = true;
            }
          })).subscribe();
  }

  // markMessagesAsSeen(): Observable<any>{
  //   if (this.messages.some(m => m.)
  // }
}
