import axios from 'axios';
import store from '../store/index';

export default {
    all() {
        return new Promise((resolve, reject) => {
            axios.get('/api/ResponseType/All', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(response => {
                    return resolve(response.data);
                })
                .catch(e => {
                    reject(e);
                });
        });
    }
}