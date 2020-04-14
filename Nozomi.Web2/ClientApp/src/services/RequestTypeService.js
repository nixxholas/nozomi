import axios from 'axios';
import store from '../store/index';

export default {
    all() {
        return new Promise((resolve, reject) => {
            axios.get('/api/RequestType/All', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(response => {
                    if (response.status === 200) {
                        return resolve(response.data);
                    }
                    
                    reject({
                        message: "Server did not reply with status code of 200.\nCould not retrieve request types."
                    });
                })
                .catch(e => {
                    reject(e);
                });
        });
    }
}