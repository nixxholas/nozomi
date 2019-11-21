import axios from 'axios';
import Converter from '../helpers/converter';

export default {
  getCurrencyData(page = 1, itemsPerPage = 50, type = "CRYPTO", sortType = "MarketCap", typesToTake = ["MarketCap"]
  ,descendingOrder = true) {
    return new Promise((resolve, reject) => {
      axios.get('/api/Currency/All?' +
        Converter.arrayToString("typesToTake", typesToTake), {
        params: {
          currencyType: type,
          itemsPerIndex: itemsPerPage,
          index: (page - 1),
          sortType: sortType, // 1 = Market cap
          orderDescending: descendingOrder,
        }
      }).then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  },
  
  getCurrencyCount(type) {
    return new Promise((resolve, reject) => {
      axios.get('/api/Currency/GetCountByType', {
        params: {
          currencyType: type,
        }
      }).then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  }
}
