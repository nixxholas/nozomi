<template>
    <div>
        <b-field>
            <b-navbar>
                <template slot="brand">
                    <b class="has-text-dark">Components</b>
                </template>
                <template slot="end">
                    <CreateRequestComponentModal v-if="showCreateFeature"></CreateRequestComponentModal>
                </template>
            </b-navbar>
            <b-table :data="data">
                <template slot-scope="props">
                    <b-table-column field="type" label="Type" sortable centered>
                    <span class="tag is-success">
                        {{ props.row.type }}
                    </span>
                    </b-table-column>

                    <b-table-column field="identifier" label="Identifier" sortable>
                        <a @click="toggle(props.row)">
                            {{ props.row.identifier }}
                        </a>
                    </b-table-column>

                    <b-table-column field="queryComponent" label="Query Component" sortable>
                        {{ props.row.queryComponent }}
                    </b-table-column>

                    <b-table-column field="storeHistorical" label="Store History">
                        <b-checkbox v-model="form.storeHistorical"
                                    true-value="Yes"
                                    false-value="No">
                            {{ form.storeHistorical }}
                        </b-checkbox>
                    </b-table-column>

                    <b-table-column field="isDenominated" label="Denominated">
                        <b-checkbox v-model="form.isDenominated"
                                    true-value="Yes"
                                    false-value="No">
                            {{ form.isDenominated }}
                        </b-checkbox>
                    </b-table-column>

                    <b-table-column field="anomalyIgnorance" label="Ignore Anomalies">
                        <b-checkbox v-model="form.anomalyIgnorance"
                                    true-value="Yes"
                                    false-value="No">
                            {{ form.anomalyIgnorance }}
                        </b-checkbox>
                    </b-table-column>
                </template>
                <template slot="empty">
                    <section class="section">
                        <div class="content has-text-grey has-text-centered">
                            <p>
                                <b-icon
                                        icon="emoticon-sad"
                                        size="is-large">
                                </b-icon>
                            </p>
                            <p>No components yet.</p>
                        </div>
                    </section>
                </template>
            </b-table>
        </b-field>
    </div>
</template>

<script>
    import CreateRequestComponentModal from '../../elements/create-request-component-modal';

    export default {
        name: "request-components-table",
        components: {CreateRequestComponentModal},
        props: {
            showCreateFeature: false,
            requestId: 0
        },
        data: function () {
            return {
              data: []
            }
        },
        beforeCreate: function () {
            // If this is a request-specific 
            if (this.requestId && this.requestId > 0) {
                
            } else {
                // Else load everything because there's nothing specific
            }
        }
    }
</script>

<style scoped>

</style>
