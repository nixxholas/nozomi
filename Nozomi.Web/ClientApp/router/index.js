import Vue from 'vue'
import VueRouter from 'vue-router'
import { routes } from './routes'

Vue.use(VueRouter);

let router = new VueRouter({
  mode: 'history',
  routes
})
// Before each route is accessed,
.beforeEach(async (to, from, next) => {
  let app = router.app.$data || {isAuthenticated: false} ;
  if (app.isAuthenticated) {
    // already signed in, we can navigate anywhere
    next()
  } else if (to.matched.some(record => record.meta.requiresAuth)) {
    // authentication is required. Trigger the sign in process, including the return URI
    router.app.authenticate(to.path).then(() => {
      console.log('authenticating a protected url:' + to.path);
      next();
    });
  } else {
    //No auth required. We can navigate
    next()
  }
});

export default router
