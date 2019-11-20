import axios from 'axios';

export default {
  getAll() {
    return new Promise((resolve, reject) => {
      axios.get('/api/SourceType/All').then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  }
}
