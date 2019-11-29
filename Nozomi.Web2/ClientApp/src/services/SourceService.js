import axios from 'axios';

export default {
  getAll() {
    return new Promise((resolve, reject) => {
      axios.get('/api/Source/All').then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  },

  listByCurrency(guid, page, itemsPerPage) {
    return new Promise((resolve, reject) => {
      axios.get('/api/Source/ListByCurrency/' + guid, {
        params: {
          page: page,
          itemsPerPage: itemsPerPage
        }
      }).then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  }
}
