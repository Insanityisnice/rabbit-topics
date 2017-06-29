import { OnInit, Injectable } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import { Observable, BehaviorSubject } from 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';

import { SettingsService } from '../../settings/settings.service'

@Injectable()
export class ConsumersService {

    url: string;
    messagesPath: string = 'messages'

    constructor(private settingsService: SettingsService, private http: Http) {
        this.url = settingsService.getConsumersUrl();
        console.log('ConsumerUrl:' + this.url);
    }

    addConsumer(consumer) {
        var requestOptions = new RequestOptions({
            headers: new Headers({
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*'
            })
        });

        console.log('Adding new consumer ' + JSON.stringify(consumer) + " to url[" + this.url + "]");
        return this.http
                   .post(this.url, consumer, requestOptions)
                   .map(res => res.json())
                   .subscribe(res => console.log(res), this.handleError, () => console.log("Customer added."));
    }
    
    getConsumers() {
        var requestOptions = new RequestOptions();

        console.log('Getting messages.');
        return this.http
                   .get(this.url, requestOptions)
                   .toPromise()
                   .then(res => res.json())
                   .catch(this.handleError);
    }

    getConsumerMessages(consumer: string) {
        var requestOptions = new RequestOptions();

        console.log('Getting messages.');
        return this.http
                   .get(this.url + '/' + consumer + '/' + this.messagesPath, requestOptions)
                   .toPromise()
                   .then(res => res.json())
                   .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occured: ', error);
        return Promise.reject(error.message || error);
    }
}