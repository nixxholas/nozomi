import axios from 'axios';
import store from '../store/index';
import Converter from '../helpers/converter';

export default {
    getCurrencyData(page = 1, itemsPerPage = 50, currencySortType = "None" , 
                    type = "CRYPTO", sortType = "MarketCap",
                    typesToTake = ["MarketCap"], descendingOrder = true) {
        if (!typesToTake)
            typesToTake = ["MarketCap"];

        return new Promise((resolve, reject) => {
            axios.get('/api/Currency/All?' +
                Converter.arrayToString("typesToTake", typesToTake), {
                params: {
                    currencyType: type,
                    currencySortType: currencySortType,
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

    list(slug = "") {
        return new Promise((resolve, reject) => {
            axios.get('/api/Currency/List', {
                params: {
                    slug: slug
                }
            })
                .then(function (response) {
                    resolve(response.data);
                }).catch(function (error) {
                reject(error);
            });
        });
    },

    listAll(page = 0, itemsPerPage = 50, currencyTypeName = null, orderAscending = false,
            orderingParam = "Abbreviation") {
        return new Promise((resolve, reject) => {
            axios.get('/api/Currency/ListAll', {
                params: {
                    currencyTypeName: currencyTypeName,
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
    },

    create(vm) {
        if (!vm)
            throw new Error("Invalid payload. Please try again.");

        return new Promise((resolve, reject) => {
            axios.post('/api/Currency/Create', vm, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    edit(vm) {
        if (!vm)
            throw new Error("Invalid payload. Please try again.");

        return new Promise((resolve, reject) => {
            axios.put('/api/Currency/Edit', vm, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    }
}
