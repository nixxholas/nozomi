<template>
    <div>
        <div class="buttons" v-if="guid">
            <b-button 
                    native-type="button"
                    type="is-warning"
                    @click="isModalActive = true">
                Modify
            </b-button>
            <b-button :loading="isDeleteLoading"
                      @click="remove()"
                      native-type="button"
                      type="is-danger">
                <b-icon size="is-small" icon="trash" />
            </b-button>
        </div>
        <b-navbar v-else-if="hasNavbar"
                :spaced="hasNavbar">
            <template slot="brand">
                <b class="has-text-dark">Commands properties</b>
            </template>
            <template slot="end">
                <b-button v-if="hasModalButton"
                          native-type="button"
                          type="is-primary"
                          @click="isModalActive = true">
                    Create
                </b-button>
            </template>
        </b-navbar>
        <b-button v-else-if="hasModalButton"
                  native-type="button"
                type="is-primary"
                @click="isModalActive = true">
            Create
        </b-button>

        <b-modal has-modal-card trap-focus :active.sync="isModalActive">
            <b-loading :active.sync="isModalLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form class="has-text-justified" style="z-index: 1;">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title" v-if="guid">Edit</p>
                        <p class="modal-card-title" v-else>Create a command property</p>
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
                                    v-model="form.value"
                                    expanded>
                            </b-input>
                        </b-field>
                        
                        <b-field label="Make it active?">
                            <b-switch v-model="form.isEnabled" />
                        </b-field>
                    </section>

                    <footer class="modal-card-foot">
                        <button class="button" native-type="button" type="button" @click="isModalActive = false">Close</button>
                        <b-button type="is-primary" native-type="button" @click="pushCommandPropertyForm()" :disabled="!isValid()">Submit</b-button>
                    </footer>
                </div>
            </form>
        </b-modal>
    </div>
</template>

<script>
    import {mapActions} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import WebsocketCommandPropertyService from "@/services/WebsocketCommandPropertyService";
    import WebsocketCommandPropertyTypeService from "@/services/WebsocketCommandPropertyTypeService";

    export default {
        name: "websocket-command-property-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            childMode: {
                type: Boolean,
                default: false
            },
            hasNavbar: {
                type: Boolean,
                default: false
            },
            hasModalButton: {
                type: Boolean,
                default: true
            },
            guid: {
                type: String,
                default: null,
            },
            commandGuid: {
                type: String,
                default: null,
            },
            commandId: {
                type: Number,
                default: 0,
            },
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            isValid() {
                return (this.form.type || this.form.type >= 0) && this.form.key && this.form.value && this.form.isEnabled;
            },
            remove: function() {
                let self = this;
                self.isDeleteLoading = true;

                WebsocketCommandPropertyService.delete(self.guid)
                .then(function(res) {
                    Notification.open({
                        duration: 2500,
                        message: res && res.data && res.data !== "" ? res.data : "Property successfully deleted!",
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
                        message: err && err.statusText ? err.statusText : "There was an issue deleting this property!",
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
            pushCommandPropertyForm: function () {
                this.isModalLoading = true;
                let self = this;
                
                if (self.commandGuid && !self.form.commandGuid)
                    self.form.commandGuid = self.commandGuid;

                if (self.commandId && !self.form.commandId)
                    self.form.commandId = self.commandId;
                
                if (self.childMode) { // If this modal has a parent command form,
                    // We need to do a thorough check here
                    if (self.isValid()) { // Since its valid,
                        self.$emit('added', self.form); // Push the form and close the modal
                        Notification.open({
                            duration: 2500,
                            message: `Property successfully added!`,
                            position: 'is-bottom-right',
                            type: 'is-success',
                            hasIcon: true
                        });
                        
                        // Reset the form data regardless
                        self.form = {
                            type: 0,
                            key: "",
                            value: "",
                            isEnabled: true,
                            commandGuid: self.commandGuid ? self.commandGuid : null,
                            commandId: self.commandId ? self.commandId : null,
                        };
                        
                        self.isModalActive = false;
                    } else {
                        Notification.open({
                            duration: 2500,
                            message: `Please make sure your entry is correctly filled!`,
                            position: 'is-bottom-right',
                            type: 'is-danger',
                            hasIcon: true
                        });
                    }
                    
                    self.isModalLoading = false;
                    
                    return; // Send off, don't continue.
                }

                if (!self.guid && (self.form.commandGuid || self.form.commandId)) {
                    WebsocketCommandPropertyService.create(self.form)
                        .then(function (response) {
                            // Reset the form data regardless
                            self.form = {
                                type: 0,
                                key: "",
                                value: "",
                                isEnabled: true,
                                commandGuid: self.commandGuid ? self.commandGuid : null,
                                commandId: self.commandId ? self.commandId : null,
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
                            self.$emit('created', self.form.commandId ? self.form.commandId : self.form.commandGuid);
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
                    WebsocketCommandPropertyService.update(self.form)
                    .then(function (response) {
                        self.isModalActive = false; // Close the modal
                        Notification.open({
                            duration: 2500,
                            message: `Property successfully updated!`,
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
            WebsocketCommandPropertyTypeService.all()
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
                WebsocketCommandPropertyService.get(self.guid)
                    .then(function(res) {
                        if (res && res.data) {
                            if (res.data.type)
                                self.form.type = res.data.type;
                            if (res.data.key)
                                self.form.key = res.data.key;
                            if (res.data.value)
                                self.form.value = res.data.value;
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
                    key: "",
                    value: "",
                    isEnabled: true,
                    commandId: this.commandId,
                    commandGuid: this.commandGuid,
                },
                types: [],
                typesIsLoading: true,
            }
        }
    }
</script>

<style scoped>

</style>
