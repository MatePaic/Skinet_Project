import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersDetailedComponent } from './orders-detailed/orders-detailed.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    OrdersDetailedComponent
  ],
  imports: [
    CommonModule,
    OrdersRoutingModule,
    SharedModule
  ]
})
export class OrdersModule { }
