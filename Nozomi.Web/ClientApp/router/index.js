import Vue from 'vue'
import VueRouter from 'vue-router'
import store from '../store/index';
import { routes } from './routes'

let router = new VueRouter({
  mode: 'history',
  routes
});

// This shouldn't be together with the router's initialisation
// https://router.vuejs.org/guide/advanced/navigation-guards.html#in-component-guard
// Before each route is accessed,
router.beforeEach((to, from, next) => {
    console.dir(to);
    console.dir(from);
    if(to.matched.some(record => record.meta.requiresAuth)) {
      if (store.getters.isLoggedIn) {
        next();
        return
      }
      next('/login')
    } else {
      next()
    }
  });

Vue.use(VueRouter);

export default router
