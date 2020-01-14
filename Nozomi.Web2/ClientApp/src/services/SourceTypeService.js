import axios from 'axios';
import store from "../store";

export default {
  getAll() {
    return new Promise((resolve, reject) => {
      axios.get('/api/SourceType/All').then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  },
  
  create(vm) {
    return new Promise((resolve, reject) => {
      axios.post('/api/SourceType/Create', vm, {
        headers: {
          Authorization: "Bearer " + store.state.oidcStore.access_token
        }
      }).then(function (response) {
        resolve(response);
      }).catch(function (error) {
        reject(error);
      });
    });
  },
  
  update(vm) {
    return new Promise((resolve, reject) => {
      axios.put('/api/SourceType/Update', vm, {
        headers: {
          Authorization: "Bearer " + store.state.oidcStore.access_token
        }
      }).then(function (response) {
        resolve(response);
      }).catch(function (error) {
        reject(error);
      });
    });
  },
}
