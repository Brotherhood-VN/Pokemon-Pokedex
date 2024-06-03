import { Injectable } from "@angular/core";
import { Confirmation, ConfirmationService, Message, MessageService } from "primeng/api";

@Injectable({
  providedIn: "root",
})
export class ToastUtility {
  life: number = 3000;

  constructor(private _messageService: MessageService, private _confirmDialog: ConfirmationService) { }

  success(summary: string, detail: string) {
    let message: Message = <Message>{
      severity: 'success',
      life: this.life,
      summary: summary,
      detail: detail
    };

    this._messageService.add(message)
  }

  warning(summary: string, detail: string) {
    let message: Message = <Message>{
      severity: 'warn',
      life: this.life,
      summary: summary,
      detail: detail
    };

    this._messageService.add(message)
  }

  error(summary: string, detail: string) {
    let message: Message = <Message>{
      severity: 'error',
      life: this.life,
      summary: summary,
      detail: detail
    };

    this._messageService.add(message)
  }

  info(summary: string, detail: string) {
    let message: Message = <Message>{
      severity: 'info',
      life: this.life,
      summary: summary,
      detail: detail
    };

    this._messageService.add(message)
  }

  confirm(header: string, message: string, accept?: () => any, reject?: () => any) {
    let confirmation: Confirmation = <Confirmation>{
      header: header,
      message: message,
      accept: accept,
      reject: reject,
    }
    this._confirmDialog.confirm(confirmation);
  }

  clear() {
    this._messageService.clear();
  }
}