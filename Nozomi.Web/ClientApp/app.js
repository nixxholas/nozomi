import Vue from 'vue';
import Buefy from 'buefy';
import 'buefy/dist/buefy.css';
import Trend from "vuetrend";
import axios from 'axios';
import * as numeral from 'numeral';
import VueNumerals from 'vue-numerals';
import VueApexCharts from 'vue-apexcharts';
import router from './router/index';
import store from './store';
import { sync } from 'vuex-router-sync';
import App from 'components/app-root';
import { FontAwesomeIcon } from './icons';
import * as moment from 'moment';
import TvLwChart from 'components/chart/tv-lw-chart';
import * as Web3 from 'web3';

// Registration of global components
Vue.component('icon', FontAwesomeIcon);
// Registration of Vue apexcharts
Vue.component('apexchart', VueApexCharts);
Vue.component('tv-lw-chart', TvLwChart);

// Registration of Buefy
Vue.use(Buefy);
// Registration of Vue Numerals
Vue.use(VueNumerals); // default locale is 'en'
// Registration of vue-trend
Vue.use(Trend);
// Registration of Vue apexcharts
Vue.use(VueApexCharts);

// Registration of Axios
// https://stackoverflow.com/questions/51374367/axios-is-not-defined-in-vue-js-cli
Vue.prototype.$http = axios;
Vue.prototype.$axios = axios;
Vue.prototype.$moment = moment;
Vue.prototype.$numeral = numeral;
Vue.prototype.$Web3 = Web3;

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
