import axios from "axios";
import store from "@/store";

const baseUrl = 'api/WebsocketCommand/';
export default {
    get(guid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'Get/' + guid, {
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

    viewByRequest(requestGuid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'ViewByRequest/' + requestGuid, {
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
    
    create(vm: any) {
        return new Promise((resolve, reject) => {
            axios.post(baseUrl  + 'Create',  vm, {
                headers: {
                    'Content-Type': "application/json", // Enforce the content type..
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },

    update(vm: any) {
        return new Promise((resolve, reject) => {
            axios.put(baseUrl  + 'Update/',  vm, {
                headers: {
                    'Content-Type': "application/json", // Enforce the content type..
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response);
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
                resolve(response);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
}