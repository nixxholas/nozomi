<template>
    <div>
        <div class="buttons" v-if="guid">
            <button class="button is-warning"
                    @click="isModalActive = true">
                Modify
            </button>
            <b-button :loading="isDeleteLoading" @click="remove()" type="is-danger">
                <b-icon size="is-small" icon="trash" />
            </b-button>
        </div>
        <button v-else
                class="button is-primary"
                @click="isModalActive = true">
            Create
        </button>

        <b-modal has-modal-card trap-focus :active.sync="isModalActive">
            <b-loading :active.sync="isModalLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form class="has-text-justified" style="z-index: 1;">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title" v-if="guid">Edit</p>
                        <p class="modal-card-title" v-else>Create a command</p>
                    </header>
                    <section class="modal-card-body">
                        <b-input type="hidden" v-model="form.guid" />
                        
                        <b-field label="Type">
                            <b-select placeholder="Pick one!" v-model="form.type"
                                      :loading="typesIsLoading">
                                <option
                                        v-for="option in types"
                                        :value="option.value"
                                        :key="option.value">
                                    {{ option.key }}
                                </option>
                            </b-select>
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

                        <b-field>
                            <template slot="label">
                                Delay
                            </template>
                            <b-input
                                    type="number"
                                    v-model="form.delay"
                                    expanded>
                            </b-input>
                        </b-field>

                        <b-field label="Make it active?">
                            <b-switch v-model="form.isEnabled" />
                        </b-field>
                        
                        <b-field>
                            <template slot="Label">
                                Properties
                            </template>

                            <WebsocketCommandPropertiesTable @pushed="addFreshCommandProperty"
                                    :is-section="false"
                                    :show-create-feature="true" />
                        </b-field>
                    </section>

                    <footer class="modal-card-foot">
                        <button class="button" type="button" @click="isModalActive = false">Close</button>
                        <b-button type="is-primary" native-type="button" @click="push" :disabled="!requestGuid">Submit</b-button>
                    </footer>
                </div>
            </form>
        </b-modal>
    </div>
</template>

<script>
    import {mapActions} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import WebsocketCommandTypeService from "../../services/WebsocketCommandTypeService";
    import WebsocketCommandPropertiesTable from "@/components/tables/websocket-command-properties-table";
    import WebsocketCommandService from "@/services/WebsocketCommandService";

    export default {
        name: "websocket-command-modal",
        components: {WebsocketCommandPropertiesTable},
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            guid: {
                type: String,
                default: null,
            },
            requestGuid: {
                type: String,
                default: null
            },
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            addFreshCommandProperty: function(prop) {
                if (prop) {
                    if (!this.form.properties)
                        this.form.properties = [];
                    
                    this.form.properties.push(prop);
                }
            },
            remove: function() {
                let self = this;
                self.isDeleteLoading = true;
                
                WebsocketCommandService.delete(self.guid)
                .then(function(res) {
                    Notification.open({
                        duration: 2500,
                        message: res && res.data && res.data !== "" ? res.data : "Command successfully deleted!",
                        position: 'is-bottom-right',
                        type: 'is-success',
                        hasIcon: true
                    });

                    self.$emit('deleted', true);
                })
                .catch(function(err) {
                    console.dir(err);
                    Notification.open({
                        duration: 2500,
                        message: err && err.statusText ? err.statusText : "There was an issue deleting this command!",
                        position: 'is-bottom-right',
                        type: 'is-danger',
                        hasIcon: true
                    });

                    self.$emit('updated', true);
                })
                .finally(function() {
                    self.isDeleteLoading = false;
                });
            },
            push: function () {
                this.isModalLoading = true;
                let self = this;
                
                if (!self.form.requestGuid)
                    self.form.requestGuid = self.requestGuid;

                if (!self.guid && self.form.requestGuid) {
                    console.dir(self.form);
                    WebsocketCommandService.create(JSON.stringify(self.form))
                        .then(function (response) {
                            // Reset the form data regardless
                            self.form = {
                                type: 0,
                                name: "",
                                delay: 0,
                                isEnabled: false,
                                properties: [],
                                requestGuid: self.requestGuid,
                            };

                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Property successfully added!`,
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });

                            // Inform the parent that a new request has been created
                            // https://forum.vuejs.org/t/passing-data-back-to-parent/1201
                            self.$emit('created', true);
                        })
                        .catch(function (error) {
                            console.log(error);
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
                    if (self.form.properties && self.form.properties.length > 0)
                        self.form.properties = []; // Don't permit the user to update properties just like that
                    
                    WebsocketCommandService.update(self.form)
                    .then(function (response) {
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
                        self.$emit('updated', true);
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
            WebsocketCommandTypeService.all()
                .then(function (response) {
                    self.types = response.data;
                })
                .catch(function (error) {
                    //console.dir(error);
                    // handle error
                    self.authenticateOidc(window.location.href);
                })
                .finally(function () {
                    // always executed
                    self.typesIsLoading = false;
                });
        },
        created: function() {
            let self = this;
            
            if (self.guid) {
                WebsocketCommandService.get(self.guid)
                    .then(function(res) {
                        if (res && res.data) {
                            if (res.data.type)
                                self.form.type = res.data.type;
                            if (res.data.name)
                                self.form.key = res.data.name;
                            if (res.data.delay)
                                self.form.delay = res.data.delay;
                            if (res.data.isEnabled)
                                self.form.isEnabled = res.data.isEnabled;
                        }
                    });
            }
            
            self.isModalLoading = false;
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: true,
                isDeleteLoading: false,
                currentTypeTab: 0,
                form: {
                    guid: this.guid,
                    type: 0,
                    name: "",
                    delay: 0,
                    isEnabled: true,
                    properties: [],
                    requestGuid: this.requestGuid,
                },
                types: [],
                typesIsLoading: true,
            }
        }
    }
</script>

<style scoped>

</style>
