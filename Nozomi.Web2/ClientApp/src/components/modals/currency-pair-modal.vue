<template>
    <div>
        <button class="button is-primary"
                @click="isModalActive = true" v-if="currencyPair == null">
            Create
        </button>
        <button class="button is-warning"
                @click="isModalActive = true" v-else>
            Edit
        </button>

        <b-modal has-modal-card trap-focus :active.sync="isModalActive">
            <b-loading :active.sync="isModalLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form v-on:submit.prevent="create()" class="has-text-justified">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title" v-if="currencyPair == null">Create a currency pair</p>
                        <p class="modal-card-title" v-else>Modify a currency pair</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Type">
                            <b-select placeholder="Pick one!" v-model="form.type">
                                <option v-if="types && types.length > 0"
                                        v-for="option in types"
                                        :value="option.value"
                                        :key="option.key">
                                    {{ option.key }}
                                </option>
                            </b-select>
                        </b-field>

                        <b-field label="Main Ticker">
                            <b-select placeholder="Pick one!" v-model="form.mainTicker">
                                <option
                                        v-for="option in currencies"
                                        :value="option.slug"
                                        :key="option.name">
                                    {{ option.name }}
                                </option>
                            </b-select>
                        </b-field>

                        <b-field label="Counter Ticker">
                            <b-select placeholder="Pick one!" 
                                      v-model="form.counterTicker"
                                      :disabled="!form.mainTicker || form.mainTicker === ''">
                                <option
                                        v-for="option in currencies"
                                        v-if="option.slug !== form.mainTicker"
                                        :value="option.slug"
                                        :key="option.name">
                                    {{ option.name }}
                                </option>
                            </b-select>
                        </b-field>

                        <b-field label="API Url">
                            <b-input
                                    type="url"
                                    placeholder="https://www.api.somesource.com/getpriceforlmao"
                                    v-model="form.apiUrl"
                                    expanded>
                            </b-input>
                        </b-field>

                        <b-field label="Default Component">
                            <b-input
                                    type="text"
                                    placeholder=""
                                    v-model="form.defaultComponent">
                            </b-input>
                        </b-field>

                        <b-field label="Source">
                            <b-select placeholder="Pick a source" 
                                      v-model="form.sourceGuid">
                                <option
                                        v-for="option in sources"
                                        :value="option.guid"
                                        :key="option.name">
                                    {{ option.name }}
                                </option>
                            </b-select>
                        </b-field>
                        
                        <b-field label="Make it active?">
                            <b-switch v-model="form.isEnabled" />
                        </b-field>
                    </section>

                    <footer class="modal-card-foot">
                        <button class="button" type="button" @click="isModalActive = false">Close</button>
                        <button class="button is-primary" type="submit" :disabled="!types && types.length <= 0">Submit
                        </button>
                    </footer>
                </div>
            </form>
        </b-modal>
    </div>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import CurrencyService from "@/services/CurrencyService";
    import CurrencyPairService from "@/services/CurrencyPairService";
    import CurrencyPairTypeService from "@/services/CurrencyPairTypeService";
    import SourceService from "@/services/SourceService";

    export default {
        name: "currency-pair-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            currency: Object,
            currencyPair: Object
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcUser'
            ])
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                form: {
                    mainTicker: "",
                    counterTicker: "",
                    apiUrl: "",
                    defaultComponent: "",
                    sourceGuid: "",
                    type: "",
                    isEnabled: true
                },
                currencies: [],
                sources: [],
                types: [],
                typesIsLoading: false
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            create: function () {
                this.isModalLoading = true;

                let self = this;

                if (self.currencyPair) {
                    CurrencyPairService.edit(self.form)
                        .then(function (response) {
                            // TODO: Pull the latest currency data

                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Currency successfully updated!`,
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });

                            // Inform the parent that a new request has been created
                            // https://forum.vuejs.org/t/passing-data-back-to-parent/1201
                            self.$emit('created', true);
                        })
                        .catch(function (error) {
                            Notification.open({
                                duration: 2500,
                                message: `Please make sure your entry is correctly filled!`,
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                            });
                        })
                        .finally(function () {
                            // always executed
                            self.isModalLoading = false;
                        });
                } else {
                    CurrencyPairService.create(self.form)
                        .then(function (response) {
                            // Reset the form data regardless
                            self.form = {
                                mainTicker: "",
                                counterTicker: "",
                                apiUrl: "",
                                defaultComponent: "",
                                sourceGuid: "",
                                type: ""
                            };

                            if (response.status === 200) {
                                self.isModalActive = false; // Close the modal
                                Notification.open({
                                    duration: 2500,
                                    message: `Currency successfully created!`,
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
                                type: 'is-danger',
                                hasIcon: true
                            });
                        })
                        .finally(function () {
                            // always executed
                            self.isModalLoading = false;
                        });
                }
            }
        },
        beforeCreate: function () {
            let self = this;

            // Synchronously call for data
            CurrencyService.listAll()
                .then(function (response) {
                    self.currencies = response;
                })
                .catch(function (error) {
                    // handle error
                    self.methods.authenticateOidc(self.currentRoute);
                });

            SourceService.getAll()
                .then(function (response) {
                    self.sources = response;
                })
                .catch(function (error) {
                    // handle error
                    self.methods.authenticateOidc(self.currentRoute);
                });
        },
        mounted: function () {
            let self = this;

            // Synchronously call for data
            self.typesIsLoading = true;
            CurrencyPairTypeService.all()
                .then(function (res) {
                    self.types = res;
                    self.typesIsLoading = false;

                    // If currency isn't null, it means we're editing an existing one.
                    if (self.currency) {
                        self.form = self.currency; // Set first

                        // Update the currency pair type
                        // if (self.types && self.currency.currencyTypeGuid
                        //     && self.types.filter(t => t.guid === self.currency.currencyTypeGuid).length > 0) {
                        //     self.form.sourceType = self.types.filter(t => t.guid === self.currency.currencyTypeGuid)[0].guid;
                        // }
                    }
                });
        }
    }
</script>

<style scoped>

</style>
