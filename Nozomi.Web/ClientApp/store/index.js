import Vue from 'vue';
import Vuex from 'vuex';
import { vuexOidcCreateStoreModule } from 'vuex-oidc';
import { oidcSettings } from './config';
import { NotificationProgrammatic as Notification } from 'buefy';

Vue.use(Vuex);

// TYPES
//const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER';

// STATE
// states bring stashed data required to components
const state = {
  status: '',
  token: localStorage.getItem('token') || '',
  user : {}
};

// MUTATIONS
const mutations = {
  // [MAIN_SET_COUNTER] (state, obj) {
  //   state.counter = obj.counter
  // }
};

// GETTERS
const getters = ({
  isLoggedIn: state => !!state.token,
  authStatus: state => state.status,
});

// ACTIONS
// All backend API communications should only be done here
const actions = ({
});

export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters,
  modules: {
    oidcStore: vuexOidcCreateStoreModule(oidcSettings)
  }
})
