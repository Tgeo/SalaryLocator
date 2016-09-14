import { RouterConfig } from '@angular/router';
import { Home } from './components/home/home';
import { About } from './components/about/about';
import { Sources } from './components/sources/sources';

export const routes: RouterConfig = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: Home },
    { path: 'sources', component: Sources },
    { path: 'about', component: About },
    { path: '**', redirectTo: 'home' }
];
