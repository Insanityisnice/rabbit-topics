import { Component, Input } from '@angular/core';

import { ConsumersService } from './consumers.service';

import { AppComponent } from '../app.component';

@Component({
    selector: 'add-consumer',
    templateUrl: './add.consumer.component.html'
})
export class AddConsumerComponent {
    consumerName: string = '';
    exchange: string = '';
    bindingKey: string = '';

  constructor(private consumersService: ConsumersService) {
  }

  validateConsumer() {
      return this.consumerName === '' || this.exchange === '' || this.bindingKey === '';
  }

  addConsumer(event) {
      console.log("Adding Consumer:" + this.consumerName + " Exchange:" + this.exchange + " BindingKey:" + this.bindingKey);
      this.consumersService.addConsumer({
          name: this.consumerName,
          exchange: this.exchange,
          bindingKey: this.bindingKey
      });
  }
}