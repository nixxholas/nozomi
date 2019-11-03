import HomePage from '../components/home-page';
import About from '../components/about';

import ViewCurrency from 'components/currency/view';

import SourceIndex from 'components/source/index';

import DashboardHome from 'components/dashboard/index';
import ModifyProfile from 'components/dashboard/account/modify-profile';

import OidcCallback from '../components/auth/oidc-callback';
import OidcCallbackError from '../components/auth/oidc-callback-error';

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: ['fa', 'home'],
    meta: { isPublic: true, onNav: false }
  },
  { name: 'about', path: '/about', component: About, display: 'About', icon: ['fa', 'info'],
    meta: { isPublic: true, onNav: true }
  },
  { name: 'source-index', path: '/source', props: true, component: SourceIndex, display: 'Sources', icon: ['fa', 'university'],
    meta: { isPublic: true, onNav: true }
  },
  // Currency-specific routing
  { name: 'view-currency', path: '/currency/:slug', props: true, component: ViewCurrency,
    meta: { isPublic: true, onNav: false }
  },
  // Authentication-specific routing
  {
    path: '/oidc-callback', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
    name: 'oidcCallback',
    display: 'OIDC Callback',
    component: OidcCallback,
    meta: { isPublic: true, onNav: false }
  },
  // Authentication-specific routing
  {
    path: '/oidc-callback-error', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
    name: 'oidcCallbackError',
    display: 'Nozomi Auth Error',
    component: OidcCallbackError,
    meta: { isPublic: true, onNav: false }
  },
  // Dashboard home routing
  { name: 'dashboard', path: '/dashboard', props: true, component: DashboardHome,
    meta: { isPublic: false, onNav: false }
  },
  { name: 'modify-account', path: '/dashboard/modify-account', props: true, component: ModifyProfile,
    meta: { isPublic: false, onNav: false }
  },
];
