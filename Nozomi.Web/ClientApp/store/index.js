import Vue from 'vue';
import Vuex from 'vuex';
import VuexPersist from 'vuex-persist'
import {NotificationProgrammatic as Notification} from 'buefy';
import Oidc from 'oidc-client';
import { oidcSettings } from "./config";
import 'babel-polyfill';
import axios from "axios";

const vuexPersist = new VuexPersist({
  key: 'nozomi-spa',
  storage: window.localStorage
});

Vue.use(Vuex);

const baseUrl = process.env.NODE_ENV === "production" ? "https://auth.nozomi.one" : 'https://localhost:6001';

// OIDC CLIENT MANAGER
const mgr = new Oidc.UserManager(oidcSettings);

Oidc.Log.logger = console;
Oidc.Log.level = Oidc.Log.INFO;

mgr.events.addUserLoaded(function (user) {
  console.log('New User Loaded：', arguments);
  console.log('Access_token: ', user.access_token)
});

mgr.events.addAccessTokenExpiring(function () {
  console.log('AccessToken Expiring：', arguments);
  if (oidcSettings.automaticSilentRenew)
    context.dispatch('signInSilent');
});

mgr.events.addAccessTokenExpired(function () {
  console.log('AccessToken Expired：', arguments);
  alert('Session expired. Going out!');
  mgr.signoutRedirect().then(function (resp) {
    console.log('signed out', resp);
    context.commit('unsetOidcAuth');
  }).catch(function (err) {
    console.log(err)
  })
});

mgr.events.addSilentRenewError(function () {
  console.error('Silent Renew Error：', arguments);
});

mgr.events.addUserSignedOut(function () {
  alert('Going out!');
  console.log('UserSignedOut：', arguments);
  mgr.signoutRedirect().then(function (resp) {
    context.commit('unsetOidcAuth');
    // console.log('signed out', resp);
  }).catch(function (err) {
    console.log(err)
  })
});

// TYPES
//const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER';

// STATE
// states bring stashed data required to components
const state = {
  status: '',
  access_token: null,
  id_token: null,
  scopes: null,
  user: {},
  is_checked: false,
  events_are_bound: false,
  error: null
};

// MUTATIONS
const mutations = {
  // Transfer all user payload to current state
  setOidcAuth (state, user) {
    state.id_token = user.id_token;
    state.access_token = user.access_token;
    state.user = user.profile;
    state.scopes = user.scopes;
    state.error = null;
  },
  // Resets the user state
  unsetOidcAuth (state) {
    state.id_token = null;
    state.access_token = null;
    state.user = null;
  },
  setOidcEventsAreBound (state) {
    state.events_are_bound = true;
  },
  setOidcAuthIsChecked (state) {
    state.is_checked = true;
  },
  setOidcError (state, payload) {
    state.error = payload.error; // TODO: Handle/Output erroneous data.
  }
  // [MAIN_SET_COUNTER] (state, obj) {
  //   state.counter = obj.counter
  // }
};

// GETTERS
const getters = ({
  isLoggedIn: state => state.id_token,
  authStatus: state => state.status,
  getAccessToken: async () => {
    await user.getAccessToken().then(
      accessToken => {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + accessToken
      }, err => {
        console.log(err)
      })
  },
  // Get the user who is logged in
  getUserExplicitly: () => {
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        return user;
      }).catch(function (err) {
        console.log(err);
        return null;
      });
    })
  },
});

// ACTIONS
// All backend API communications should only be done here
const actions = ({
  // Renew the token manually
  renewToken() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.signinSilent().then(function (user) {
        if (user == null) {
          self.signIn(null)
        } else {
          return resolve(user)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Obtain the authorization headers
  async defineHeaderAxios() {
    //let self = this;
    return new Promise((resolve, reject) => {
      mgr.getAccessToken().then(function (accessToken) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + accessToken
      }, err => {
        console.log(err)
      })
    });
  },

  // API Caller lol
  async getAll(api) {
    let self = this;
    return new Promise((resolve, reject) => {
      self.defineHeaderAxios();
      axios
        .get(baseUrl + api)
        .then(response => response.data)
        .catch(err => {
          console.log(err);
        });
    });
  },

  // Get the user who is logged in
  getUser() {
    let self = this;
    return new Promise((resolve, reject) => {
      // TODO: Should we comply with this?
      // https://github.com/IdentityModel/oidc-client-js/issues/689
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Check if there is any user logged in
  getSignedIn() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(false) // Not authenticated yet lol
        } else {
          return resolve(true)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  signInSilent (context) {
    mgr.signinSilent().then(user => {
      context.dispatch('oidcWasAuthenticated', user);
    })
      .catch(err => {
        context.commit('setOidcError', errorPayload('signInSilent', err));
        context.commit('setOidcAuthIsChecked');
      })
  },

  // Redirect of the current window to the authorization endpoint.
  signIn(context) {
    let self = this;
    mgr.signinRedirect()
      .then(function (resp) {
        // console.log('signed in!', resp);
        state.user = self.getSignedIn();
      })
      .catch(function (err) {
        console.log(err);
      })
  },

  // Redirect of the sign in from the authorization endpoint.
  oidcSignInCallback (context, url) {
    return new Promise((resolve, reject) => {
      console.dir("Requested URL: " + url);
      mgr.signinRedirectCallback(url)
        .then(user => {
          context.dispatch('oidcWasAuthenticated', user);
          resolve(sessionStorage.getItem('vuex_oidc_active_route') || '/')
        })
        .catch(err => {
          context.commit('setOidcError', errorPayload('oidcSignInCallback', err));
          context.commit('setOidcAuthIsChecked');
          reject(err)
        })
    })
  },

  // usually triggered by the OIDC sign callback. Allows us to process the successful authentication.
  oidcWasAuthenticated (context, user) {
    context.commit('setOidcAuth', user); // Stash the user data.
    if (!context.state.events_are_bound) // Declare that all usermanager events are bound.
      context.commit('setOidcEventsAreBound');
    context.commit('setOidcAuthIsChecked') // Declare that the auth is processed.
  },

  // Redirect of the current window to the end session endpoint
  signOut() {
    mgr.signoutRedirect().then(function (resp) {
      console.log('signed out', resp);
    }).catch(function (err) {
      console.log(err);
    })
  },

  // Get the profile of the user logged in
  getProfile() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user.profile)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Get the token id
  getIdToken() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user.id_token)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Get the session state
  getSessionState() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user.session_state)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Get the access token of the logged in user
  getAccessToken() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user.access_token)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Takes the scopes of the logged in user
  getScopes() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user.scopes)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  },

  // Get the user roles logged in
  getRole() {
    let self = this;
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn();
          return resolve(null)
        } else {
          return resolve(user.profile.role)
        }
      }).catch(function (err) {
        console.log(err);
        return reject(err)
      });
    })
  }
});

export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters,
  plugins: [vuexPersist.plugin]
})
