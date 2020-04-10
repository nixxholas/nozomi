<template>
    <div class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title has-text-centered">Create a request</h1>

                <div class="columns is-centered">
                    <div class="column is-8">
                        <div class="box">

                            <b-steps v-model="activeStep"
                                     ref="steps"
                                     :animated="true"
                                     :has-navigation="false">

                                <b-step-item label="Create">
                                    <RequestForm :request-methods="requestMethods"
                                                 :response-types="responseTypes"
                                                 :request-form="requestFormInput"
                                                 @onCreate="createRequest"
                                                 @onDisable="canProceed = false"
                                                 @onEnable="canProceed = true"
                                    />
                                </b-step-item>

                                <b-step-item label="Identify">
                                    <ComponentIdentificationForm/>
                                    <b-loading :is-full-page="false"
                                               :active.sync="isLoading"
                                               :can-cancel="false">
                                        <b-icon
                                                pack="fas"
                                                icon="spinner"
                                                size="is-large">
                                        </b-icon>
                                    </b-loading>
                                </b-step-item>

                                <b-step-item label="Finish">

                                </b-step-item>

                                <template
                                        :v-if="true"
                                        slot="navigation"
                                        slot-scope="{previous, next}">
                                    <div class="buttons">
                                        <b-button
                                                outlined
                                                icon-pack="fas"
                                                icon-left="chevron-left"
                                                :disabled="previous.disabled || !canBacktrack"
                                                @click.prevent="previous.action">
                                            Previous
                                        </b-button>
                                        <b-button
                                                outlined
                                                icon-pack="fas"
                                                icon-right="chevron-right"
                                                :disabled="next.disabled || !canProceed"
                                                @click.prevent="next.action">
                                            Next
                                        </b-button>
                                    </div>
                                </template>
                            </b-steps>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import ComponentIdentificationForm from "./components/ComponentIdentificationForm";
    import DispatchService from "../../services/DispatchService";
    import RequestTypeService from "../../services/RequestTypeService";
    import ResponseTypeService from "../../services/ResponseTypeService";
    import RequestPropertyTypeService from "../../services/RequestPropertyTypeService";

    import RequestForm from "./components/RequestForm";

    export default {
        components: {
            ComponentIdentificationForm,
            RequestForm,
        },
        data() {
            return {
                canBacktrack: false,
                canProceed: false,
                activeStep: 0,
                isLoading: false,
                requestMethods: [],
                responseTypes: [],
                requestPropertyTypes: [],

                requestFormInput: {
                    endpoint: null,
                    requestMethod: 0,
                    responseType: 1,
                    delay: 604800000,
                    failureDelay: 300000,
                    isEnabled: true,
                    properties: [],
                    websocketCommands: [],
                    socketKillSwitchDelay: 0,
                    socketDataCount: 0,

                    // Defaults parentType to 'NONE' = -1
                    // as per required in backend
                    parentType: -1,
                }
            }
        },

        watch: {
            activeStep(newVal, oldVal) {
                if (newVal === 1 && oldVal === 0) { // When the user is about to obtain the payload
                    let self = this;
                    self.isLoading = true;

                    // console.dir(self.requestFormInput);
                    // console.dir(JSON.stringify(self.requestFormInput));
                    DispatchService.fetch(self.requestFormInput)
                        .then(function (res) {
                            console.dir(res);
                        })
                        .catch(function (err) {
                            console.dir(err);
                            self.canProceed = false;
                            self.canBacktrack = true;
                        })
                        .finally(() => {
                            self.isLoading = false;
                        });
                }
            }
        },

        mounted() {
            this.isLoading = true;

            Promise.all([
                RequestTypeService.all(),
                ResponseTypeService.all(),
                RequestPropertyTypeService.all(),
            ])
                .then(([
                           requestMethods,
                           responseTypes,
                           requestPropertyTypes
                       ]) => {
                    this.requestMethods = requestMethods.data.value || [];
                    this.responseTypes = responseTypes.data.value || [];
                    this.requestPropertyTypes = requestPropertyTypes.data || [];
                })
                .catch((error) => {
                    console.error(error);
                })
                .finally(() => {
                    this.isLoading = false;
                    this.setDefaultRequestForm();
                });
        },

        methods: {
            setDefaultRequestForm() {
                if (this.requestMethods.length > 0) {
                    this.requestFormInput.requestMethod = this.requestMethods[0].value;
                }

                if (this.responseTypes.length > 0) {
                    this.requestFormInput.responseType = this.responseTypes[0].value;
                }
            },

            createRequest() {
                // TODO: Submit request form here

                this.activeStep++;
            }
        }
    }
</script>