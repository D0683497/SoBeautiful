import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../services/article/article.service';
import { NotificationService } from '../services/notification/notification.service';
import { ArticleSingle } from '../models/article/article-single.model';
import {BehaviorSubject, Observable} from 'rxjs';
import {MatPaginatorIntl, PageEvent} from '@angular/material/paginator';
import {HttpErrorResponse, HttpResponse} from '@angular/common/http';
import { Pagination } from '../models/pagination/pagination.model';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {map, shareReplay} from 'rxjs/operators';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.scss']
})
export class ArticleListComponent implements OnInit {

  articles!: ArticleSingle[];
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  pageSizeOptions: number[] = [5, 10, 20, 30, 40, 50];
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private articleService: ArticleService,
    private notificationService: NotificationService,
    private matPaginatorIntl: MatPaginatorIntl,
    private breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.initData();
    this.initPaginator();
  }

  initData(): void {
    this.articleService.getArticles(this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<ArticleSingle[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: HttpResponse<ArticleSingle[]>): void {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.articles = res.body as ArticleSingle[];
    this.loadingError$.next(false);
  }

  getFail(err: HttpErrorResponse): void {
    this.loadingError$.next(true);
  }

  onPageChange(event: PageEvent): void {
    this.pagination.pageIndex = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.initData();
  }

  initPaginator(): void {
    this.matPaginatorIntl.getRangeLabel = (page: number, pageSize: number, length: number): string => {
      return `第 ${page + 1} / ${Math.ceil(length / pageSize)} 頁`;
    };
    this.matPaginatorIntl.itemsPerPageLabel = '每頁筆數：';
    this.matPaginatorIntl.nextPageLabel = '下一頁';
    this.matPaginatorIntl.previousPageLabel = '上一頁';
    this.matPaginatorIntl.firstPageLabel = '第一頁';
    this.matPaginatorIntl.lastPageLabel = '最後一頁';
  }

  reload(): void {
    this.initData();
  }

}
