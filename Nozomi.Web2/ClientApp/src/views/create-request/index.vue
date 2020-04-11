<template>
    <div class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title has-text-centered">Create a request</h1>

                <div class="columns is-centered">
                    <div class="column is-8">
                        <div class="box">

                            <b-steps v-model="activeStep"
                                     :animated="true"
                                     :has-navigation="false">

                                <b-step-item label="Create">
                                    <RequestForm :request-methods="requestMethods"
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
                                            @setIdentifiedSelection="setIdentifiedSelection"
                                    />
                                    
                                    <b-loading :is-full-page="false" :active.sync="isLoading"
                                               :can-cancel="false"></b-loading>
                                </b-step-item>

                                <b-step-item label="Finish">

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

    export default {
        components: {
            ComponentIdentificationForm,
            RequestForm,
        },
        data() {
            return {
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
                    properties: {
                        params: [],
                        headers: [],
                        body: [],
                        socket: []
                    },
                    websocketCommands: [],
                    socketKillSwitchDelay: 0,
                    socketDataCount: 0,

                    // Defaults parentType to 'NONE' = -1
                    // as per required in backend
                    parentType: -1,
                },

                dispatchResult: {
                    response: null,
                    payload: null
                },
                
                identifySelectionForm: []
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
                const formattedForm = { ...this.requestFormInput };
                
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
                        cb(true);
                        
                        this.setActiveStep();
                    })
                    .catch(err => {
                        cb(false);
                    });
            },
            
            setIdentifiedSelection(identifiedSelection) {
                console.log(identifiedSelection);
                this.setActiveStep();
            }
        }
    }
</script>