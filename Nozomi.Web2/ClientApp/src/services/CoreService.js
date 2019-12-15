import axios from 'axios';
import store from '../store/index';

export default {
    getCurrentBuildTime() {
        return new Promise((resolve, reject) => {
            axios.get('/api/Core/GetCurrentBuildTime/').then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    getUserDetails() {
        return new Promise((resolve, reject) => {
            axios.get('/api/Core/GetUserDetails', {
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
