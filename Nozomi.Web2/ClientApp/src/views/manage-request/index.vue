<template>
    <div class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title has-text-centered" v-if="!guid">Create a request</h1>
                <h1 class="title has-text-centered" v-if="guid">Update a request</h1>
{{guid}}
                <div class="columns is-centered">
                    <div class="column is-8">
                        <div class="box">
                            <b-button v-if="!finishResult.canProceed"
                                      icon-left="chevron-left" class="has-text-black mb-4"
                                      tag="router-link" to="/dashboard"
                                      type="is-white">Ditch this</b-button>

                            <b-steps v-model="activeStep"
                                     :animated="true"
                                     :has-navigation="false">

                                <b-step-item label="Create">
                                    <RequestForm :request-types="requestTypes"
                                                 :response-types="responseTypes"
                                                 :request-form="requestFormInput"
                                                 :request-property-types="requestPropertyTypes"
                                                 @dispatchRequest="dispatchRequest"
                                    />
                                </b-step-item>

                                <b-step-item label="Identify">
                                    <ComponentIdentificationForm
                                            :dispatch-payload="dispatchResult"
                                            @setActiveStep="setActiveStep"
                                            @setIdentifiedSelections="setIdentifiedSelections"
                                    />

                                    <b-loading :is-full-page="false" :active.sync="isLoading"
                                               :can-cancel="false"></b-loading>
                                </b-step-item>

                                <b-step-item label="Finish">
                                    <b-loading :is-full-page="false" :active.sync="isPushLoading"
                                               :can-cancel="false" />
                                    
                                    <b-message v-if="finishResult.message" 
                                               :type="finishResult.type">{{ finishResult.message }}</b-message>
                                    
                                    <b-button v-if="finishResult.canProceed" 
                                              icon-left="chevron-left"
                                              tag="router-link" to="/dashboard"
                                              type="is-primary">Back to Cabin</b-button>
                                </b-step-item>

                            </b-steps>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import DispatchService from "../../services/DispatchService";
    import RequestTypeService from "../../services/RequestTypeService";
    import ResponseTypeService from "../../services/ResponseTypeService";
    import RequestPropertyTypeService from "../../services/RequestPropertyTypeService";

    import RequestForm from "@/components/forms/request-form";
    import ComponentIdentificationForm from "@/components/forms/component-identification-form"
    import RequestService from "../../services/RequestService";

    export default {
        props: {
            guid: {
                type: String,
                default: null,
            },
        },
        components: {
            ComponentIdentificationForm,
            RequestForm,
        },
        data() {
            return {
                activeStep: 0,
                isLoading: false,
                isPushLoading: true,
                requestTypes: [],
                responseTypes: [],
                requestPropertyTypes: [],

                requestFormInput: {
                    endpoint: null,
                    requestType: 0,
                    responseType: 1,
                    delay: 604800000,
                    failureDelay: 300000,
                    isEnabled: true,
                    properties: {
                        params: [],
                        headers: [],
                        body: [],
                        socket: []
                    },
                    websocketCommands: [],
                    socketKillSwitchDelay: 0,
                    socketDataCount: 0,

                    components: [], // Identified components

                    // Defaults parentType to 'NONE' = -1
                    // as per required in backend
                    parentType: -1,
                },

                dispatchResult: {
                    response: null,
                    payload: null
                },
                
                finishResult: {
                    type: "is-danger",
                    message: null,
                    canProceed: false
                }
            }
        },

        mounted() {
            this.isLoading = true;
            
            if (this.guid)
                this.requestFormInput.guid = this.guid;

            Promise.all([
                RequestTypeService.all(),
                ResponseTypeService.all(),
                RequestPropertyTypeService.all(),
            ])
                .then(([
                           requestTypes,
                           responseTypes,
                           requestPropertyTypes
                       ]) => {
                    this.requestTypes = requestTypes.data.value || [];
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

        watch: {
            activeStep: function (val) {
                if (val === 2) { // If we're at finish
                    this.finishResult.message = null;
                    
                    if (this.requestFormInput.guid) {
                        // TODO: Update API
                    } else {
                        this.toVmProperties(this.requestFormInput.properties); // Convert the properties first
                        
                        let self = this;
                        RequestService.create(this.requestFormInput)
                        .then(function (res) {
                            if (res && res.status && res.status === 200) {
                                self.finishResult.type = "is-success";
                                self.finishResult.message = res.data ? res.data : "Request successfully created!";
                                self.finishResult.canProceed = true;
                            } else {
                                self.finishResult.type = "is-warning";
                                self.finishResult.message = res.data ? res.data : "Something unexpected has occurred.. try again!"; 
                            }
                        })
                        .catch(function (err) {
                            if (err && err.response && err.response.data) {
                                self.finishResult.type = "is-danger";
                                self.finishResult.message = err.response.data;
                            }
                        })
                        .finally(() => {
                            this.isPushLoading = false;
                        });
                    }
                }
            }
        },
        
        methods: {
            // Converts the UI properties to backend-compatible properties
            toVmProperties(properties) {
                if (properties) { // Is this the UI format?
                    let compatProperties = [];

                    for (let key in properties) // https://stackoverflow.com/questions/14379274/how-to-iterate-over-a-javascript-object
                        for (let j = 0; j < properties[key].length; j++) {
                            if (properties[key][j] && properties[key][j].key.length > 0 
                                && properties[key][j].value.length > 0)
                                compatProperties.push(properties[key][j]);
                        }
                    
                    this.requestFormInput.properties = compatProperties;
                } else 
                    alert("Bad properties collection"); // TODO: Proper Error Output
            },
            
            setDefaultRequestForm() {
                if (this.requestTypes.length > 0) {
                    this.requestFormInput.requestType = this.requestTypes[0].value;
                }

                if (this.responseTypes.length > 0) {
                    this.requestFormInput.responseType = this.responseTypes[0].value;
                }
            },

            setActiveStep(step = 0) {
                if (step < 0) {
                    this.activeStep--;
                } else {
                    this.activeStep++;
                }
            },

            dispatchRequest(cb) {
                // Merge properties into 1 array before submit
                // Comply with backend props controller
                let newProperties = [];
                const formattedForm = {...this.requestFormInput};

                for (const propertyKey in formattedForm.properties) {
                    formattedForm.properties[propertyKey].forEach(row => {
                        if (row.key.trim().length !== 0 && row.value.trim().length !== 0)
                            newProperties.push(row);
                    });
                }
                formattedForm.properties = newProperties;

                DispatchService.fetch(formattedForm)
                    .then(res => {
                        this.dispatchResult = res.data;

                        // Convert string payload to JS object
                        this.dispatchResult.payload = JSON.parse(JSON.parse(res.data.payload));
                        cb(true);

                        this.setActiveStep();
                    })
                    .catch(err => {
                        cb(false);
                    });
            },

            setIdentifiedSelections(identifiedSelections) {
                this.requestFormInput.components = identifiedSelections;
                this.setActiveStep();
            }
        }
    }
</script>