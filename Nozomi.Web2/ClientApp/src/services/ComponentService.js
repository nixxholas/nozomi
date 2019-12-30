import axios from 'axios';
import store from "../store";

export default {
  all(index = 0, itemsPerPage = 0,
      includeNested = false) {

    return new Promise((resolve, reject) => {
      axios.get('/api/ComponentType/All', {
        headers: {
          Authorization: "Bearer " + store.state.oidcStore.access_token
        },
        params: {
          index: index,
          itemsPerPage: itemsPerPage,
          includeNested: includeNested
        }
      }).then(function (response) {
        resolve(response);
      }).catch(function (error) {
        reject(error);
      });
    });
  },
  
  allByRequest(requestGuid, index = 0, itemsPerPage = 0,
      includeNested = false) {

    return new Promise((resolve, reject) => {
      axios.get('/api/ComponentType/AllByRequest', {
        headers: {
          Authorization: "Bearer " + store.state.oidcStore.access_token
        },
        params: {
          requestGuid: requestGuid,
          index: index,
          itemsPerPage: itemsPerPage,
          includeNested: includeNested
        }
      }).then(function (response) {
        resolve(response);
      }).catch(function (error) {
        reject(error);
      });
    });
  },

  create(vm) {
    if (!vm)
      throw new Error("Invalid payload. Please try again.");

    return new Promise((resolve, reject) => {
      axios.post('/api/Component/Create', vm, {
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
  
  // Ad-hoc functions
  
  getComponentValue(dataset, type) {
    if (dataset && dataset.length > 0) {
      let res = dataset.filter(c => c.type === type);

      if (res && res.length > 0) {
        return res[0].value;
      }
    }

    return null;
  }
}