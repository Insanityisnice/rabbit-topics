import { OnInit, Injectable } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import { Observable, BehaviorSubject } from 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ConsumersService {

    url: string = `http://localhost:5000/api/consumers`;

    constructor(private http: Http) {
    }
    
    getMessages() {
        var requestOptions = new RequestOptions();

        console.log('Getting messages.');
        return this.http
                   .get(this.url, requestOptions)
                   .toPromise()
                   .then(res => res.json())
                   .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occured: ', error);
        return Promise.reject(error.message || error);
    }
}