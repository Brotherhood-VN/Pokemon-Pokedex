import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
    // Progress Bar
    showSpinner: boolean = false;
    color: string = '#7367f0';
    barHeight: string = '5px';
    initialValue: number = 0;
    overlay: boolean = true;
    overlayValue: number = 0.5;
    showBar: boolean = true;
    indeterminate: boolean = false;
    spinnerDiameter: string = '15px';
    spinnerSpeed: 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 = 2;

    constructor(private primengConfig: PrimeNGConfig) {}

    ngOnInit() {
        this.primengConfig.ripple = true;
    }
}
