import Vue from 'vue';
import Vuex from 'vuex';
import VuexPersist from 'vuex-persist';
import { vuexOidcCreateStoreModule } from 'vuex-oidc';
import { NotificationProgrammatic as Notification } from 'buefy'
import { oidcSettings } from "./config";
import 'babel-polyfill';
//import axios from "axios";

const vuexPersist = new VuexPersist({
  key: 'nozomi-spa',
  storage: window.localStorage
});

Vue.use(Vuex);

// const baseUrl = process.env.NODE_ENV === "production" ? "https://auth.nozomi.one" : 'https://localhost:6001';

export default new Vuex.Store({
  modules: {
    oidcStore: vuexOidcCreateStoreModule(oidcSettings,
      // NOTE: If you do not want to use localStorage for tokens, in stead of just passing oidcSettings, you can
      // spread your oidcSettings and define a userStore of your choice
      // {
      //   ...oidcSettings,
      //   userStore: new WebStorageStateStore({ store: window.sessionStorage })
      // },
      // Optional OIDC store settings
      {
        namespaced: true,
        dispatchEventsOnWindow: true
      },
      // Optional OIDC event listeners
      {
        userLoaded: (user) =>  {
          // console.log('OIDC user is loaded:', user)
          Notification.open({
            duration: 3000,
            message: 'Successfully authenticated!',
            position: 'is-bottom-right',
            type: 'is-success',
            hasIcon: true
          })
        },
        userUnloaded: () => console.log('OIDC user is unloaded'),
        accessTokenExpiring: () => console.log('Access token will expire'),
        accessTokenExpired: () => console.log('Access token did expire'),
        silentRenewError: () => console.log('OIDC user is unloaded'),
        userSignedOut: () => console.log('OIDC user is signed out'),
        oidcError: (payload) => console.log('OIDC error', payload)
      })
  },
  state: {

  },
  mutations: {

  },
  actions: {

  },
  plugins: [vuexPersist.plugin]
})
