import Vue from 'vue';
import Buefy from 'buefy';
import 'buefy/dist/buefy.css';
import axios from 'axios';
import router from './router/index';
import store from './store';
import { sync } from 'vuex-router-sync';
import App from 'components/app-root';
import { FontAwesomeIcon } from './icons';''

// Registration of global components
Vue.component('icon', FontAwesomeIcon);

// Registration of Buefy
Vue.use(Buefy);

// Registration of Axios
// https://stackoverflow.com/questions/51374367/axios-is-not-defined-in-vue-js-cli
Vue.prototype.$http = axios;
Vue.prototype.$axios = axios;

sync(store, router);

const app = new Vue({
  store,
  router,
  ...App
});

export {
  app,
  router,
  store
}
