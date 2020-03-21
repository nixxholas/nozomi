import store from '@/store/';
import {oidcSettings} from '@/store/config';
import axios from 'axios';

export default {
    get() {
        return new Promise(async (resolve, reject) => {
            try {
                const response = await axios.get(`${oidcSettings.authority}/ApiKey/All`, {
                    headers: {
                        Authorization: `Bearer ${store.state.oidcStore.access_token}`
                    }
                });

                if (response.status === 200) {
                    return resolve(response.data);
                }

                reject({message: "Could not retrieve API keys"});
            } catch (e) {
                reject(e);
            }
        });
    },

    insert(label) {
        return new Promise(async (resolve, reject) => {
            try {
                const response = await axios.post(`${oidcSettings.authority}/ApiKey/Create/${label}`, {}, {
                    headers: {
                        Authorization: `Bearer ${store.state.oidcStore.access_token}`
                    },
                });

                if (response.status === 200) {
                    return resolve(response.data);
                }

                reject({message: "Unable to generate API key"});
            } catch (e) {
                reject(e);
            }
        });
    },

    remove(apiKey) {
        return new Promise(async (resolve, reject) => {
            try {
                const response = await axios.delete(`${oidcSettings.authority}/ApiKey/Revoke/${apiKey}`, {
                    headers: {
                        Authorization: `Bearer ${store.state.oidcStore.access_token}`
                    }
                });

                if (response.status === 200) {
                    return resolve(response.data);
                }

                reject({message: "API key could not be revoked"});
            } catch (e) {
                reject(e);
            }
        });
    }
}