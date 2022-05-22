import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { BaseMessage } from '../models/message/baseMessage';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private connection: any;

  private receivedMessageObject: BaseMessage = new BaseMessage();
  private sharedObj = new Subject<BaseMessage>();

  public async connect(token: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseHubUrl}/Messages/Hub`, {
        accessTokenFactory: () => token
      } as IHttpConnectionOptions)
      .configureLogging(signalR.LogLevel.Information)
      .build()

    this.connection.onclose(async () => {
      await this.start();
    });
    this.connection.on("ReceiveOne", (user: string, text: string) => { this.mapReceivedMessage(user, text); });
    this.start();
  }

  public disconnect(){
    this.connection.disconnect();
  }

  private async start() {
    try {
      await this.connection.start();
      console.log("connected");
    } catch (err) {
      console.log(err);
      setTimeout(() => this.start(), 5000);
    }
  }

  private mapReceivedMessage(user: string, text: string): void {
    this.receivedMessageObject.conversationId = user;
    this.receivedMessageObject.text = text;

    this.sharedObj.next(this.receivedMessageObject);
  }

  public retrieveMappedObject(): Observable<BaseMessage> {
    return this.sharedObj.asObservable();
  }

}
