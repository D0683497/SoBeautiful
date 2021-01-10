import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ArticleCreate } from '../../models/article/article-create.model';
import { ArticleSingle } from '../../models/article/article-single.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

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
  create(data: ArticleCreate): Observable<object> {
    const url = `${this.urlRoot}/articles`;
    return this.http.post(url, data, this.httpOptions);
  }

  // 文章列表
  getArticles(pageIndex: number, pageSize: number): Observable<HttpResponse<ArticleSingle[]>> {
    const url = `${this.urlRoot}/articles?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<ArticleSingle[]>(url, this.httpResponseOptions);
  }

  // 文章
  getArticle(articleId: string): Observable<ArticleSingle> {
    const url = `${this.urlRoot}/articles/${articleId}`;
    return this.http.get<ArticleSingle>(url, this.httpOptions);
  }

}
