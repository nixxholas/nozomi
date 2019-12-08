import axios from 'axios';
import Converter from '../helpers/converter';

export default {
    all(page = 0, itemsPerPage = 50, sourceGuid = null, mainTicker = null, orderAscending = true, 
        orderingParam = "TickerPair") {
        return new Promise((resolve, reject) => {
            axios.get('/api/CurrencyPair/All', {
                params: {
                    page: page,
                    itemsPerPage: itemsPerPage,
                    sourceGuid: sourceGuid,
                    mainTicker: mainTicker,
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
    
    getCount(mainTicker = null) {
        return new Promise((resolve, reject) => {
            axios.get('/api/CurrencyPair/Count', {
                params: {
                    mainTicker: mainTicker
                }
            }).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    }
}
