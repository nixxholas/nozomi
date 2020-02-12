import axios from "axios";

const baseUrl = 'api/RequestProperty/';
export default {
    getAllByRequest(guid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'GetAllByRequest/' + guid).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
}