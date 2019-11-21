import Vue from 'vue';
import Vuex, {Plugin, StoreOptions} from 'vuex';
import VuexPersist from 'vuex-persist';
import { oidcSettings } from "./config";
// TODO: Wait for the vuex-oidc PR for typings to merge
// @ts-ignore
import { vuexOidcCreateStoreModule } from 'vuex-oidc';
import { RootState } from './types';
import { counter } from './counter/index';
import {NotificationProgrammatic as Notification} from "buefy";

const vuexPersist = new VuexPersist<RootState>({
  key: 'nozomi-spa',
  storage: window.localStorage
});

Vue.use(Vuex);

// Vuex structure based on https://codeburst.io/vuex-and-typescript-3427ba78cfa8

const store: StoreOptions<RootState> = {
  state: {
    version: '1.0.0', // a simple property
  },
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
          userLoaded: (user: any) =>  {
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
          oidcError: (payload: any) => console.log('OIDC error', payload)
        }),
    counter,
  },
  plugins: [vuexPersist.plugin]
};

export default new Vuex.Store<RootState>(store);