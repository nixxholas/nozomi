import axios from 'axios';
import Converter from '../helpers/converter';
import store from "../store";

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
    },

    create(vm) {
        if (!vm)
            throw new Error("Invalid payload. Please try again.");

        return new Promise((resolve, reject) => {
            axios.post('/api/CurrencyPair/Create', vm, {
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
            axios.put('/api/CurrencyPair/Edit', vm, {
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
}
