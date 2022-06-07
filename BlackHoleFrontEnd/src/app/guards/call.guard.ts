import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { RtcService } from '../services/rtc.service';

@Injectable({
  providedIn: 'root'
})
export class CallGuard implements CanActivate {

  constructor(private router: Router, private rtcService: RtcService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let canActivate = false;

    this.rtcService.isCallInProgress.subscribe(value => canActivate = value);

    if (!canActivate) {
      this.router.navigateByUrl('');
    }

    return canActivate;
  }

}
