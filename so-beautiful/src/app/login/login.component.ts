import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Login } from '../models/account/login.model';
import { AuthService } from '../services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification/notification.service';
import { FormError } from '../models/form-error';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  pattern = new RegExp(/[\w\-\.\@\+\#\$\%\\\/\(\)\[\]\*\&\:\>\<\^\!\{\}\=]+/gm);
  loginForm!: FormGroup;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.loginForm = this.fb.group({
      UserName: [null, [Validators.required, Validators.maxLength(256), Validators.pattern(this.pattern)]],
      Password: [null, [Validators.required, Validators.minLength(8), Validators.maxLength(64)]]
    });
  }

  onSubmit(value: Login): void {
    this.authService.login(value).subscribe(
      () => { this.loginSuccess(); },
      (err: HttpErrorResponse) => { this.loginFail(err); }
    );
  }

  loginSuccess(): void {
    this.notificationService.successToast('登入成功', 2000);
    this.router.navigate(['/']);
  }

  loginFail(err: HttpErrorResponse): void {
    if (err.status === 400) {
      const errors: FormError[] = err.error;
      if (!errors[0].propertyName) {
        switch (errors[0].errorMessage) {
          case '帳號被鎖定':
            this.notificationService.warningNotify('帳號被鎖定', '請聯絡管理員');
            break;
          case '帳戶尚未驗證':
            this.notificationService.warningNotify('帳戶尚未驗證', '請聯絡管理員');
            break;
          default:
            this.notificationService.errorMessage('登入失敗');
            break;
        }
      } else {
        try {
          errors.every(element => {
            const controlName = element.propertyName;
            // 雖然可能有多個錯誤，但後面的會蓋掉前面的
            if (this.loginForm.get(controlName) !== null) {
              this.loginForm.controls[controlName].setErrors({server: element.errorMessage});
            }
            return true;
          });
          this.notificationService.errorMessage('登入失敗');
        } catch (error) {
          this.notificationService.errorMessage('發生未知錯誤');
        }
      }
    }
  }

}
