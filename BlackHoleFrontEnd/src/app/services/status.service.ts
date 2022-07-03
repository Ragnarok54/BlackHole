import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { BaseMessage } from '../models/message/baseMessage';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { RtcService } from './rtc.service';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class StatusService {

  private connection: signalR.HubConnection;
  private _activeUserList: Map<string, boolean> = new Map();
  private _activeUsers = new BehaviorSubject<Map<string, boolean>>(this._activeUserList);
  public activeUsers = this._activeUsers.asObservable();

  constructor(private rtcService: RtcService, private authService: AuthService) { 
    this.authService.currentUser.subscribe(user => {
      if (user) {
        this.connect(user.token);
      } else {
        this.disconnect();
      }
    }
    );
  }

  public async connect(token: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseHubUrl}/Status/Hub`, {
        accessTokenFactory: () => token
      } as IHttpConnectionOptions)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.onclose(async () => {
      await this.start();
    });

    this.connection.on("StatusHubAllActive",
      (users: string[]) => { 
        users.forEach(userId => this._activeUserList.set(userId, true));

        this._activeUsers.next(this._activeUserList);
      }
    );
    this.connection.on("StatusUpdateActive", (user: string) => { this.updateUserStatus(user, true); });
    this.connection.on("StatusUpdateInactive", (user: string) => { this.updateUserStatus(user, false); });

    this.connection.on('CallRejected', () => { this.rtcService.endCall(); });
    this.connection.on('CallClosed', () => { this.rtcService.endCall(); });

    this.start();
  }

  public async reject(userId: string) {
    await this.connection.send('Reject', userId);
    this.rtcService.endCall();
  }

  public async close(userId: string) {
    await this.connection.send('Close', userId);
    this.rtcService.endCall();
  }

  public disconnect(){
    this.connection.stop();
  }

  private async start() {
    try {
      await this.connection.start();
    } catch (err) {
      setTimeout(() => this.start(), 5000);
    }
  }

  private updateUserStatus(userId: string, active: boolean): void {
    this._activeUserList.set(userId, active);

    this._activeUsers.next(this._activeUserList);
  }
}
