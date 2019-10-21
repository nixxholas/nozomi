<template>
  <b-field label="Request Type">
    <b-select placeholder="Pick a request type" :loading="isLoading">
      <option
        v-for="option in requestTypes"
        :value="option.value"
        :key="option.value">
        {{ option.key }}
      </option>
    </b-select>
  </b-field>
</template>

<script>
    import store from '../../store/index';

    export default {
        name: "request-type-dropdown",
        beforeCreate: function () {
            // https://github.com/perarnborg/vuex-oidc/wiki#6-optional-use-access-token-in-ajax-requests
            // console.log(store.state.oidcStore.access_token);

            let self = this;
            // Synchronously call for data
            this.$axios.get('/api/RequestType/All', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    // handle success
                    //console.log(response);

                    self.requestTypes = response.data.data.value;
                })
                .catch(function (error) {
                    // handle error
                    console.log(error);
                })
                .finally(function () {
                    // always executed
                    self.isLoading = false;
                });
        },
        data: function () {
            return {
                isLoading: true,
                requestTypes: [],
                currentRoute: window.location.href // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            }
        }
    }
</script>

<style scoped>

</style>
