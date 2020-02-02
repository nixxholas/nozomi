import 'core-js/stable';
import 'regenerator-runtime/runtime';
import Vue from 'vue';
import Buefy from 'buefy';
import 'buefy/dist/buefy.css';
import '@/assets/main.css';
import 'bulma-extensions/dist/css/bulma-extensions.min.css'
import 'bulma-extensions/dist/js/bulma-extensions.min.js';
import './plugins/axios';
// @ts-ignore
import * as numeral from 'numeral';
// @ts-ignore
import VueNumerals from 'vue-numerals';
import VueApexCharts from 'vue-apexcharts';
import * as moment from 'moment';
import App from './App.vue';
// @ts-ignore
import router from '@/routing/router';
import store from '@/store/index';
import dateFilter from '@/filters/date.filter';

// FontAwesome!!!
import { library } from '@fortawesome/fontawesome-svg-core';
// Vue Injection
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
// Vue.component('icon', FontAwesomeIcon);
Vue.component('vue-fontawesome', FontAwesomeIcon);

// Font awesome solid icon configurations
import { faAngleLeft, faAngleRight, faArrowUp, faAtlas, faBug, faCaretDown, 
  faCheck, faCheckCircle, faChevronDown, faCog,
  faCoins, faColumns, faCreditCard, faDumpster, faEnvelope, faExclamationCircle, 
  faExclamationTriangle,
  faEye, faFingerprint, faFrown, faHome, faInfo, faInfoCircle, faKey, faLandmark, 
  faMoneyBill, faMoneyBillWave, faSadCry, faSpinner,
  faSearch, faSignOutAlt,faTicketAlt, faTimes, faTrash, faUniversity, faUser } 
  from '@fortawesome/free-solid-svg-icons';
library.add(faAngleLeft, faAngleRight, faArrowUp, faAtlas, faBug, faCaretDown, 
    faCheck, faCheckCircle, faChevronDown, faCog,
    faCoins, faColumns, faCreditCard, faDumpster, faEnvelope, faExclamationCircle, 
    faExclamationTriangle,
    faEye, faFingerprint, faFrown, faHome, faInfo, faInfoCircle, faKey, faLandmark, 
    faMoneyBill, faMoneyBillWave, faSadCry, faSpinner,
    faSearch, faSignOutAlt, faTicketAlt, faTimes, faTrash, faUniversity, faUser);

// Font awesome brand icon configurations
import { faBitcoin, faEthereum, faFontAwesome } from '@fortawesome/free-brands-svg-icons';
library.add(faBitcoin, faEthereum, faFontAwesome);

// Registration of Buefy
Vue.use(Buefy, {
  defaultIconPack: 'fas',
  defaultIconComponent: 'vue-fontawesome',
  // defaultContainerElement: '#content',
  // ...
});
// Registration of Vue Numerals
Vue.use(VueNumerals); // default locale is 'en'
// Registration of Vue apexcharts
Vue.component('apexchart', VueApexCharts);

Vue.prototype.$moment = moment;
Vue.prototype.$numeral = numeral;

Vue.config.productionTip = false;

Vue.filter('date', dateFilter);

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
