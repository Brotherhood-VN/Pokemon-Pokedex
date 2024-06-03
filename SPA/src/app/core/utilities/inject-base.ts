import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { NgxProgressService } from "@kken94/ngx-progress";
import { ToastUtility } from "./toast-utility";

export abstract class InjectBase {
    protected _router = inject(Router);
    protected _toast = inject(ToastUtility);
    protected _progress = inject(NgxProgressService);
}