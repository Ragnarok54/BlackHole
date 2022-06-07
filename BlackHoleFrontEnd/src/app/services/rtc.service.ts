import { Injectable, OnDestroy } from '@angular/core';
import { ModalController } from '@ionic/angular';
import Peer from "peerjs"; //tsconfig.json "esModuleInterop": true,
import { Subject } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { IncomingCallPage } from '../call/incoming-call/incoming-call.page';
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

  private _isCallInProgress = new Subject<boolean>();
  public isCallInProgress = this._isCallInProgress.asObservable();

  private _incomingCall = new Subject<Peer.MediaConnection>();
  public incomingCall = this._incomingCall.asObservable();

  private modal: Promise<HTMLIonModalElement>;
  private stream: MediaStream;

  public get isAudioEnabled(): boolean {
    return this.stream?.getVideoTracks()[0].enabled;
  }

  public get isVideoEnabled(): boolean {
    return this.stream?.getVideoTracks()[0].enabled;
  }

  constructor(authService: AuthService, modalController: ModalController) {
    this.modal = modalController.create({
      component: IncomingCallPage,
    });

    authService.currentUser.subscribe(
      (user) => {
        if (user !== null) {
          try {
            this.peer = new Peer(user.userId, this.peerJsOptions);
            this.peer.on('call',
              async (call) => {
                // a call is already incoming or in progress
                if (false){ //this._incomingCall != null || this.isCallStarted){
                  this.declineCall();
                } else {
                  this.mediaCall = call;
                  this._incomingCall.next(call);
  
                  (await this.modal).present();
                }
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

  public async call(userId: string, isVideo: boolean) {
    try {
      this.stream = await navigator.mediaDevices.getUserMedia({ video: isVideo, audio: true });

      this.mediaCall = this.peer.call(userId, this.stream);
      if (!this.mediaCall) {
        throw new Error('Unable to connect to remote peer');
      }

      this._localStream.next(this.stream);
      this._isCallInProgress.next(true);

      this.mediaCall.on('stream',
        (remoteStream) => {
          this._remoteStream.next(remoteStream);
        }
      );

      this.mediaCall.on('error',
        err => {
          console.error(err);
          this._isCallInProgress.next(false);
        }
      );

      this.mediaCall.on('close', () => this.onCallClose());
    }
    catch (ex) {
      console.error(ex);
      this._isCallInProgress.next(false);
    }
  }

  public async answerCall() {
    try {
      this._incomingCall.next(null);
      this._isCallInProgress.next(true);

      this.mediaCall.on('stream', (remoteStream) => {
        this._remoteStream.next(remoteStream);
      });

      this.mediaCall.on('error', err => {
        this._isCallInProgress.next(false);
        console.error(err);
      });

      this.mediaCall.on('close', () => this.onCallClose());

      const stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      this._localStream.next(stream);

      this.mediaCall.answer(stream);

      (await this.modal).dismiss();
    }
    catch (ex) {
      console.error(ex);
      this._isCallInProgress.next(false);
    }
  }

  public async declineCall() {
    try {
      debugger;
      this._incomingCall.next(null);
      (await this.modal).dismiss();
    }
    catch (ex) {
      this._isCallInProgress.next(false);
    }
  }

  public closeMediaCall() {
    this.mediaCall?.close();

    if (!this.mediaCall) {
      this.onCallClose();
    }

    this._isCallInProgress.next(false);
  }

  private onCallClose() {
    this._remoteStream?.value.getTracks().forEach(track => {
      track.stop();
    });
    this._localStream?.value.getTracks().forEach(track => {
      track.stop();
    });
  }

  public toggleMicrophone() {
    this.stream.getAudioTracks()[0].enabled = !this.isAudioEnabled;
  }

  public toggleVideo() {
    this.stream.getVideoTracks()[0].enabled = !this.isVideoEnabled;
  }

  ngOnDestroy() {
    this.mediaCall?.close();
    this.peer?.disconnect();
    this.peer?.destroy();
   }
}
