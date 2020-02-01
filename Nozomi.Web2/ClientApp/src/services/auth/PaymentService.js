import store from '../store/index';
import { oidcSettings } from '../store/config';
import axios from 'axios';

export default {
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
}