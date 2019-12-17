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
    },

    listAll(page = 0, itemsPerPage = 50, orderAscending = true, 
            orderingParam = "TypeShortForm") {
        return new Promise((resolve, reject) => {
            axios.get('/api/CurrencyType/ListAll', {
              params: {
                page: page, 
                itemsPerPage: itemsPerPage, 
                orderAscending: orderAscending,
                orderingParam: orderingParam
              }
            })
                .then(function (response) {
                    resolve(response.data);
                }).catch(function (error) {
                reject(error);
            });
        });
    },
}
