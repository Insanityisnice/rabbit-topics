import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { SettingsService } from '../settings/settings.service';
import { ConsumersService } from './consumer/consumers.service';

import { AppComponent } from './app.component';
import { ConsumersComponent } from './consumer/consumers.component';
import { AddConsumerComponent } from './consumer/add.consumer.component';
import { ConsumerComponent } from './consumer/consumer.component';

export function initialize(settingsService: SettingsService) {
  console.log("Loading settings...");
  return () => settingsService.load();
}

@NgModule({
  declarations: [
    AppComponent,
    ConsumersComponent,
    AddConsumerComponent,
    ConsumerComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initialize,
      deps: [SettingsService, Http],
      multi: true
    },
    SettingsService,
    ConsumersService
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }