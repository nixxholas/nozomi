<template>
    <div>
        <button v-if="!request"
                class="button is-primary"
                @click="isActive = true">
            Create
        </button>
        <button v-else
                class="button is-warning"
                @click="isActive = true">
            Edit
        </button>

        <b-modal has-modal-card trap-focus :active.sync="isActive">
            <b-loading :active.sync="isLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form v-on:submit.prevent="create">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title" v-if="!request">Create a request</p>
                        <p class="modal-card-title" v-else>Modify a request</p>
                    </header>
                    <section class="modal-card-body">
                        <RequestTypeDropdown v-model="form.type"/>
                        <ResponseTypeDropdown v-model="form.responseType"/>
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
                                <b-field>
                                    <b-select icon="money-bill" 
                                            placeholder="Pick a currency"
                                              v-if="currencies && currencies.length > 0"
                                              v-model="form.currencySlug">
                                        <option v-for="c in currencies" 
                                                :value="c.slug">{{ c.name }}</option>
                                    </b-select>
                                    <b-message v-else>Oh no.. There aren't any currencies at the moment...</b-message>
                                </b-field>
                            </b-tab-item>
                            <b-tab-item label="Currency Pair">
                                <b-field>
                                    <CurrencyPairsAutoComplete :incoming-currency-pair-guid="form.currencyPairGuid" @input="setCurrencyPairGuid" />
                                </b-field>
                            </b-tab-item>
                            <b-tab-item label="Currency Type">
                                <b-field>
                                    <b-select placeholder="Select a currency type"
                                              v-model="form.currencyTypeGuid"
                                              v-if="currencyTypes !== null && currencyTypes.length > 0">
                                        <option v-for="ct in currencyTypes" :value="ct.guid">{{ ct.name }}</option>
                                    </b-select>
                                    <b-message v-else>Oh no.. There aren't any currency types at the moment..
                                    </b-message>
                                </b-field>
                            </b-tab-item>
                        </b-tabs>
                    </section>

                    <footer class="modal-card-foot">
                        <button class="button" type="button" @click="isActive = false">Close</button>
                        <button class="button is-primary" type="submit">Submit</button>
                    </footer>
                </div>
            </form>
        </b-modal>
    </div>
</template>

