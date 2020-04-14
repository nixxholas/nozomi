<template>
    <section>
        <h1 class="title pt-5">Specify an endpoint</h1>
        <p class="subtitle">
            Where should we poll your information from?
        </p>

        <!-- Request URL & Protocol selection -->
        <b-field grouped>
            <b-field label="Protocol" label-position="inside">
                <b-select placeholder="Select a protocol"
                          v-model="requestForm.requestType"
                          size="is-medium">
                    <option v-for="requestType in requestTypes"
                            :key="requestType.key"
                            :value="requestType.value">
                        {{ requestType.key }}
                    </option>
                </b-select>
            </b-field>

            <b-field label="Enter a request URL" label-position="inside" expanded>
                <b-input v-model="requestForm.endpoint" type="url" size="is-medium"
                         placeholder="https://jsonplaceholder.typicode.com/users"/>
            </b-field>
        </b-field>

        <!-- Response type -->
        <b-field label-position="inside">
            <template slot="label">
                <p>
                    <span>Response type</span>
                    <b-tooltip type="is-dark"
                               label="What kind of data format are we polling for you?"
                               class="is-valigned"
                               multilined
                    >
                        <b-icon size="is-small" icon="question-circle"></b-icon>
                    </b-tooltip>
                </p>
            </template>

            <b-select placeholder="Select a response"
                      v-model="requestForm.responseType"
                      size="is-medium"
                      expanded
            >
                <option v-for="responseType in responseTypes"
                        :key="responseType.key"
                        :value="responseType.value"
                >
                    {{ responseType.key }}
                </option>
            </b-select>
        </b-field>

        <!-- Request Properties -->
        <div class="columns is-desktop pt-5">
            <div class="column is-4-desktop">
                <h1 class="subtitle">Properties</h1>
            </div>
        </div>

        <b-tabs type="is-boxed">
            <b-tab-item label="Params">
                <RequestFormPropertiesTable
                        :header-types="paramPropertyTypes"
                        :form-data="requestForm.properties.params"
                />
            </b-tab-item>

            <b-tab-item label="Headers">
                <RequestFormPropertiesTable
                        :header-types="headerPropertyTypes"
                        :form-data="requestForm.properties.headers"
                />
            </b-tab-item>

            <b-tab-item label="Body" :visible="requestForm.requestType !== 50">
                <RequestFormPropertiesTable
                        :header-types="bodyPropertyTypes"
                        :form-data="requestForm.properties.body"
                />
            </b-tab-item>

            <b-tab-item label="Socket" :visible="requestForm.requestType === 50">
                <RequestFormPropertiesTable
                        :header-types="socketPropertyTypes"
                        :form-data="requestForm.properties.body"
                />
            </b-tab-item>

        </b-tabs>

        <b-button @click="nextStep"
                  :loading.sync="isLoading">
            Next
        </b-button>
    </section>
</template>

<script>
    import RequestFormPropertiesTable from "../tables/request-form-properties-table";

    export default {
        components: {
            RequestFormPropertiesTable
        },
        props: {
            requestTypes: {
                type: Array,
                required: true,
                default: () => ([])
            },
            requestPropertyTypes: {
                type: Array,
                required: true,
                default: () => ([])
            },
            responseTypes: {
                type: Array,
                required: true,
                default: () => ([])
            },
            requestForm: {
                type: Object,
                required: true,
                default: {
                    endpoint: null,
                    requestType: null,
                    responseType: null,
                    // delay: 604800000,
                    // failureDelay: 300000,
                    isEnabled: true,
                    parentType: -1,
                    properties: {
                        params: [],
                        headers: [],
                        body: [],
                        socket: []
                    },
                }
            }
        },
        data() {
            return {
                isLoading: false
            };
        },
        computed: {
            paramPropertyTypes() {
                return this.requestPropertyTypes.filter(requestType => requestType.key.includes("HttpQuery"));
            },
            headerPropertyTypes() {
                return this.requestPropertyTypes.filter(requestType => requestType.key.includes("HttpHeader"));
            },
            bodyPropertyTypes() {
                return this.requestPropertyTypes.filter(requestType => requestType.key.includes("HttpBody"));
            },
            socketPropertyTypes() {
                return this.requestPropertyTypes.filter(requestType => requestType.key.includes("Socket"));
            }
        },
        methods: {
            nextStep() {
                this.isLoading = true;

                if (this.isValidInput()) {
                    this.$emit("dispatchRequest", isSuccess => this.isLoading = false);
                } else {
                    this.isLoading = false;
                }
            },
            isValidInput() {
                let urlPattern = /(https|http|ws|wss):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
                let urlIsValid = urlPattern.test(this.requestForm.endpoint);

                if (!urlIsValid) {
                    this.$buefy.dialog.alert("Invalid URL pattern, please try again");
                }

                return urlIsValid && this.requestForm.requestType > -1
                    && this.requestForm.responseType;
            }
        },
        mounted() {
            if (this.requestForm && this.requestForm.properties) {
                this.isLoading = true;
                let props = this.requestForm.properties;
                
                if (!props.params && props.length > 0) { // Ensure props is an array and is not an object first
                    this.requestForm.properties = { // Convert it to an object
                        params: [],
                        headers: [],
                        body: [],
                        socket: []
                    };
                    for (let prop in props) {
                        if (prop && prop.type) {
                            if (this.paramPropertyTypes.filter(e => e.value === prop.type).length > 0) {
                                this.requestForm.properties.params.push(prop);
                            } else if (this.headerPropertyTypes.filter(e => e.value === prop.type).length > 0) {
                                this.requestForm.properties.headers.push(prop);
                            } else if (this.bodyPropertyTypes.filter(e => e.value === prop.type).length > 0) {
                                this.requestForm.properties.body.push(prop);
                            }  else if (this.socketPropertyTypes.filter(e => e.value === prop.type).length > 0) {
                                this.requestForm.properties.socket.push(prop);
                            }
                        }
                    }
                }
                
                this.isLoading = false;
            }
        }
    }
</script>

<style scoped>
    .is-valigned {
        vertical-align: middle;
    }
</style>