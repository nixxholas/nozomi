<template>
  <b-field label="Request Type" v-if="requestTypes.length > 0">
    <b-select placeholder="Pick a request type" :loading="isLoading"
              v-model="computedValue" required>
      <option
        v-for="option in requestTypes"
        :value="option.value"
        :key="option.value"
        @click="computedValue(option.value)">
        {{ option.key }}
      </option>
    </b-select>
  </b-field>
</template>

<script>
    import store from '../../store/index';

    export default {
        name: "request-type-dropdown",
        computed: {
            // This is required to get v-model working at the component level.
            // Sources:
            // https://github.com/buefy/buefy/blob/dev/src/components/select/Select.vue
            // https://scotch.io/tutorials/add-v-model-support-to-custom-vuejs-component
            computedValue: {
                get() {
                    return this.value
                },
                set(val) {
                    this.$emit('input', val)
                }
            }
        },
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
                currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            }
        },
        props: {
            // General property for data binding.
            value: null,
        }
    }
</script>

<style scoped>

</style>
