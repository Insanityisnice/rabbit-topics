import { Component } from '@angular/core';
import { MessagesService } from './messages.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  messages: Array<string> = [ "Test 1", "Test 2" ];

  constructor(private messagesService: MessagesService) {}

  ngOnInit() {
    this.getMessages();
  }

  private getMessages(): void {
    this.messagesService.getMessages()
        .then(data => {
          console.log(data);
          this.messages = data
        });
  }
}
