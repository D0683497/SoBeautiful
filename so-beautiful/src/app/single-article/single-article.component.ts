import { Component, OnInit } from '@angular/core';
import { ArticleSingle } from '../models/article/article-single.model';
import {BehaviorSubject, Observable} from 'rxjs';
import { ArticleService } from '../services/article/article.service';
import { NotificationService } from '../services/notification/notification.service';
import { ActivatedRoute } from '@angular/router';
import {HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {MatDialog} from '@angular/material/dialog';
import {CreateCommentComponent} from '../create-comment/create-comment.component';
import {CommentService} from '../services/comment/comment.service';
import {CommentSingle} from '../models/comment/comment-single.model';
import {Pagination} from '../models/pagination/pagination.model';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {map, shareReplay} from 'rxjs/operators';
import {PageEvent} from '@angular/material/paginator';

@Component({
  selector: 'app-single-article',
  templateUrl: './single-article.component.html',
  styleUrls: ['./single-article.component.scss']
})
export class SingleArticleComponent implements OnInit {

  articleId: string = this.route.snapshot.paramMap.get('articleId') as string;
  article!: ArticleSingle;
  comments!: CommentSingle[];
  loadingError$ = new BehaviorSubject<boolean>(true);
  pagination = new Pagination();
  pageSizeOptions: number[] = [5, 10, 20, 30, 40, 50];
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private route: ActivatedRoute,
    private articleService: ArticleService,
    private notificationService: NotificationService,
    public dialog: MatDialog,
    private commentService: CommentService,
    private breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.initData();
    this.initComments();
  }

  initData(): void {
    this.articleService.getArticle(this.articleId).subscribe(
      (res: ArticleSingle) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  initComments(): void {
    this.commentService.getComments(this.articleId, this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<CommentSingle[]>) => { this.getCommentsSuccess(res); },
      (err: HttpErrorResponse) => { this.getCommentsFail(err); }
    );
  }

  getCommentsSuccess(res: HttpResponse<CommentSingle[]>): void {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.comments = res.body as CommentSingle[];
    this.loadingError$.next(false);
  }

  getCommentsFail(err: HttpErrorResponse): void {
    this.loadingError$.next(true);
  }

  onPageChange(event: PageEvent): void {
    this.pagination.pageIndex = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.initData();
  }

  getSuccess(res: ArticleSingle): void {
    this.article = res;
    this.loadingError$.next(false);
  }

  getFail(err: HttpErrorResponse): void {
    if (err.status === 404) {
      this.notificationService.errorMessage('查無此文章');
    }
    this.loadingError$.next(true);
  }

  reload(): void {
    this.ngOnInit();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CreateCommentComponent, {
      data: {articleId: this.articleId}
    });
  }

}
