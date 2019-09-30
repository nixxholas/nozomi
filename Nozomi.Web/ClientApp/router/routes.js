import CounterExample from 'components/counter-example';
import FetchData from 'components/fetch-data';
import HomePage from 'components/home-page';
import About from 'components/about';

import ViewCurrency from 'components/currency/view';

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'about', path: '/about', component: About, display: 'About', icon: 'info', meta: {
    isPublic: false
  }},
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Data', icon: 'list' },
  // Currency-specific routing
  { name: 'view-currency', path: '/currency/:slug', props: true, component: ViewCurrency }
];
