// @ts-ignore
import HomePage from '../views/home';
// @ts-ignore
import About from '../views/About';
// @ts-ignore
import ApiTerms from '../views/legal/api-terms';
// @ts-ignore
import ViewCurrency from '../views/currency/View';
// @ts-ignore
import Changelog from '../views/Changelog';
// @ts-ignore
import CurrencyIndex from '../views/currency/Index';
// @ts-ignore
import SourceIndex from '../views/source/Index';
// @ts-ignore
import Demo from '../views/demo/index';
// @ts-ignore
import DashboardHome from '../views/dashboard/Index';
// @ts-ignore
import SettingsIndex from '../views/settings/Index';
// @ts-ignore
import OidcCallback from '../components/auth/oidc-callback';
// @ts-ignore
import OidcCallbackError from '../components/auth/oidc-callback-error';
// @ts-ignore
import Pricing from '../views/Pricing';
// @ts-ignore
import Bugs from '../views/Bugs';

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home',
    meta: { icon: 'home', isPublic: true, onNav: false }
  },
  // { name: 'currency-index', path: '/currencies', props: true, display: 'Currencies',
  //   component: CurrencyIndex,
  //   meta: { icon: 'coins', isPublic: true, onNav: true }
  // },
  // { name: 'source-index', path: '/sources', props: true, display: 'Sources', 
  //   component: SourceIndex,
  //   meta: { icon: 'landmark', isPublic: true, onNav: true }
  // },
  {
    name: 'demo-index', path: '/demo', props: true, display: 'Demo',
    component: Demo,
    meta: {icon: '', isPublic: true, onNav: true}
  },
  { name: 'about', path: '/about', component: About, display: 'About',
    meta: { icon: 'info', isPublic: true, onNav: false }
  },
  { name: 'api-terms', path: '/legal/api-terms', component: ApiTerms, display: 'API Terms',
    meta: { icon: 'info', isPublic: true, onNav: false }
  },
  { name: 'pricing', path: '/pricing', component: Pricing, display: 'Pricing',
    meta: { icon: '', isPublic: true, onNav: true }
  },
  { name: 'changelog', path: '/changelog', component: Changelog, display: 'Changelog',
    meta: { icon: 'info', isPublic: true, onNav: false }
  },
  { name: 'bugs-and-issues', path: '/bugs', component: Bugs, display: 'Bugs & Issues',
    meta: { icon: 'bug', isPublic: true, onNav: false }
  },
  // Currency-specific routing
  { name: 'view-currency', path: '/currency/:slug', props: true,
    display: 'View Currency', component: ViewCurrency,
    meta: { icon: null, isPublic: true, onNav: false }
  },
  // Authentication-specific routing
  {
    path: '/oidc-callback', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
    name: 'oidcCallback', display: 'Nozomi OIDC Redirection',
    component: OidcCallback,
    meta: { icon: null, isPublic: true, onNav: false }
  },
  // Authentication-specific routing
  {
    path: '/oidc-callback-error', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
    name: 'oidcCallbackError', display: 'Nozomi OIDC Callback Error',
    component: OidcCallbackError,
    meta: { icon: null, isPublic: true, onNav: false }
  },
  // Dashboard home routing
  { name: 'dashboard', path: '/dashboard', props: true, component: DashboardHome,
    meta: { icon: 'columns', isPublic: false, onNav: false }
  },
  { name: 'settings', path: '/settings', props: true, component: SettingsIndex,
    meta: { icon: null, isPublic: false, onNav: false }
  },
];