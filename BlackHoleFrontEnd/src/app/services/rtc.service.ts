import { Injectable, OnDestroy } from '@angular/core';
import Peer from "peerjs"; //tsconfig.json "esModuleInterop": true,
import { Subject } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})

export class RtcService implements OnDestroy {
  private peerJsOptions: Peer.PeerJSOption = {
    debug: 10,
    config: {
      iceServers: [
        {
          urls: [
            'stun:stun1.l.google.com:19302',
            'stun:stun2.l.google.com:19302',
          ],
        }
      ]
    }
  };

  private peer: Peer;
  private mediaCall: Peer.MediaConnection;

  private _localStream: BehaviorSubject<MediaStream> = new BehaviorSubject(null);
  public localStream$ = this._localStream.asObservable();
  private _remoteStream: BehaviorSubject<MediaStream> = new BehaviorSubject(null);
  public remoteStream$ = this._remoteStream.asObservable();

  private _isCallStarted = new Subject<boolean>();
  public isCallStarted$ = this._isCallStarted.asObservable();

  private _incomingCall = new Subject<boolean>();
  public incomingCall = this._incomingCall.asObservable();

  constructor(authService: AuthService) {
    authService.currentUser.subscribe(
      (user) => {
        if (user !== null) {
          try {
            this.peer = new Peer(user.userId, this.peerJsOptions);
            this.peer.on('call',
              () => {
                this._incomingCall.next(true);
              }
            );
          } catch (error) {
            console.error(error);
          }
        } else {
          this.peer = null;
        }
      }
    )
  }

  public async call(userId: string) {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      const connection = this.peer.connect(userId);

      connection.on('error', err => {
        console.error(err);
      });

      this.mediaCall = this.peer.call(userId, stream);
      if (!this.mediaCall) {
        throw new Error('Unable to connect to remote peer');
      }

      this._localStream.next(stream);
      this._isCallStarted.next(true);

      this.mediaCall.on('stream',
        (remoteStream) => {
          this._remoteStream.next(remoteStream);
        }
      );

      this.mediaCall.on('error',
        err => {
          console.error(err);
          this._isCallStarted.next(false);
        }
      );

      this.mediaCall.on('close', () => this.onCallClose());
    }
    catch (ex) {
      console.error(ex);
      this._isCallStarted.next(false);
    }
  }

  public async answerCall() {
    try {
      this._incomingCall.next(false);
      const stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      this._localStream.next(stream);
      this.peer.on('call', async (call) => {
        console.log("GG");
        this.mediaCall = call;
        this._isCallStarted.next(true);

        this.mediaCall.answer(stream);
        this.mediaCall.on('stream', (remoteStream) => {
          this._remoteStream.next(remoteStream);
        });
        this.mediaCall.on('error', err => {
          this._isCallStarted.next(false);
          console.error(err);
        });
        this.mediaCall.on('close', () => this.onCallClose());
      });
    }
    catch (ex) {
      console.error(ex);
      this._isCallStarted.next(false);
    }
  }

  public declineCall() {
    try {
      debugger;
      this._incomingCall.next(false);
      this.peer;
    }
    catch (ex) {
      this._isCallStarted.next(false);
    }
  }

  public closeMediaCall() {
    this.mediaCall?.close();

    if (!this.mediaCall) {
      this.onCallClose();
    }

    this._isCallStarted.next(false);
  }

  private onCallClose() {
    this._remoteStream?.value.getTracks().forEach(track => {
      track.stop();
    });
    this._localStream?.value.getTracks().forEach(track => {
      track.stop();
    });
  }

  ngOnDestroy() {
    this.mediaCall?.close();
    this.peer?.disconnect();
    this.peer?.destroy();
   }
}
