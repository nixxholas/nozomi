import Vue from 'vue';
import VueRouter from 'vue-router';
import { NotificationProgrammatic as Notification } from 'buefy';
import store from '../store/index'
import { routes } from './routes';

let router = new VueRouter({
  mode: 'history',
  routes
});

Vue.use(VueRouter);

// router.beforeEach((to, from, next) => {
//   const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
//   if (requiresAuth) {
//     mgr.getRole().then(
//       success => {
//         if (to.meta.role === success){
//           next();
//         } else {
//           next('/accessdenied');
//         }
//       },
//       err => {
//         console.log(err);
//       }
//     );
//   } else {
//     next();
//   }
// });

// This shouldn't be together with the router's initialisation
// https://router.vuejs.org/guide/advanced/navigation-guards.html#in-component-guard
// Before each route is accessed,
router.beforeEach((to, from, next) => {
    // If the target is demanding auth,
    if (to.matched.some(record => !record.meta.isPublic)) {
      // And if he is already auth'ed,
      if (store.getters.isLoggedIn) {
        next(); // Let him go
        return
      }

      Notification.open({
        duration: 3000,
        message: `Please login first.`,
        position: 'is-bottom-right',
        type: 'is-danger',
        hasIcon: true
      });
      next(from.fullPath); // Send him back
    } else {
      next()
    }
  });

export default router
