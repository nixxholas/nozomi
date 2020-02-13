import axios from "axios";
import store from "@/store";

const baseUrl = 'api/RequestPropertyType/';
export default {
    all() {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'All', {
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
}