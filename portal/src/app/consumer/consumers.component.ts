import { Component, Input, OnInit, OnChanges } from '@angular/core';

import { ConsumersService } from './consumers.service';

import { AppComponent } from '../app.component';

import { Consumer } from '../../models/consumer';

@Component({
    selector: 'consumers',
    templateUrl: './consumers.component.html'
})
export class ConsumersComponent implements OnInit {
  consumers: Array<Consumer> = [];
  
  constructor(private consumersService: ConsumersService) {
  }
  
  ngOnChanges() {
      this.consumersService.getConsumers()
        .then(result => this.consumers = result);
  }

  ngOnInit() {
    this.consumersService.getConsumers()
        .then(result => this.consumers = result);
  }
}