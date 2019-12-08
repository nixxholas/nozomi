import axios from 'axios';

export default {
    list(guid, page, itemsPerPage) {
        if (!guid || !page || !itemsPerPage || page < 0 || itemsPerPage < 1)
            throw "Invalid request.";
        
        return new Promise((resolve, reject) => {
            axios.get('/api/List/' + guid, {
                params: {
                    page: page,
                    itemsPerPage: itemsPerPage
                }
            }).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    }
}