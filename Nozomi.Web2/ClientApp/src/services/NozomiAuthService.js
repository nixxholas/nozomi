import store from '../store/index';
import { oidcSettings } from '../store/config';
import axios from 'axios';

export default {
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
    }
}