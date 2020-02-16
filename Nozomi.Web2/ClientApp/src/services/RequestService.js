import axios from 'axios';
import store from '../store/index';

export default {
    getAllForUser() {
        return new Promise((resolve, reject) => {
            axios.get('/api/Request/GetAll', {
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
    
    create(vm) {
        if (!vm)
            throw new Error("Invalid payload. Please try again.");

        return new Promise((resolve, reject) => {
            axios.post('/api/Request/Create', vm, {
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
            axios.put('/api/Request/Update', vm, {
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
    
    delete(guid) {
        if (!guid)
            throw new Error("Invalid GUID. Please try again.");

        return new Promise((resolve, reject) => {
            axios.delete('/api/Request/Delete/' + guid, {
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
