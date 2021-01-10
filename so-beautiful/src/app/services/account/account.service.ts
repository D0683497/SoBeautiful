import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Register } from '../../models/account/register.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  // 註冊
  register(data: Register): Observable<object> {
    const url = `${this.urlRoot}/account/register`;
    return this.http.post(url, data, this.httpOptions);
  }

}
