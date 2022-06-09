import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { RtcService } from '../services/rtc.service';

@Injectable({
  providedIn: 'root'
})
export class CallGuard implements CanActivate {
  private isCallInProgress: boolean;

  constructor(private router: Router, private rtcService: RtcService) {
    this.rtcService.isCallInProgress.subscribe(value => this.isCallInProgress = value);
   }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // if (!this.isCallInProgress) {
    //   this.router.navigateByUrl('');
    // }

    return true;
  }

}
