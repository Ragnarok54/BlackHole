import { Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ModalController, ToastController } from '@ionic/angular';
import Peer, { MediaConnection, PeerJSOption } from 'peerjs';
import { Subject } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { IncomingCallPage } from '../call/incoming-call/incoming-call.page';
import { AuthService } from './auth.service';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})

export class RtcService implements OnDestroy {
  private peerJsOptions: PeerJSOption = {
    debug: 3,
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

  public mediaCall: MediaConnection;

  private _localStream: BehaviorSubject<MediaStream> = new BehaviorSubject(null);
  public localStream = this._localStream.asObservable();
  private _remoteStream: BehaviorSubject<MediaStream> = new BehaviorSubject(null);
  public remoteStream = this._remoteStream.asObservable();

  private _isCallInProgress = new Subject<boolean>();
  public isCallInProgress = this._isCallInProgress.asObservable();

  private _calling = new Subject<boolean>();
  public calling = this._calling.asObservable();

  private stream: MediaStream;

  public get isAudioEnabled(): boolean {
    return this.stream?.getAudioTracks()[0].enabled;
  }

  public get isVideoEnabled(): boolean {
    return this.stream?.getVideoTracks()[0].enabled;
  }

  constructor(private router: Router, authService: AuthService, modalController: ModalController) {
    authService.currentUser.subscribe(
      (user) => {
        if (user !== null) {
          try {
            this.peer = new Peer(user.userId, this.peerJsOptions);
            this.peer.on('call',
              async (call) => {
                this.mediaCall = call;
                this._calling.next(true);

                var modal = modalController.create({
                  component: IncomingCallPage,
                  swipeToClose: false,
                  backdropDismiss: false,
                  cssClass: 'incoming-call-modal',
                });
                
                (await modal).present();
              }
            );

            this.peer.on('connection', conn => {
              console.log("new conn");
              
              conn.on('close', () => {
                console.log("conn close event");
                //handlePeerDisconnect();
              });
            });
          

            this.peer.on('error',
              (error) => {
                debugger;
                console.error(error);
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

  public async callAsync(userId: string, isVideo: boolean) {
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
          this._calling.next(false);
        }
      );

      this.mediaCall.on('close',
        () => {
          this._calling.next(false);
          this.cleanUpCallResources();
          this.router.navigateByUrl('');
        }
      );

      this.mediaCall.on('error',
        err => {
          console.error(err);
          this.cleanUpCallResources();
          this._calling.next(false);
        }
      );
    }
    catch (ex) {
      console.error(ex);
      this._isCallInProgress.next(false);
    }
  }

  public async answerCallAsync() {
    try {
      this._calling.next(false);
      this._isCallInProgress.next(true);

      this.mediaCall.on('stream',
        (remoteStream) => {
          debugger;
          this._remoteStream.next(remoteStream);
        }
      );

      this.mediaCall.on('close',
        () => {
          debugger;

          this.cleanUpCallResources();
          this.router.navigateByUrl('');
        }
      );

      this.mediaCall.on('error', 
        err => {
          debugger;

          this._isCallInProgress.next(false);
          this.cleanUpCallResources();
          console.error(err);
        }
      );

      this.stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      this._localStream.next(this.stream);

      this.mediaCall.answer(this.stream);
    }
    catch (ex) {
      console.error(ex);
      this._isCallInProgress.next(false);
    }
  }

  public async declineCallAsync() {
    try {
      await this.answerCallAsync();
      this.mediaCall.close();
      this._calling.next(false);
    }
    catch (ex) {
      this._isCallInProgress.next(false);
      console.error(ex);
    }
  }

  public endCall() {
    this.mediaCall.close();
  }

  private cleanUpCallResources() {
    this.discardVideoFeeds();
    
    this._isCallInProgress.next(false);
  }

  private discardVideoFeeds() {
    this.stream.getTracks().forEach(track => {
      track.stop();
    });

    this._remoteStream?.value?.getTracks().forEach(track => {
      track.stop();
    });
    this._localStream?.value?.getTracks().forEach(track => {
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
