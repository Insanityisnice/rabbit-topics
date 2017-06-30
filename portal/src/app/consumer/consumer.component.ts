import { Component, Input, OnInit } from '@angular/core';

import { ConsumersService } from './consumers.service';

import { AppComponent } from '../app.component';

import { Consumer } from '../../models/consumer';

@Component({
    selector: 'consumer',
    templateUrl: './consumer.component.html'
})
export class ConsumerComponent  implements OnInit {
    @Input() consumer: Consumer;
    messages: Array<string> = [];

    constructor(private consumersService: ConsumersService) {
    }

    ngOnInit() {
        this.consumersService.getConsumerMessages(this.consumer.name)
              .then(result => this.messages = result);
    }
}