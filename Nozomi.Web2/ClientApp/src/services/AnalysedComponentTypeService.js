import axios from 'axios';

export default {
    all() {
        return new Promise((resolve, reject) => {
            axios.get('/api/AnalysedComponentType/All')
                .then(function (response) {
                    resolve(response);
                }).catch(function (error) {
                reject(error);
            });
        });
    },
}