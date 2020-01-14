<template>
    <div>
        <button class="button is-primary"
                @click="isActive = true">
            Create
        </button>

        <b-modal has-modal-card trap-focus :active.sync="isActive">
            <b-loading :active.sync="isModalLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form v-on:submit.prevent="create()" class="has-text-justified">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Link a source</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Source">
                            <b-select placeholder="Pick a source!" v-model="form.sourceGuid">
                                <option
                                        v-for="option in sources"
                                        :value="option.guid"
                                        :key="option.name">
                                    {{ option.name }}
                                </option>
                            </b-select>
                        </b-field>
                        
                        <b-field label="Currency">
                            <b-select placeholder="Pick a currency!" v-model="form.currencySlug">
                                <option
                                        v-for="option in currencies"
                                        :value="option.slug"
                                        :key="option.name">
                                    {{ option.name }}
                                </option>
                            </b-select>
                        </b-field>
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
    import store from '../../store/index';
    import {mapActions} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import SourceService from "../../services/SourceService";
    import CurrencyService from "@/services/CurrencyService";

    export default {
        name: "currency-source-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            defCurrencySlug: {
                type: String,
                default: null
            },
            defSourceGuid: {
                type: String,
                default: null
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            create: function () {
                this.isModalLoading = true;

                let self = this;
                this.$axios.post('/api/CurrencySource/Create', self.form, {
                    headers: {
                        Authorization: "Bearer " + store.state.oidcStore.access_token
                    }
                })
                    .then(function (response) {
                        // Reset the form data regardless
                        self.form = {
                            sourceGuid: null,
                            currencySlug: null
                        };

                        if (response.status === 200) {
                            self.isActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Source successfully created!`,
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
        },
        beforeCreate: function () {
            let self = this;

            SourceService.getAll()
                .then(function (res) {
                    self.sources = res;
                    self.isModalLoading = false;
                    
                    // TODO: Get this working again
                    // while (!self.currenciesAreLoading) {
                    //     self.isModalLoading = false;
                    // }

                    if (self.defSourceGuid) {
                        let sourceRes = self.sources
                            .filter(c => c.guid === self.defSourceGuid);

                        if (sourceRes && sourceRes.length > 0 && sourceRes[0]) {
                            self.form.sourceGuid = sourceRes[0].guid;
                        }
                    }
                });

            // Synchronously call for data
            CurrencyService.list()
                .then(function (res) {
                    self.currencies = res;
                    self.currenciesAreLoading = false;
                    
                    if (self.defCurrencySlug) {
                        let currencies = self.currencies
                            .filter(c => c.slug === self.defCurrencySlug);
                        
                        if (currencies && currencies.length > 0 && currencies[0]) {
                            self.form.currencySlug = currencies[0].slug;
                        }
                    }
                });
        },
        data: function () {
            return {
                isActive: false,
                isModalLoading: true,
                form: {
                    sourceGuid: null,
                    currencySlug: null
                },
                sources: [],
                currencies: [],
                currenciesAreLoading: true
            }
        }
    }
</script>

<style scoped>

</style>