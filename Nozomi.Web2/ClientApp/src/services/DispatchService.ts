import axios from "axios";
import store from "@/store";

const baseUrl = '/api/Dispatch/';
export default {
    fetch(vm: any) {
        return new Promise((resolve, reject) => {
            axios.post(baseUrl  + 'Fetch', vm, {
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
}