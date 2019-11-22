import 'core-js/stable';
import 'regenerator-runtime/runtime';
import Vue from 'vue';
import Buefy from 'buefy';
import 'buefy/dist/buefy.css';
import 'vue-material-design-icons/styles.css';
import './plugins/axios';
// @ts-ignore
import * as numeral from 'numeral';
// @ts-ignore
import VueNumerals from 'vue-numerals';
import VueApexCharts from 'vue-apexcharts';
import * as moment from 'moment';
// import vuetify from './plugins/vuetify';
import App from './App.vue';
// @ts-ignore
import router from '@/routing/router';
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
// Registration of Vue Numerals
Vue.use(VueNumerals); // default locale is 'en'
// Registration of Vue apexcharts
Vue.component('apexchart', VueApexCharts);

Vue.prototype.$moment = moment;
Vue.prototype.$numeral = numeral;

Vue.config.productionTip = false;

Vue.filter('date', dateFilter);

new Vue({
  // vuetify,
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
