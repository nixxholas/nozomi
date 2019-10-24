<template>
  <div>
    <button class="button is-primary"
            @click="isCreateRequestModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isCreateRequestModalActive">
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">Create a request</p>
        </header>
        <section class="modal-card-body">
          <form action="">
            <RequestTypeDrowdown v-model="form.type"></RequestTypeDrowdown>
            <ResponseTypeDropdown v-model="form.responseType"></ResponseTypeDropdown>
            <b-field label="URL">
              <b-input
                type="url"
                placeholder="https://nozomi.one/api/Ticker/GetAllAsync/0"
                v-bind="form.apiUrl"
                required
                expanded>
              </b-input>
            </b-field>
            <b-field>
              <template slot="label">
                Delay <span class="has-text-primary is-italic">(In milliseconds)</span>
              </template>
              <b-input
                type="number"
                placeholder="Delay between each update in milliseconds"
                v-bind="form.delay"
                expanded
                required>
              </b-input>
            </b-field>
            <b-field>
              <template slot="label">
                Retry Delay <span class="has-text-primary is-italic">(In milliseconds)</span>
              </template>
              <b-input
                type="number"
                placeholder="Retry attempt delay in milliseconds"
                v-bind="form.failureDelay"
                required
                expanded>
              </b-input>
            </b-field>

            <b-tabs v-model="form.requestParentType" expanded class="has-text-dark">
              <b-tab-item label="Currency">
                <b-field label="Find a currency">
                  <b-autocomplete
                    rounded
                    v-model="form.currencySlug"
                    :data="currencies"
                    placeholder="e.g. EUR"
                    icon="magnify"
                    @select="option => form.currencySlug = option">
                    <template slot="empty">No results found</template>
                  </b-autocomplete>
                </b-field>
              </b-tab-item>
              <b-tab-item label="Currency Pair">
                <b-field>
                  <b-autocomplete
                    :data="currencyPairs"
                    placeholder="e.g. EURUSD"
                    field="id"
                    :custom-formatter="getCurrencyPairTickerPairStr"
                    :loading="currencyPairsIsLoading"
                    @select="option => form.currencyPair = option">

                    <template slot-scope="props">
                      <b-taglist attached>
                        <b-tag type="is-dark">
                          {{ props.option.mainTicker }}{{ props.option.counterTicker }}
                        </b-tag>
                        <b-tag type="is-info">
                          <b>{{ props.option.sourceName }}</b>
                        </b-tag>
                      </b-taglist>
                    </template>
                  </b-autocomplete>
                </b-field>
              </b-tab-item>
              <b-tab-item label="Currency Type">
                <b-field v-if="currencyTypes !== null && currencyTypes.length > 0">
                  <b-select placeholder="Select a currency type" v-model="form.currencyType">
                    <option v-for="ct in currencyTypes" :value="ct.id">{{ ct.name }}</option>
                  </b-select>
                </b-field>
              </b-tab-item>
            </b-tabs>
          </form>
        </section>
        <footer class="modal-card-foot">
          <button class="button" type="button" @click="isCreateRequestModalActive = false">Close</button>
          <button class="button is-primary">Submit</button>
        </footer>
      </div>
    </b-modal>
  </div>
</template>

<script>
    import store from '../../../store/index';
    import RequestTypeDrowdown from '../../elements/request-type-dropdown';
    import ResponseTypeDropdown from "../../elements/response-type-dropdown";

    export default {
        name: "create-request-modal",
        components: {ResponseTypeDropdown, RequestTypeDrowdown},
        props: {
            // Nothing yet
        },
        beforeCreate: function () {
            let self = this;

            // Synchronously call for data
            this.$axios.get('/api/Currency/ListAll')
                .then(function (response) {
                    self.currencies = response.data.data;
                })
                .catch(function (error) {
                    // handle error
                    console.log(error);
                })
                .finally(function () {
                    // always executed
                    self.isLoading = false;
                });


            // Synchronously call for data
            self.currencyPairsIsLoading = true;
            this.$axios.get('/api/CurrencyPair/ListAll', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    self.currencyPairs = response.data;
                })
                .catch(function (error) {
                    // handle error
                    console.log(error);
                })
                .finally(function () {
                    // always executed
                    self.currencyPairsIsLoading = false;
                });

            // Synchronously call for data
            self.currencyTypesIsLoading = true;
            this.$axios.get('/api/CurrencyType/ListAll', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    self.currencyTypes = response.data;
                })
                .catch(function (error) {
                    // handle error
                    console.log(error);
                })
                .finally(function () {
                    // always executed
                    self.currencyTypesIsLoading = false;
                });
        },
        data: function () {
            return {
                isCreateRequestModalActive: false,
                form: {
                    activeTab: 0, // For requestParentType
                    type: 0,
                    responseType: 1,
                    apiUrl: '',
                    delay: 0,
                    failureDelay: 0,
                    requestParentType: 0,
                    currencySlug: '',
                    currencyPair: {},
                    currencyType: 0,
                    storeHistorical: false,
                    isDenominated: false,
                    anomalyIgnorance: false
                },
                formHelper: {},
                currencies: [],
                currencyPairs: [],
                currencyPairsIsLoading: false,
                currencyTypes: [],
                currencyTypesIsLoading: false,
            }
        },
        methods: {
            getCurrencyPairTickerPairStr: function(obj) {
                return obj.mainTicker + obj.counterTicker + " (" + obj.sourceName + ")";
            }
        }
    }
</script>

<style scoped>

</style>
