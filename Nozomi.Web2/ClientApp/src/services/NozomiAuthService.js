import store from '../store/index';
import { oidcSettings } from '../store/config';
import axios from 'axios';

export default {
    bootstripe() {
        return new Promise((resolve, reject) => {
            axios.head(oidcSettings.authority + '/Account/Bootstripe', {
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
    
    update(vm) {
        if (!vm)
            throw new Error("Invalid payload. Please try again.");

        return new Promise((resolve, reject) => {
            axios.put(oidcSettings.authority + '/Account/Update', vm, {
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
        
    resendConfirmationEmail() {
        return new Promise((resolve, reject) => {
            const { authority, client_id, redirect_uri, response_type, scope } = oidcSettings;
            const postData = {
                returnUrl: redirect_uri
            };
            
            axios.post(`${authority}/Account/ResendConfirmationEmail`, postData, {
                headers: {
                    Authorization: `Bearer ${store.state.oidcStore.access_token}`
                }
            })
                .then(response => {
                    if (response.status === 200) {
                        resolve(response.data);
                    } else if (response.status === 400) {
                        reject("Invalid information provided");
                    } else {
                        reject("Unauthorized access");
                    }
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
}