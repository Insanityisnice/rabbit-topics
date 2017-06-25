import { OnInit, Injectable } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import { Observable, BehaviorSubject } from 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class MessagesService {

    url: string = `http://localhost/api/messages`;

    constructor(private http: Http) {
    }
    
    getMessages() {}

    private handleError(error: any): Promise<any> {
        console.error('An error occured: ', error);
        return Promise.reject(error.message || error);
    }
}