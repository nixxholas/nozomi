import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '../store/index'
// @ts-ignore
import { vuexOidcCreateRouterMiddleware } from 'vuex-oidc';

// @ts-ignore
import HomePage from '../views/Home';
// @ts-ignore
import About from '../views/About';
// @ts-ignore
import ViewCurrency from '../views/currency/View';
// @ts-ignore
import SourceIndex from '../views/source/Index';
// @ts-ignore
import DashboardHome from '../views/dashboard/Index';
// @ts-ignore
import ModifyProfile from '../views/dashboard/account/ModifyProfile';
// @ts-ignore
import OidcCallback from '../components/auth/oidc-callback';
// @ts-ignore
import OidcCallbackError from '../components/auth/oidc-callback-error';

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    { name: 'home', path: '/', component: HomePage,
      meta: { icon: 'home', isPublic: true, onNav: false }
    },
    { name: 'about', path: '/about', component: About, 
      meta: { icon: 'info', isPublic: true, onNav: true }
    },
    { name: 'source-index', path: '/source', props: true, component: SourceIndex, 
      meta: { icon: 'landmark', isPublic: true, onNav: true }
    },
    // Currency-specific routing
    { name: 'view-currency', path: '/currency/:slug', props: true, component: ViewCurrency,
      meta: { icon: null, isPublic: true, onNav: false }
    },
    // Authentication-specific routing
    {
      path: '/oidc-callback', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
      name: 'oidcCallback',
      component: OidcCallback,
      meta: { icon: null, isPublic: true, onNav: false }
    },
    // Authentication-specific routing
    {
      path: '/oidc-callback-error', // Needs to match redirectUri (redirect_uri if you use snake case) in you oidcSettings
      name: 'oidcCallbackError',
      component: OidcCallbackError,
      meta: { icon: null, isPublic: true, onNav: false }
    },
    // Dashboard home routing
    { name: 'dashboard', path: '/dashboard', props: true, component: DashboardHome,
      meta: { icon: 'columns', isPublic: false, onNav: false }
    },
    { name: 'modify-account', path: '/dashboard/modify-account', props: true, component: ModifyProfile,
      meta: { icon: null, isPublic: false, onNav: false }
    },
  ]
  // routes: [
  //   {
  //     path: '/',
  //     name: 'home',
  //     component: Home,
  //   },
  //   {
  //     path: '/counter',
  //     name: 'counter',
  //     // route level code-splitting
  //     // this generates a separate chunk (about.[hash].js) for this route
  //     // which is lazy-loaded when the route is visited.
  //     component: () => import(/* webpackChunkName: "counter" */ './views/Counter.vue'),
  //   },
  //   {
  //     path: '/fetch-data',
  //     name: 'fetch-data',
  //     component: () => import(/* webpackChunkName: "fetch-data" */ './views/FetchData.vue'),
  //   },
  // ],
});

router.beforeEach(vuexOidcCreateRouterMiddleware(store, 'oidcStore'));

Vue.use(VueRouter);

export default router