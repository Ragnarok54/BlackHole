import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { BaseMessage } from '../models/message/baseMessage';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message/message';


@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private connection: signalR.HubConnection;

  private receivedMessageObject: Message = new Message();
  private sharedObj = new Subject<Message>();

  public async connect(token: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseHubUrl}/Messages/Hub`, {
        accessTokenFactory: () => token
      } as IHttpConnectionOptions)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.onclose(async () => {
      await this.start();
    });
    this.connection.on("ReceiveOne", (message: Message) => { this.mapReceivedMessage(message); });
    this.start();
  }

  public disconnect(){
    this.connection.stop();
  }

  private async start() {
    try {
      await this.connection.start();
    } catch (err) {
      console.log(err);
      setTimeout(() => this.start(), 5000);
    }
  }

  private mapReceivedMessage(message: Message): void {
    this.receivedMessageObject = message;
    
    this.sharedObj.next(this.receivedMessageObject);
  }

  public retrieveMappedObject(): Observable<Message> {
    return this.sharedObj.asObservable();
  }

}
