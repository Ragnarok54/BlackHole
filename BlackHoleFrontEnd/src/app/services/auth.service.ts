import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';
import { RegisterUser } from '../models/user/registerUser';
import { User } from '../models/user/user';
import { Common } from '../shared/common';
import { ChatService } from './chat.service';
import { RtcService } from './rtc.service';
import { StatusService } from './status.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  static currentUser: any;
  
  constructor(private http: HttpClient, private chatService: ChatService, private statusService: StatusService, private rtcService: RtcService) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();

    this.currentUser.subscribe(
      () => {
        if (this.isAuthenticated()) {
          this.chatService.connect(this.token);
          this.statusService.connect(this.token);
        }
      }
    )
  }
  
  public get currentUserName(): string {
    if (this.currentUserSubject.value != null){
      var user = this.currentUserSubject.value;
      return user.firstName + " " + user.lastName;
    }
    else {
      return null;
    }
  }

  public get token(): string{
    return this.currentUserValue().token;
  }

  public currentUserValue(): User{
    return this.currentUserSubject.value;
  }

  public isAuthenticated(): boolean{
    return this.currentUserSubject.value != null;
  }

  login(phoneNumber: string, password: string){
    return this.http.post(Common.LOGIN_URL, { phoneNumber, password }).pipe(
      map(async (user: User) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        
        return user;
      })
    );
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.chatService?.disconnect();
    this.statusService?.disconnect();
    this.currentUserSubject.next(null);
    this.rtcService.destroyPeer();
  }

  register(model: RegisterUser){
    return this.http.post(Common.REGISTER_URL, model).pipe(
      map((result: boolean) =>{
        return result;
      })
    );
  }

  edit(user: User){
    return this.http.post(Common.EDIT_USER_URL, user);
  }
}
