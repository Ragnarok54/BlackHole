import { Injectable, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Message } from '../models/message/message';
import { API_URL } from 'src/environments/environment';
import { AuthService } from '../auth/auth.service';
import { DefaultHttpClient, HttpRequest, HttpResponse, IHttpConnectionOptions } from '@microsoft/signalr';


@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private connection: any;

  private receivedMessageObject: Message = new Message();
  private sharedObj = new Subject<Message>();

  constructor(private http: HttpClient) {
  }

  public async connect(token: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:44340/Messages/Hub", {
        accessTokenFactory: () => token
      } as IHttpConnectionOptions)
      .configureLogging(signalR.LogLevel.Information)
      .build()

    this.connection.onclose(async () => {
      await this.start();
    });
    this.connection.on("ReceiveOne", (user, text) => { this.mapReceivedMessage(user, text); });
    this.start();
  }

  public async start() {
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

  public retrieveMappedObject(): Observable<Message> {
    return this.sharedObj.asObservable();
  }

}
