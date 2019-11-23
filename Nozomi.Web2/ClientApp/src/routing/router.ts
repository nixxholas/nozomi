import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '../store/index'
// @ts-ignore
import { vuexOidcCreateRouterMiddleware } from 'vuex-oidc';
import { routes } from "@/routing/routes";

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: routes
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