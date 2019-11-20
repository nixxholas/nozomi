import axios from 'axios';
import Converter from '../helpers/converter';

export default {
  getAll() {
    return new Promise((resolve, reject) => {
      axios.get('/api/CurrencyType/All').then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  }
}
