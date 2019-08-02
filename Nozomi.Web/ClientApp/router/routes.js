import CounterExample from 'components/counter-example';
import HomePage from 'components/home-page';
import About from 'components/about';

import ViewCurrency from 'components/currency/view';

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'about', path: '/about', component: About, display: 'About', icon: 'info' },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  // Currency-specific routing
  { name: 'view-currency', path: '/currency/:slug', props: true, component: ViewCurrency }
];
