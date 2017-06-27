import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { ConsumersService } from './consumer/consumers.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpModule
  ],
  providers: [
    ConsumersService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }