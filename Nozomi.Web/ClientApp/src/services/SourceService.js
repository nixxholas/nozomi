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
  }
}
