import axios from "axios";

const baseUrl = 'api/RequestProperty/';
export default {
    get(guid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'Get/' + guid).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
    
    getAllByRequest(requestGuid: string) {
        return new Promise((resolve, reject) => {
            axios.get(baseUrl  + 'GetAllByRequest/' + requestGuid).then(function (response) {
                resolve(response.data);
            }).catch(function (error) {
                reject(error);
            });
        });
    },
}