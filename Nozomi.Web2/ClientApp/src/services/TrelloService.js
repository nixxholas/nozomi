import axios from 'axios';

export default {
    getList(boardId = null) {
        return new Promise((resolve, reject) => {
            axios.get(`/api/Trello/Lists/${boardId}`)
                .then(function(response) {
                    resolve(response.data);
                })
                .catch(function(error) {
                    reject(error);
                });
        });
    },
    
    getCards(boardId = null, listId = null) {
        return new Promise((resolve, reject) => {
            axios.get(`/api/Trello/Cards/${boardId}/list/${listId}`)
                .then(function(response) {
                    resolve(response.data);
                }).catch(function(error) {
                    reject(error);            
                });
        });
    },
    
    getChecklists(boardId = null, listId = null, cardId = null) {
        return new Promise((resolve, reject) => {
             axios.get(`/api/Trello/Checklists/${boardId}/List/${listId}/Card/${cardId}`)
                 .then(function(response) {
                     resolve(response.data);
                 })
                 .catch(function(error) {
                     reject(error);
                 });
        });
    }
}