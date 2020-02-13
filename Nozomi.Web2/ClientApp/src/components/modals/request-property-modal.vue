<template>
    <div>
        <button v-if="guid"
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
                        <p class="modal-card-title" v-if="guid">Edit</p>
                        <p class="modal-card-title" v-else>Create a request property</p>
                    </header>
                    <section class="modal-card-body">
                        <b-input type="hidden" v-model="form.guid" />
                        <b-input type="hidden" v-model="form.requestGuid" />
                        
                        <b-field label="Type">
                            <b-select placeholder="Pick one!" v-model="form.type">
                                <option
                                        v-for="option in requestPropertyTypes"
                                        :value="option.key"
                                        :key="option.value">
                                    {{ option.value }}
                                </option>
                            </b-select>
                        </b-field>

                        <b-field>
                            <template slot="label">
                                Key
                            </template>
                            <b-input
                                    type="text"
                                    placeholder=""
                                    v-model="form.key"
                                    expanded>
                            </b-input>
                        </b-field>

                        <b-field>
                            <template slot="label">
                                Value
                            </template>
                            <b-input
                                    type="text"
                                    placeholder=""
                                    v-model="form.value"
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
    import RequestPropertyService from "../../services/RequestPropertyService";
    import RequestPropertyTypeService from "../../services/RequestPropertyTypeService";

    export default {
        name: "request-property-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            guid: String,
            requestGuid: String,
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            push: function () {
                this.isModalLoading = true;
                let self = this;

                if (!self.guid) {
                    RequestPropertyService.create(self.form)
                        .then(function (response) {
                            console.dir(response);
                            // Reset the form data regardless
                            self.form = {
                                type: 0,
                                key: "",
                                value: "",
                            };

                            if (response.status === 200) {
                                self.isModalActive = false; // Close the modal
                                Notification.open({
                                    duration: 2500,
                                    message: `Component successfully created!`,
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
                } else if (self.guid) {
                    AnalysedComponentService.update(self.form)
                    .then(function (response) {
                        if (response.status === 200) {
                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Component successfully updated!`,
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
                                message: `Please make sure your entries are correctly filled!`,
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                            });
                        })
                    .finally(function () {
                        self.isModalLoading = false;
                    })
                }
                else {
                    Notification.open({
                        duration: 2500,
                        message: `The modal was incorrectly instantiated! You might have to contact our staff :(.`,
                        position: 'is-bottom-right',
                        type: 'is-danger',
                        hasIcon: true
                    });
                    this.isModalLoading = false;
                }
            }
        },
        beforeCreate: function () {
            let self = this;

            // Synchronously call for data
            self.requestPropertyTypesIsLoading = true;
            RequestPropertyTypeService.all()
                .then(function (response) {
                    self.requestPropertyTypes = response;
                })
                .catch(function (error) {
                    //console.dir(error);
                    // handle error
                    self.authenticateOidc(window.location.href);
                })
                .finally(function () {
                    // always executed
                    self.requestPropertyTypesIsLoading = false;
                });
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                currentTypeTab: 0,
                form: {
                    guid: this.guid,
                    type: 0,
                    value: "",
                    key: "",
                    requestGuid: this.requestGuid,
                },
                requestPropertyTypes: [],
                requestPropertyTypesIsLoading: false
            }
        }
    }
</script>

<style scoped>

</style>
