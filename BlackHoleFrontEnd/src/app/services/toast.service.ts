import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';
import { ToastButton } from '@ionic/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastCtrl: ToastController) { }

  public createToast(message: string, color: string, position: 'top' | 'bottom' | 'middle', buttons?: (ToastButton | string)[], duration: number = 5000) {
    this.toastCtrl.create({
      message: message,
      duration: duration,
      position: position,
      color: color,
      buttons: buttons != null ? buttons : ['Dismiss']
    }).then((el) => el.present());
  }
}