<script>
    import {mapActions} from 'vuex';
    import RequestService from "@/services/RequestService";
    import CurrencyPairsAutoComplete from '../autocompletes/currency-pairs-autocomplete';
    import RequestTypeDropdown from '../dropdowns/request-type-dropdown';
    import ResponseTypeDropdown from "../dropdowns/response-type-dropdown";
    import {NotificationProgrammatic as Notification} from 'buefy';
    import CurrencyService from "@/services/CurrencyService";
    import CurrencyPairService from "@/services/CurrencyPairService";
    import CurrencyTypeService from "@/services/CurrencyTypeService";

    export default {
        name: "request-modal",
        components: { CurrencyPairsAutoComplete, ResponseTypeDropdown, 
            RequestTypeDropdown},
        props: {
            request: Object,
            currentRoute: window.location.href // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
        },
        watch: {
            request: function(newVal) { // watch it
                let self = this;

                if (newVal) {
                    self.form = newVal; // Set first
                }
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            setCurrencyPairGuid(val) {
                this.form.currencyPairGuid = val;
            },
            create: function () {
                this.isLoading = true;

                // Process the parent type first
                switch (this.form.parentType) {
                    case 0: // Currency
                        // Reset the rest just incase
                        this.form.currencyPairGuid = null;
                        this.form.currencyPairStr = null;
                        this.form.currencyTypeGuid = 0;
                        break;
                    case 1: // Currency Pair
                        // Reset the rest just incase
                        this.form.currencySlug = '';
                        this.form.currencyTypeGuid = 0;
                        break;
                    case 2: // Currency Type
                        // Reset the rest just incase
                        this.form.currencySlug = '';
                        this.form.currencyPairGuid = null;
                        this.form.currencyPairStr = null;
                        break;
                }

                let self = this;
                
                if (this.request) {
                    RequestService.update(self.form)
                        .then(function (response) {
                            if (response.status === 200) {
                                self.isActive = false; // Close the modal
                                Notification.open({
                                    duration: 2500,
                                    message: `Request successfully updated!`,
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
                                message: `Please make sure your entries are correctly filled!`,
                                position: 'is-bottom-right',
                                type: 'is-warning',
                                hasIcon: true
                            });
                        })
                        .finally(function () {
                            // always executed
                            self.isLoading = false;
                        });
                } else {
                    RequestService.create(self.form)
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
                                currencyPairGuid: null,
                                currencyPairStr: null,
                                currencyTypeGuid: 0
                            };

                            if (response.status === 200) {
                                self.isActive = false; // Close the modal
                                Notification.open({
                                    duration: 2500,
                                    message: `Request successfully created!`,
                                    position: 'is-bottom-right',
                                    type: 'is-success',
                                    hasIcon: true
                                });

                                // Inform the parent that a new request has been created
                                // https://forum.vuejs.org/t/passing-data-back-to-parent/1201
                                self.$emit('created', true);
                            }
                        })
                        .catch(function (error) {
                            //console.log(error);
                            Notification.open({
                                duration: 2500,
                                message: `Please make sure your entry is correctly filled!`,
                                position: 'is-bottom-right',
                                type: 'is-warning',
                                hasIcon: true
                            });
                        })
                        .finally(function () {
                            // always executed
                            self.isLoading = false;
                        });
                }
            }
        },
        beforeCreate: function () {
            let self = this;

            // Synchronously call for data
            CurrencyService.list()
                .then(function (response) {
                    self.currencies = response;
                    
                    // if (response && response.length > 0) {
                    //     let firstEl = response[0];
                    //    
                    //     if (firstEl && firstEl.currencySlug)
                    //         this.form.currencySlug = firstEl.currencySlug;
                    // }
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
            self.currencyTypesIsLoading = true;
            CurrencyTypeService.listAll()
                .then(function (response) {
                    self.currencyTypes = response;
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
        mounted: function() {
            // If its a modification, add the data in
            if (this.request) {
                if (this.request.guid && this.form.guid !== this.request.guid)
                    this.form.guid = this.request.guid;
                
                if (this.request.type && this.form.type !== this.request.type)
                    this.form.type = this.request.type;

                if (this.request.responseType && this.form.responseType !== this.request.responseType)
                    this.form.responseType = this.request.responseType;
                
                if (this.request.dataPath && this.form.dataPath !== this.request.dataPath)
                    this.form.dataPath = this.request.dataPath;
                
                if (this.request.delay && this.request.delay > 0 && this.form.delay !== this.request.delay)
                    this.form.delay = this.request.delay;

                if (this.request.failureDelay && this.request.failureDelay > 0 && this.form.failureDelay !== this.request.failureDelay)
                    this.form.failureDelay = this.request.failureDelay;

                if (this.request.currencySlug && this.form.currencySlug !== this.request.currencySlug) {
                    this.form.parentType = 0;
                    this.form.currencySlug = this.request.currencySlug;
                }
                
                if (this.request.currencyPairGuid && this.form.currencyPairGuid !== this.request.currencyPairGuid) {
                    this.form.parentType = 1;
                    this.form.currencyPairGuid = this.request.currencyPairGuid;
                    this.form.currencyPairStr = "Unchanged";
                }

                if (this.request.currencyTypeGuid && this.form.currencyTypeGuid !== this.request.currencyTypeGuid) {
                    this.form.parentType = 2;
                    this.form.currencyTypeGuid = this.request.currencyTypeGuid;
                }
            }
        },
        data: function () {
            return {
                isActive: false,
                isLoading: false,
                form: {
                    guid: null,
                    type: 0,
                    responseType: 1,
                    dataPath: "",
                    delay: 0,
                    failureDelay: 0,
                    parentType: 0,
                    currency: {},
                    currencySlug: '',
                    currencyPairGuid: null,
                    currencyPairStr: null,
                    currencyTypeGuid: 0
                },
                formHelper: {},
                currencies: [],
                currencyTypes: [],
                currencyTypesIsLoading: false
            }
        }
    }
</script>

<style scoped>

</style>
