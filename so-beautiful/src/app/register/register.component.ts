import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Register } from '../models/account/register.model';
import { Observable } from 'rxjs';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import { NotificationService } from '../services/notification/notification.service';
import { Router } from '@angular/router';
import { AccountService } from '../services/account/account.service';
import { HttpErrorResponse } from '@angular/common/http';
import { FormError } from '../models/form-error';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnInit {

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  registerForm!: FormGroup;
  userNamePattern = new RegExp(/[\w\-\.\@\+\#\$\%\\\/\(\)\[\]\*\&\:\>\<\^\!\{\}\=]+/gm);
  hideConfirmPassword = true;
  maxDate = new Date();

  constructor(
    private breakpointObserver: BreakpointObserver,
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.registerForm = this.fb.group({
      UserName: [null, [Validators.required, Validators.maxLength(256), Validators.pattern(this.userNamePattern)]],
      Password: [null, [Validators.required, Validators.minLength(8), Validators.maxLength(64)]],
      PasswordConfirm: [null, [Validators.required, Validators.minLength(8), Validators.maxLength(64)]],
      Email: [null, [Validators.required, Validators.maxLength(256), Validators.email]],
      Surname: [null, [Validators.required, Validators.maxLength(20)]],
      GivenName: [null, [Validators.required, Validators.maxLength(20)]],
      Gender: [null, [Validators.required]], // TODO: 驗證格式
      DateOfBirth: [null, [Validators.required]] // TODO: 驗證格式
    });
  }

  onSubmit(value: Register): void {
    this.accountService.register(value).subscribe(
      () => { this.registerSuccess(); },
      (err: HttpErrorResponse) => { this.registerFail(err); }
    );
  }

  registerSuccess(): void {
    this.notificationService.successMessage('註冊成功');
    this.router.navigate(['/login']);
  }

  registerFail(err: HttpErrorResponse): void {
    if (err.status === 400) {
      const errors: FormError[] = err.error;
      try {
        errors.every(element => {
          if (!element.propertyName) {
            return false;
          } else {
            const controlName = element.propertyName;
            // 雖然可能有多個錯誤，但後面的會蓋掉前面的
            if (this.registerForm.get(controlName) !== null) {
              this.registerForm.controls[controlName].setErrors({server: element.errorMessage});
            }
            return true;
          }
        });
        this.notificationService.errorMessage('註冊失敗');
      } catch (error) {
        this.notificationService.errorMessage('發生未知錯誤');
      }
    }
  }

}
