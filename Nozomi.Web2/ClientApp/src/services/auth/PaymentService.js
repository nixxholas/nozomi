import store from '@/store/index';
import { oidcSettings } from '@/store/config';
import axios from 'axios';

export default {
    addPaymentMethod(pmToken) {
        return new Promise((resolve, reject) => {
            axios.post(oidcSettings.authority + '/Payment/AddPaymentMethod', pmToken, {
                headers: {
                    // 'Content-Type': "application/json",
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    currentPlan() {
        return new Promise((resolve, reject) => {
            axios.get(oidcSettings.authority + '/Payment/CurrentPlan', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    getStripePubKey() {
        return new Promise((resolve, reject) => {
            axios.get(oidcSettings.authority + '/Payment/GetStripePubKey', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    getStripeCustId() {
        return new Promise((resolve, reject) => {
            axios.get(oidcSettings.authority + '/Payment/GetStripeCustId', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    listPaymentMethods() {
        return new Promise((resolve, reject) => {
            axios.get(oidcSettings.authority + '/Payment/ListPaymentMethods', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    plans() {
        return new Promise((resolve, reject) => {
            axios.get(oidcSettings.authority + '/Payment/Plans')
            .then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    removePaymentMethod(id) {
        return new Promise((resolve, reject) => {
            axios.delete(oidcSettings.authority + '/Payment/RemovePaymentMethod/' + id, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    stripeSetupIntent() {
        return new Promise((resolve, reject) => {
            axios.get(oidcSettings.authority + '/Payment/StripeSetupIntent', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    changeSubscription(planId) {
        if (!planId)
            reject("Invalid plan id.");

        return new Promise((resolve, reject) => {
            axios.put(oidcSettings.authority + '/Payment/ChangeSubscription/' + planId , null, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    subscribe(planId) {
        if (!planId)
            reject("Invalid plan id.");
        
        return new Promise((resolve, reject) => {
            axios.post(oidcSettings.authority + '/Payment/Subscribe/' + planId , null, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    unsubscribe() {
        return new Promise((resolve, reject) => {
            axios.delete(oidcSettings.authority + '/Payment/Unsubscribe', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
}