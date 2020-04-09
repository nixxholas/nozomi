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
                          v-model="requestForm.requestMethod"
                          size="is-medium"
                >
                    <option v-for="requestMethod in requestMethods"
                            :key="requestMethod.key"
                            :value="requestMethod.value"
                    >
                        {{ requestMethod.key }}
                    </option>
                </b-select>
            </b-field>

            <b-field label="Enter a request URL" label-position="inside" expanded>
                <b-input v-model="requestForm.url"
                         size="is-medium"
                         placeholder="https://jsonplaceholder.typicode.com/users"
                ></b-input>
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

        <!-- Data polling settings for Nozomi -->
        <h1 class="subtitle pt-5">Fetch frequency</h1>

        <b-field label-position="inside">
            <template slot="label">
                <p>
                    <span>Frequency (in Milliseconds)</span>
                    <b-tooltip type="is-dark"
                               class="is-valigned"
                               label="Lower frequency will result in having a more up-to-date dataset."
                               multilined
                    >
                        <b-icon size="is-small" icon="question-circle"></b-icon>
                    </b-tooltip>
                </p>
            </template>

            <b-input type="number"
                     size="is-medium"
                     v-model="requestForm.delay"
                     placeholder="604800000 (1 week)"
            >
            </b-input>
        </b-field>

        <b-field label-position="inside">
            <template slot="label">
                <p>
                    <span>Idle time before retry (in Milliseconds)</span>
                    <b-tooltip type="is-dark"
                               label="When a fetch fails, it will wait until the idle time expires before fetching again."
                               class="is-valigned"
                               multilined
                    >
                        <b-icon size="is-small" icon="question-circle"></b-icon>
                    </b-tooltip>
                </p>
            </template>
            <b-input type="number"
                     size="is-medium"
                     v-model="requestForm.failureDelay"
                     placeholder="300000 (5 minutes)"
            >
            </b-input>
        </b-field>

        <!-- Request properties for Nozomi -->
        <div class="columns is-desktop pt-5">
            <div class="column is-4-desktop">
                <h1 class="subtitle">Properties</h1>
            </div>
            <!-- TODO: FIX THE RESPONSIVENESS MAN THIS IS HARD SHIT -->
            <div class="column is-offset-6-desktop">
                <CreateRequestPropertyModal />
            </div>
        </div>

        <b-field label-position="inside">
            <template slot="label">
                <p>
                    <span>What are request properties actually?</span>
                    <b-tooltip type="is-dark"
                               class="is-valigned"
                               label="Properties are special parameters required by the source (i.e. your identity key)."
                               multilined>
                        <b-icon size="is-small" icon="question-circle"></b-icon>
                    </b-tooltip>
                </p>
            </template>

            <!-- Request properties table -->
            <b-table :data="requestForm.properties && requestForm.properties.length > 0"
                    :bordered="true" :narrowed="true" :hoverable="false" :focusable="false" :mobile-cards="true">

                <template slot-scope="props">
                    <b-table-column field="type" label="Type">
                        {{ props.row.type }}
                    </b-table-column>

                    <b-table-column field="key" label="Key">
                        {{ props.row.key }}
                    </b-table-column>

                    <b-table-column field="value" label="Value" centered>
                    <span class="tag is-success">
                        {{ props.row.value }}
                    </span>
                    </b-table-column>
                </template>

                <template slot="empty">
                    <section class="section">
                        <div class="content has-text-grey has-text-centered">
                            <p>Nothing yet hmm..</p>
                        </div>
                    </section>
                </template>
            </b-table>
        </b-field>
        
        <b-button @click="submitForm">Next</b-button>

    </section>
</template>

<script>
    import CreateRequestPropertyModal from '../../../components/modals/request-property-modal'
    
    export default {
        components: {
            CreateRequestPropertyModal
        },
        props: {
            requestMethods: {
                type: Array,
                required: true,
                default: []
            },
            responseTypes: {
                type: Array,
                required: true,
                default: []
            },
            requestForm: {
                type: Object,
                required: true,
                default: {
                    url: null,
                    requestMethod: null,
                    responseType: null,
                    delay: 604800000,
                    failureDelay: 300000,
                    isEnabled: true,
                    parentType: -1,
                    properties: [],
                }
            }
        },
        methods: {
            submitForm() {
                this.$emit("onCreate");
            }
        }
    }
</script>

<style scoped>
    .is-valigned {
        vertical-align: middle;
    }
</style>