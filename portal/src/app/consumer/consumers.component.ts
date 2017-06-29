import { Component, Input, OnInit, OnChanges } from '@angular/core';

import { ConsumersService } from './consumers.service';

import { AppComponent } from '../app.component';

@Component({
    selector: 'consumers',
    templateUrl: './consumers.component.html'
})
export class ConsumersComponent implements OnInit {
  consumers: Array<any> = [];
  
  constructor(private consumersService: ConsumersService) {
  }

  addConsumer(event) {
    
  }
  
  ngOnChanges() {

  }

  ngOnInit() {
    
  }
}