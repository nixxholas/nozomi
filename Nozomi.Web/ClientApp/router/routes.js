import CounterExample from 'components/counter-example';
import FetchData from 'components/fetch-data';
import HomePage from 'components/home-page';
import About from 'components/about';

import ViewCurrency from 'components/currency/view';

import OidcCallback from 'components/auth/oidc-callback';

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home',
    meta: { isPublic: true }
  },
  { name: 'about', path: '/about', component: About, display: 'About', icon: 'info',
    meta: { isPublic: false }
  },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap',
    meta: { isPublic: true }
  },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Data', icon: 'list',
    meta: { isPublic: true }
  },
  // Currency-specific routing
  { name: 'view-currency', path: '/currency/:slug', props: true, component: ViewCurrency,
    meta: { isPublic: true }
  },
  // Authentication-specific routing
  {
    path: '/oidc-callback', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
    name: 'oidcCallback',
    display: 'OIDC Callback',
    component: OidcCallback
  }
];
