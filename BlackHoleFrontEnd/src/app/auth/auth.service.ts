import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { last, map } from 'rxjs/operators';
import { API_URL, LOGIN_URL, REGISTER_URL } from 'src/environments/environment';
import { RegisterUser } from '../models/user/registerUser';
import { User } from '../models/user/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  static currentUser: any;
  
  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(sessionStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
   }

   public get currentUserName(): String {
    if (this.currentUserSubject.value != null){
      var user = this.currentUserSubject.value;
      return user.name;
    }
    else {
      return null;
    }
  }

  public currentUserValue(): User{
    return this.currentUserSubject.value;
  }

  public isAuthenticated(): boolean{
    return this.currentUserSubject.value != null;
  }

  login(email: string, password: String){
    return this.http.post(API_URL + LOGIN_URL, { email, password }).pipe(
      map((user: User) => {
        sessionStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  logout() {
    sessionStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  register(model: RegisterUser){
    return this.http.post(API_URL + REGISTER_URL, model).pipe(
      map((result: boolean) =>{
        return result;
      })
    );
  }
}
