import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor() { }

  successMessage(message: string): any {
    return Swal.fire({
      icon: 'success',
      title: message,
      confirmButtonText: '確認'
    });
  }

  errorMessage(message: string): any {
    return Swal.fire({
      icon: 'error',
      title: message,
      confirmButtonText: '確認'
    });
  }

  warningMessage(message: string): any {
    return Swal.fire({
      icon: 'warning',
      title: message,
      confirmButtonText: '確認'
    });
  }

  infoMessage(message: string): any {
    return Swal.fire({
      icon: 'info',
      title: message,
      confirmButtonText: '確認'
    });
  }

  questionMessage(message: string): any {
    return Swal.fire({
      icon: 'question',
      title: message,
      confirmButtonText: '確認'
    });
  }

  successNotify(message: string, notify: string): any {
    return Swal.fire({
      icon: 'success',
      title: message,
      text: notify,
      confirmButtonText: '確認'
    });
  }

  errorNotify(message: string, notify: string): any {
    return Swal.fire({
      icon: 'error',
      title: message,
      text: notify,
      confirmButtonText: '確認'
    });
  }

  warningNotify(message: string, notify: string): any {
    return Swal.fire({
      icon: 'warning',
      title: message,
      text: notify,
      confirmButtonText: '確認'
    });
  }

  infoNotify(message: string, notify: string): any {
    return Swal.fire({
      icon: 'info',
      title: message,
      text: notify,
      confirmButtonText: '確認'
    });
  }

  questionNotify(message: string, notify: string): any {
    return Swal.fire({
      icon: 'question',
      title: message,
      text: notify,
      confirmButtonText: '確認'
    });
  }

  successToast(message: string, time: number): void {
    Swal.fire({
      icon: 'success',
      title: message,
      timer: time,
      timerProgressBar: true,
      toast: true,
      position: 'bottom',
      showConfirmButton: false
    });
  }

  errorToast(message: string, time: number): void {
    Swal.fire({
      icon: 'error',
      title: message,
      timer: time,
      timerProgressBar: true,
      toast: true,
      position: 'bottom',
      showConfirmButton: false
    });
  }

  warningToast(message: string, time: number): void {
    Swal.fire({
      icon: 'warning',
      title: message,
      timer: time,
      timerProgressBar: true,
      toast: true,
      position: 'bottom',
      showConfirmButton: false
    });
  }

  infoToast(message: string, time: number): void {
    Swal.fire({
      icon: 'info',
      title: message,
      timer: time,
      timerProgressBar: true,
      toast: true,
      position: 'bottom',
      showConfirmButton: false
    });
  }

  questionToast(message: string, time: number): void {
    Swal.fire({
      icon: 'question',
      title: message,
      timer: time,
      timerProgressBar: true,
      toast: true,
      position: 'bottom',
      showConfirmButton: false
    });
  }

}
