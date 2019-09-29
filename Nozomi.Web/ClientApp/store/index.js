import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER';

// STATE
const state = {
  status: '',
  token: localStorage.getItem('token') || '',
  user : {},
  counter: 1
};

// MUTATIONS
const mutations = {
  auth_request(state){
    state.status = 'loading'
  },
  auth_success(state, token, user){
    state.status = 'success';
    state.token = token;
    state.user = user;
  },
  auth_error(state){
    state.status = 'error';
  },
  logout(state){
    state.status = '';
    state.token = '';
  },
  [MAIN_SET_COUNTER] (state, obj) {
    state.counter = obj.counter
  }
};

// GETTERS
const getters = ({
  isLoggedIn: state => !!state.token,
  authStatus: state => state.status,
});

// ACTIONS
const actions = ({
  // Login API
  login({commit}, user){
    return new Promise((resolve, reject) => {
      commit('auth_request');

      console.dir("logging in");
      console.dir(user);
      commit('auth_error');

      // axios({url: 'http://localhost:3000/login', data: user, method: 'POST' })
      //   .then(resp => {
      //     const token = resp.data.token;
      //     const user = resp.data.user;
      //     localStorage.setItem('token', token);
      //     axios.defaults.headers.common['Authorization'] = token;
      //     commit('auth_success', token, user);
      //     resolve(resp)
      //   })
      //   .catch(err => {
      //     commit('auth_error');
      //     localStorage.removeItem('token');
      //     reject(err)
      //   })
    })
  },
  // Registration API
  register({commit}, user){
    return new Promise((resolve, reject) => {
      commit('auth_request');
      axios({url: 'http://localhost:3000/register', data: user, method: 'POST' })
        .then(resp => {
          const token = resp.data.token;
          const user = resp.data.user;
          localStorage.setItem('token', token);
          axios.defaults.headers.common['Authorization'] = token;
          commit('auth_success', token, user);
          resolve(resp)
        })
        .catch(err => {
          commit('auth_error', err);
          localStorage.removeItem('token');
          reject(err);
        })
    })
  },
  // Logout API
  logout({commit}){
    return new Promise((resolve, reject) => {
      commit('logout');
      localStorage.removeItem('token');
      delete axios.defaults.headers.common['Authorization'];
      resolve();
    })
  },
  setCounter ({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj)
  }
});

export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters
})
