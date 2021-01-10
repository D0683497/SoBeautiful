import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CommentCreate} from '../../models/comment/comment-create.model';
import {CommentSingle} from '../../models/comment/comment-single.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  httpResponseOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    observe: 'response' as const
  };

  constructor(private http: HttpClient) { }

  // 建立
  create(articleId: string, data: CommentCreate): Observable<object> {
    const url = `${this.urlRoot}/articles/${articleId}/comments`;
    return this.http.post(url, data, this.httpOptions);
  }

  // 文章列表
  getComments(articleId: string, pageIndex: number, pageSize: number): Observable<HttpResponse<CommentSingle[]>> {
    const url = `${this.urlRoot}/articles/${articleId}/comments?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<CommentSingle[]>(url, this.httpResponseOptions);
  }

}
