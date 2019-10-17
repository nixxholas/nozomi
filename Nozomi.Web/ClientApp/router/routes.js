import CounterExample from '../components/counter-example';
import HomePage from '../components/home-page';
import About from '../components/about';

import ViewCurrency from 'components/currency/view';

import DashboardHome from 'components/dashboard/index';

import OidcCallback from '../components/auth/oidc-callback';
import OidcCallbackError from '../components/auth/oidc-callback-error';

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home',
    meta: { isPublic: true }
  },
  { name: 'about', path: '/about', component: About, display: 'About', icon: 'info',
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
    component: OidcCallback,
    meta: { isPublic: true }
  },
  // Authentication-specific routing
  {
    path: '/oidc-callback-error', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
    name: 'oidcCallbackError',
    display: 'Nozomi Auth Error',
    component: OidcCallbackError,
    meta: { isPublic: true }
  },
  // Dashboard home routing
  { name: 'dashboard', path: '/dashboard', props: true, component: DashboardHome,
    meta: { isPublic: false }
  },
];
