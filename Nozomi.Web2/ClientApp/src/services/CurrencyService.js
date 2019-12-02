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

  listAll(page = 0, itemsPerPage = 50, orderAscending = false, orderingParam = "Abbreviation") {
    return new Promise((resolve, reject) => {
      axios.get('/api/Currency/ListAll', {
        params: {
          page: page,
          itemsPerPage: itemsPerPage,
          orderAscending: orderAscending,
          orderingParam: orderingParam
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
  },

  getPairCount(slug) {
    if (!slug)
      return 0;
    
    return new Promise((resolve, reject) => {
      axios.get('/api/Currency/GetPairCount/' + slug).then(function (response) {
        resolve(response.data);
      }).catch(function (error) {
        reject(error);
      });
    });
  }
}
