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
                          size="is-medium">
                    <option v-for="requestMethod in requestMethods"
                            :key="requestMethod.key"
                            :value="requestMethod.value">
                        {{ requestMethod.key }}
                    </option>
                </b-select>
            </b-field>

            <b-field label="Enter a request URL" label-position="inside" expanded>
                <b-input v-model="requestForm.endpoint" type="url" size="is-medium"
                         placeholder="https://jsonplaceholder.typicode.com/users" />
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
                <CreateRequestPropertyModal :independent-mode="true" @created="propertyCreated"/>
            </div>
        </div>

        <b-field label-position="on-border">
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
            <b-table :data="requestForm.properties" :narrowed="false" :hoverable="false"
                     :focusable="false" :mobile-cards="true" class="p-5">

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
                    
                    <b-table-column field="actions" label="">
                        <b-button type="is-danger" @click="removeProperty(props.row)">
                            <b-icon
                                    pack="fas"
                                    icon="trash"
                                    size="is-small">
                            </b-icon>
                        </b-button>
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
                    endpoint: null,
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
        created() {
            if (this.requestForm && !this.requestForm.requestMethod 
                && this.requestMethods && this.requestMethods.length > 0)
                this.requestForm.requestMethod = this.requestMethods[0].value;

            if (this.requestForm && !this.requestForm.responseType
                && this.responseTypes && this.responseTypes.length > 0)
                this.requestForm.responseType = this.responseTypes[0];
        },
        methods: {
            propertyCreated(entity) {
                if (this.requestForm.properties && this.requestForm.properties.length >= 0) {
                    this.requestForm.properties.push(entity);
                }
            },
            removeProperty(entity) {
                // https://stackoverflow.com/questions/5767325/how-can-i-remove-a-specific-item-from-an-array
                const index = this.requestForm.properties.indexOf(entity);
                if (index > -1) {
                    this.requestForm.properties.splice(index, 1);
                }
            }
        },
        watch: {
            requestForm: {
                deep: true,
                
                handler() {
                    let urlPattern = /(https|http|ws|wss):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
                    let urlIsValid = urlPattern.test(this.requestForm.endpoint);
                    
                    if (this.requestForm && urlIsValid && this.requestForm.requestMethod > -1
                        && this.requestForm.responseType && this.requestForm.delay && this.requestForm.delay >= 0
                        && this.requestForm.failureDelay && this.requestForm.failureDelay >= 0)
                        this.$emit("onEnable");
                    else
                        this.$emit("onDisable");
                }
            },
        }
    }
</script>

<style scoped>
    .is-valigned {
        vertical-align: middle;
    }
</style>