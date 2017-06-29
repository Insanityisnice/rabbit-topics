import { Injectable } from "@angular/core"
import { Http } from "@angular/http"
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class SettingsService {
    settings: any = null;
    consumersApiPath: string = 'api/consumers';
    producerApiPath: string = 'api/producers';

    constructor(private http:Http) {
        console.log("SettingsService constructor")
    }

    load(): Promise<any> {
        console.log("SettingService - loading settings file.");
        var observable = this.http.get("./assets/settings.json").map(res => { console.log(res); return res.json(); });
        
        observable.subscribe(config => {
            console.log('Settings loaded...........');
            this.settings = config;
        });

        return observable.toPromise();
    }

    getConsumersUrl(): string {
        if(this.settings.consumersUrl.endsWith('/')) {
            return this.settings.consumersUrl + this.consumersApiPath;
        } else {
            return this.settings.consumersUrl + '/' + this.consumersApiPath;
        }
    }

    getProduerUrl(): string {
        if(this.settings.consumersUrl.endsWith('/')) {
            return this.settings.producerUrl + this.producerApiPath;
        } else {
            return this.settings.producerUrl + '/' + this.producerApiPath;
        }
    }
}