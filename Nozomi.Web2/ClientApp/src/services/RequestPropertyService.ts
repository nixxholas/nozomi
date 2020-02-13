import axios from "axios";
import store from "@/store";

const baseUrl = 'api/RequestProperty/';
export default {
    get(guid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'Get/' + guid, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    getAllByRequest(requestGuid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'GetAllByRequest/' + requestGuid, {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    create(vm: object) {
        return new Promise((resolve, reject) => {
            axios.post(baseUrl  + 'Create/',  vm, {
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

    update(vm: object) {
        return new Promise((resolve, reject) => {
            axios.put(baseUrl  + 'Update/',  vm, {
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

    delete(guid: string) {
        return new Promise((resolve, reject) => {
            axios.delete(baseUrl  + 'Delete/' + guid, {
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