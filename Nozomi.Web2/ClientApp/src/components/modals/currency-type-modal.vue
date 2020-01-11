<template>
    <div>
        <button v-if="currencyType != null"
                class="button is-warning"
                @click="isModalActive = true">
            Modify
        </button>
        <button v-else
                class="button is-primary"
                @click="isModalActive = true">
            Create
        </button>

        <b-modal has-modal-card trap-focus :active.sync="isModalActive">
            <b-loading :active.sync="isModalLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form v-on:submit.prevent="push()" class="has-text-justified">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title" v-if="currencyType && form && form.name">
                            Modify {{ form.name }}
                        </p>
                        <p class="modal-card-title" v-else>Create a currency type</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field v-if="currencyType !== null">
                            <b-input
                                    type="hidden"
                                    v-model="form.guid">
                            </b-input>
                        </b-field>

                        <b-field>
                            <template slot="label">
                                Abbreviation
                            </template>
                            <b-input
                                    type="text"
                                    placeholder=""
                                    v-model="form.typeShortForm"
                                    expanded>
                            </b-input>
                        </b-field>

                        <b-field>
                            <template slot="label">
                                Name
                            </template>
                            <b-input
                                    type="text"
                                    placeholder=""
                                    v-model="form.name"
                                    expanded>
                            </b-input>
                        </b-field>
                    </section>

                    <footer class="modal-card-foot">
                        <button class="button" type="button" @click="isModalActive = false">Close</button>
                        <button class="button is-primary" type="submit">Submit</button>
                    </footer>
                </div>
            </form>
        </b-modal>
    </div>
</template>

<script>
    import {mapActions} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import CurrencyTypeService from "../../services/CurrencyTypeService";

    export default {
        name: "currency-type-modal",
        props: {
            currencyType: {
                type: Object,
                default: null
            },
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
        },
        beforeMount: function () {
            if (this.currencyType) {
                let sType = this.currencyType;

                if (sType.guid)
                    this.form.guid = sType.guid;

                if (sType.typeShortForm)
                    this.form.typeShortForm = sType.typeShortForm;

                if (sType.name)
                    this.form.name = sType.name;
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            push: function () {
                let self = this;
                self.isModalLoading = true;

                if (self.currencyType) {
                    // Update
                    CurrencyTypeService.update(self.form)
                        .then(function (response) {
                            if (response.status === 200) {
                                self.isModalActive = false; // Close the modal
                                Notification.open({
                                    duration: 2500,
                                    message: self.form.name + ` successfully updated!`,
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
                            if (error && error.response) {
                                switch (error.response.status) {
                                    case 401:
                                    case 403:
                                        Notification.open({
                                            duration: 2500,
                                            message: `You do not have the permissions for this!`,
                                            position: 'is-bottom-right',
                                            type: 'is-danger',
                                            hasIcon: true
                                        });
                                        break;
                                    case 408:
                                        Notification.open({
                                            duration: 2500,
                                            message: `Oh no.. something is not right with our connection to the server!`,
                                            position: 'is-bottom-right',
                                            type: 'is-danger',
                                            hasIcon: true
                                        });
                                        break;
                                }
                            } else {
                                Notification.open({
                                    duration: 2500,
                                    message: `Please make sure your entry is correctly filled!`,
                                    position: 'is-bottom-right',
                                    type: 'is-danger',
                                    hasIcon: true
                                });
                            }
                        })
                        .finally(function () {
                            // always executed
                            self.isModalLoading = false;
                        });
                } else {
                    // Create
                    CurrencyTypeService.create(self.form)
                        .then(function (response) {
                            // Reset the form data regardless
                            self.form = {
                                guid: "",
                                typeShortForm: "",
                                name: "",
                                deletedAt: null,
                                isEnabled: null,
                            };

                            if (response.status === 200) {
                                self.isModalActive = false; // Close the modal
                                Notification.open({
                                    duration: 2500,
                                    message: `Currency type successfully created!`,
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
                            if (error && error.response) {
                                switch (error.response.status) {
                                    case 401:
                                    case 403:
                                        Notification.open({
                                            duration: 2500,
                                            message: `You do not have the permissions for this!`,
                                            position: 'is-bottom-right',
                                            type: 'is-danger',
                                            hasIcon: true
                                        });
                                        break;
                                    case 408:
                                        Notification.open({
                                            duration: 2500,
                                            message: `Oh no.. something is not right with our connection to the server!`,
                                            position: 'is-bottom-right',
                                            type: 'is-danger',
                                            hasIcon: true
                                        });
                                        break;
                                }
                            } else {
                                Notification.open({
                                    duration: 2500,
                                    message: `Please make sure your entry is correctly filled!`,
                                    position: 'is-bottom-right',
                                    type: 'is-danger',
                                    hasIcon: true
                                });
                            }
                        })
                        .finally(function () {
                            // always executed
                            self.isModalLoading = false;
                        });
                }
            }
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                form: {
                    guid: "",
                    typeShortForm: "",
                    name: "",
                    deletedAt: null,
                    isEnabled: null,
                }
            }
        }
    }
</script>

<style scoped>

</style>
