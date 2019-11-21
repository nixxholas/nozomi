import 'core-js/stable';
import 'regenerator-runtime/runtime';
import Vue from 'vue';
import Buefy from 'buefy';
// import 'buefy/dist/buefy.css';
import './plugins/axios';
import vuetify from './plugins/vuetify';
import App from './App.vue';
import router from './router';
import store from '@/store/index';
import './registerServiceWorker';
import dateFilter from '@/filters/date.filter';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faAtlas, faHome, faInfo, faUniversity } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

// Font awesome configurations
library.add(faAtlas, faHome, faInfo, faUniversity);
Vue.component('icon', FontAwesomeIcon);

// Registration of Buefy
Vue.use(Buefy);

Vue.config.productionTip = false;

Vue.filter('date', dateFilter);

new Vue({
  vuetify,
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
