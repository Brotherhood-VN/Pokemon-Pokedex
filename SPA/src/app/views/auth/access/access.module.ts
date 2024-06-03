import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccessRoutingModule } from './access-routing.module';
import { ButtonModule } from 'primeng/button';
import { AccessComponent } from './access.component';

@NgModule({
    declarations: [AccessComponent],
    imports: [CommonModule, AccessRoutingModule, ButtonModule],
})
export class AccessModule {}
