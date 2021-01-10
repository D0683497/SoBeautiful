import { Component, OnInit, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { MatDrawer } from '@angular/material/sidenav';
import { AuthService } from './services/auth/auth.service';
import { Router } from '@angular/router';
import { NotificationService } from './services/notification/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  @ViewChild('drawer') drawer!: MatDrawer;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  isLoggedIn$!: Observable<boolean>;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.isLoggedIn$ = this.authService.getLoginStatus();
  }

  clickItem(): void {
    this.isHandset$.subscribe(data => {
      if (data) {
        this.drawer.close();
      }
    });
  }

  userName(): string | null {
    return this.authService.getUniqueName();
  }

  logout(): void {
    this.authService.logout();
    this.notificationService.successToast('成功登出', 2000);
    this.router.navigate(['/']);
  }

}
