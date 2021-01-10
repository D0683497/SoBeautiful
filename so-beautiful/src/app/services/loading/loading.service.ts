import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {

  loadingStatus$ = new BehaviorSubject<boolean>(false);

  constructor(private spinner: NgxSpinnerService) { }

  startLoading(): void {
    this.loadingStatus$.next(true);
    this.spinner.show();
  }

  stopLoading(): void {
    this.loadingStatus$.next(false);
    this.spinner.hide();
  }

}
