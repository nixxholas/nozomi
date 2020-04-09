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
                                     :has-navigation="false"
                            >

                                <b-step-item label="Create">
                                    <RequestForm :request-methods="requestMethods"
                                                 :response-types="responseTypes"
                                                 :request-form="requestFormInput"
                                                 @onCreate="createRequest"
                                    />
                                </b-step-item>

                                <b-step-item label="Identify">

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
    import RequestTypeService from "../../services/RequestTypeService";
    import ResponseTypeService from "../../services/ResponseTypeService";
    import RequestPropertyTypeService from "../../services/RequestPropertyTypeService";

    import RequestForm from "./components/RequestForm";

    export default {
        components: {
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
                    url: null,
                    requestMethod: null,
                    responseType: null,
                    delay: 604800000,
                    failureDelay: 300000,
                    isEnabled: true,
                    properties: [],

                    // Defaults parentType to 'NONE' = -1
                    // as per required in backend
                    parentType: -1,
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