import axios from "axios";
import store from "../store";

export default {
    all(index) {

        return new Promise((resolve, reject) => {
            axios.get('/api/ComponentType/All', {
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