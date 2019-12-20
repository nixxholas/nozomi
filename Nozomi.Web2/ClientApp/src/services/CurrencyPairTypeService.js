import axios from 'axios';
import Converter from '../helpers/converter';

export default {
    all() {
        return new Promise((resolve, reject) => {
            axios.get('/api/CurrencyPairType/All').then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
}