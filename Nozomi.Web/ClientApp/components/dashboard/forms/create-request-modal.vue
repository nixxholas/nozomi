<template>
  <div>
    <button class="button is-primary"
            @click="isCreateRequestModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isCreateRequestModalActive">
      <b-loading :active.sync="isModalLoading" :can-cancel="false"></b-loading>
      <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
        <form v-on:submit.prevent="create()">
          <div class="modal-card">
            <header class="modal-card-head">
              <p class="modal-card-title">Create a request</p>
            </header>
            <section class="modal-card-body">
              <RequestTypeDrowdown v-model="form.type"></RequestTypeDrowdown>
              <ResponseTypeDropdown v-model="form.responseType"></ResponseTypeDropdown>
              <b-field label="URL">
                <b-input
                  type="url"
                  placeholder="https://nozomi.one/api/Ticker/GetAllAsync/0"
                  v-model="form.dataPath"
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
                  v-model="form.delay"
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
                  v-model="form.failureDelay"
                  required
                  expanded>
                </b-input>
              </b-field>

              <b-tabs v-model="form.parentType" expanded class="has-text-dark">
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
                      v-model="form.currencyPairStr"
                      placeholder="e.g. EURUSD"
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
                    <b-select placeholder="Select a currency type" v-model="form.currencyTypeId">
                      <option v-for="ct in currencyTypes" :value="ct.id">{{ ct.name }}</option>
                    </b-select>
                  </b-field>
                </b-tab-item>
              </b-tabs>
            </section>

            <footer class="modal-card-foot">
              <button class="button" type="button" @click="isCreateRequestModalActive = false">Close</button>
              <button class="button is-primary" type="submit">Submit</button>
            </footer>
          </div>
        </form>
    </b-modal>
  </div>
</template>

<script>
    import store from '../../../store/index';
    import { mapActions } from 'vuex';
    import RequestTypeDrowdown from '../../elements/request-type-dropdown';
    import ResponseTypeDropdown from "../../elements/response-type-dropdown";
    import { NotificationProgrammatic as Notification } from 'buefy';

    export default {
        name: "create-request-modal",
        components: {ResponseTypeDropdown, RequestTypeDrowdown},
        props: {
            currentRoute: window.location.href // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            getCurrencyPairTickerPairStr: function(obj) {
                return obj.mainTicker + obj.counterTicker + " (" + obj.sourceName + ")";
            },
            create: function() {
                this.isModalLoading = true;

                // Process the parent type first
                switch (this.form.parentType) {
                    case 0: // Currency
                        // Reset the rest just incase
                        this.form.currencyPair = null;
                        this.form.currencyPairStr = null;
                        this.form.currencyTypeId = 0;
                        break;
                    case 1: // Currency Pair
                        // Reset the rest just incase
                        this.form.currencySlug = '';
                        this.form.currencyTypeId = 0;
                        break;
                    case 2: // Currency Type
                        // Reset the rest just incase
                        console.dir("resetting non-currency type variables");
                        console.dir(this.form.currencyTypeId);
                        this.form.currencySlug = '';
                        this.form.currencyPair = null;
                        this.form.currencyPairStr = null;
                        break;
                }

                let self = this;
                this.$axios.post('/api/Request/Create', self.form, {
                    headers: {
                        Authorization: "Bearer " + store.state.oidcStore.access_token
                    }
                })
                    .then(function (response) {
                        // Reset the form data regardless
                        self.form = {
                            type: 0,
                            responseType: 1,
                            dataPath: "",
                            delay: 0,
                            failureDelay: 0,
                            parentType: 0,
                            currencySlug: '',
                            currencyPair: null,
                            currencyPairStr: null,
                            currencyTypeId: 0
                        };

                        if (response.status === 200) {
                            self.isCreateRequestModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Request successfully created!`,
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });
                        }
                    })
                    .catch(function (error) {
                        //console.log(error);
                        Notification.open({
                            duration: 2500,
                            message: `Please make sure your entry is correctly filled!`,
                            position: 'is-bottom-right',
                            type: 'is-success',
                            hasIcon: true
                        });
                    })
                    .finally(function () {
                    // always executed
                    self.isModalLoading = false;
                });
            }
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
                    self.methods.authenticateOidc(self.currentRoute);
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
                    self.methods.authenticateOidc(self.currentRoute);
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
                isModalLoading: false,
                form: {
                    type: 0,
                    responseType: 1,
                    dataPath: "",
                    delay: 0,
                    failureDelay: 0,
                    parentType: 0,
                    currencySlug: '',
                    currencyPair: null,
                    currencyPairStr: null,
                    currencyTypeId: 0
                },
                formHelper: {},
                currencies: [],
                currencyPairs: [],
                currencyPairsIsLoading: false,
                currencyTypes: [],
                currencyTypesIsLoading: false
            }
        }
    }
</script>

<style scoped>

</style>
