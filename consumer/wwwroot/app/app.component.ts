import { Component, OnInit } from '@angular/core';
import { Subscription }   from 'rxjs/Subscription';

@Component({
  selector: 'my-app',
  template: `
      <div>
        <h1>Hello World!!</h1>
      </div>`})
export class AppComponent extends OnInit {
    subscription: Subscription;
    
    Refresh(){
    }
    
    constructor() {
        super();
    }

    ngOnInit() {
        this.Refresh();
    }
}
