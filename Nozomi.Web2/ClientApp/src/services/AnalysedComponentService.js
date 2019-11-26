import store from '../../store/index';
import axios from 'axios';


export default {
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
            axios.get('/api/AnalysedComponent/All', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            }).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    }
}