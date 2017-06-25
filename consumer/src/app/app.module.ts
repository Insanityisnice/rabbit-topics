import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { MessagesService } from './messages.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpModule
  ],
  providers: [
    MessagesService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
