import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { NotificationService } from '../services/notification/notification.service';
import { CommentService } from '../services/comment/comment.service';
import {HttpErrorResponse} from '@angular/common/http';
import {CommentCreate} from '../models/comment/comment-create.model';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {FormError} from '../models/form-error';

@Component({
  selector: 'app-create-comment',
  templateUrl: './create-comment.component.html',
  styleUrls: ['./create-comment.component.scss']
})
export class CreateCommentComponent implements OnInit {

  createForm!: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<CreateCommentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private commentService: CommentService) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.createForm = this.fb.group({
      Content: [null, [Validators.required, Validators.maxLength(500)]]
    });
  }

  onSubmit(value: CommentCreate): void {
    this.commentService.create(this.data.articleId, value).subscribe(
      () => { this.createSuccess(); },
      (err: HttpErrorResponse) => { this.createFail(err); }
    );
  }

  createSuccess(): void {
    this.notificationService.successToast('建立成功', 2000);
    this.dialogRef.close();
  }

  createFail(err: HttpErrorResponse): void {
    if (err.status === 400) {
      const errors: FormError[] = err.error;
      try {
        errors.every(element => {
          if (!element.propertyName) {
            return false;
          } else {
            const controlName = element.propertyName;
            // 雖然可能有多個錯誤，但後面的會蓋掉前面的
            if (this.createForm.get(controlName) !== null) {
              this.createForm.controls[controlName].setErrors({server: element.errorMessage});
            }
            return true;
          }
        });
        this.notificationService.errorToast('建立失敗', 2000);
      } catch (error) {
        this.notificationService.errorMessage('發生未知錯誤');
        this.dialogRef.close();
      }
    }
  }

}
