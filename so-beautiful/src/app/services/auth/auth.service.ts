import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Login } from '../../models/account/login.model';
import { map } from 'rxjs/operators';
import { Token } from '../../models/account/token.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  loginStatus$ = new BehaviorSubject<boolean>(false);

  constructor(
    private jwtHelper: JwtHelperService,
    private http: HttpClient) { }

  // 登入
  login(data: Login): Observable<void> {
    const url = `${this.urlRoot}/account/login`;
    return this.http.post<Token>(url, data, this.httpOptions).pipe(
      map((result) => {
        this.setToken(result.AccessToken);
        this.setLoginStatus(true);
      })
    );
  }

  // 登出
  logout(): void {
    this.removeLoginStatus();
  }

  // 獲取登入狀態
  getLoginStatus(): Observable<boolean> {
    this.checkLoginStatus();
    return this.loginStatus$;
  }

  // 獲取角色
  getRole(): string | null {
    const token = localStorage.getItem('access_token');
    if (token === null) {
      return null;
    }
    const decodeToken = this.jwtHelper.decodeToken(token);
    return decodeToken.role;
  }

  // 獲取用戶名稱
  getUniqueName(): string | null {
    const token = localStorage.getItem('access_token');
    if (token === null) {
      return null;
    }
    const decodeToken = this.jwtHelper.decodeToken(token);
    return decodeToken.unique_name;
  }

  // 獲取用戶 Id
  getUserId(): string | null {
    const token = localStorage.getItem('access_token');
    if (token === null) {
      return null;
    }
    const decodeToken = this.jwtHelper.decodeToken(token);
    return decodeToken.nameid;
  }

  // 設置 token
  setToken(token: string): void {
    localStorage.setItem('access_token', token);
  }

  // 設置登入狀態
  setLoginStatus(status: boolean): void {
    this.loginStatus$.next(status);
  }

  // 移除登入狀態 (登出)
  removeLoginStatus(): void {
    localStorage.clear();
    this.loginStatus$.next(false);
  }

  // Token 是否過期
  private isTokenExpired(): boolean {
    const token = localStorage.getItem('access_token');
    if (token === null) { // 沒有 token
      return true;
    }
    return this.jwtHelper.isTokenExpired(token);
  }

  // 檢查登入狀態
  private checkLoginStatus(): void {
    if (!this.isTokenExpired()) { // token 沒過期
      this.loginStatus$.next(true);
    } else { // token 過期或沒有 token
      this.removeLoginStatus();
    }
  }

}
