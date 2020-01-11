import axios from 'axios';
import Converter from '../helpers/converter';

export default {
    all(index = 0, itemsPerPage = 200) {
        return new Promise((resolve, reject) => {
            axios.get('/api/CurrencyType/All', {
                params: {
                    index: index,
                    itemsPerPage: itemsPerPage
                }
            }).then(function (response) {
                resolve(response);
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
