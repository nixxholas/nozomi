import store from '../store/index';
import axios from 'axios';

export default {
    get(guid) {
        return new Promise((resolve, reject) => {
            axios.get('/api/AnalysedComponent/Get/' + guid, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    resolve(response);
                }).catch(function (error) {
                reject(error);
            });
        });
    },

    // TODO: Haven't started lol
    getAll() {
        return new Promise((resolve, reject) => {
            axios.get('/api/AnalysedComponent/All').then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    getTypes() {
        return new Promise((resolve, reject) => {
            axios.get('/api/AnalysedComponentType/All')
                .then(function (response) {
                    resolve(response.data);
                }).catch(function (error) {
                reject(error);
            });
        });
    }
}