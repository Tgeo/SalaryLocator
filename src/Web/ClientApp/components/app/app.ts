import * as ng from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { NavMenu } from '../nav-menu/nav-menu';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/toPromise';

@ng.Component({
    selector: 'app',
    template: require('./app.html'),
    directives: [...ROUTER_DIRECTIVES, NavMenu]
})
export class App {
}
