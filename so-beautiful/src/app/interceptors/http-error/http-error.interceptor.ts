import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../../services/notification/notification.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private notificationService: NotificationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        switch (error.status) {
          case 401:
            this.notificationService.warningMessage('請先登入');
            this.router.navigate(['/account/login']);
            break;
          case 403:
            this.notificationService.warningMessage('沒有權限');
            this.router.navigate(['/']);
            break;
          case 500:
            this.notificationService.warningMessage('伺服器忙碌中');
            break;
          default:
            break;
        }
        return throwError(error);
      })
    );
  }
}
