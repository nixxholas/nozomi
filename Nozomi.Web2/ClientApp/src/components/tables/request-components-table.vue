<template>
    <div class="section">
        <b-navbar spaced="true">
            <template slot="brand">
                <b class="has-text-dark">Components</b>
            </template>
            <template v-if="showCreateFeature"
                      slot="end">
                <CreateRequestComponentModal/>
            </template>
        </b-navbar>
        <b-table :data="data">
            <template slot-scope="props">
                <b-table-column field="type" label="Type" sortable centered>
                    <span class="tag is-info">
                        {{ props.row.type }}
                    </span>
                </b-table-column>

                <!--                    <b-table-column field="identifier" label="Identifier" sortable>-->
                <!--                        <a @click="toggle(props.row)">-->
                <!--                            {{ props.row.identifier }}-->
                <!--                        </a>-->
                <!--                    </b-table-column>-->

                <!--                    <b-table-column field="queryComponent" label="Query Component" sortable>-->
                <!--                        {{ props.row.queryComponent }}-->
                <!--                    </b-table-column>-->

                <!--                    <b-table-column field="storeHistorical" label="Store History">-->
                <!--                        <b-checkbox v-model="form.storeHistorical"-->
                <!--                                    true-value="Yes"-->
                <!--                                    false-value="No">-->
                <!--                            {{ form.storeHistorical }}-->
                <!--                        </b-checkbox>-->
                <!--                    </b-table-column>-->

                <b-table-column field="isDenominated" label="Denominated">
                    <b-icon v-if="props.row.isDenominated" icon="check"/>
                    <b-icon v-else icon="times"/>
                </b-table-column>

                <!--                    <b-table-column field="anomalyIgnorance" label="Ignore Anomalies">-->
                <!--                        <b-checkbox v-model="form.anomalyIgnorance"-->
                <!--                                    true-value="Yes"-->
                <!--                                    false-value="No">-->
                <!--                            {{ form.anomalyIgnorance }}-->
                <!--                        </b-checkbox>-->
                <!--                    </b-table-column>-->

                <b-table-column field="value" label="Value">
                    {{ props.row.value ? props.row.value : "" }}
                </b-table-column>
            </template>
            <template slot="empty">
                <section class="section">
                    <div class="content has-text-grey has-text-centered">
                        <p>
                            <b-icon
                                    icon="sad-cry"
                                    size="is-large">
                            </b-icon>
                        </p>
                        <p>No components yet.</p>
                    </div>
                </section>
            </template>
        </b-table>
    </div>
</template>

<script>
    import CreateRequestComponentModal from '../modals/create-request-component-modal';
    import ComponentService from "../../services/ComponentService";

    export default {
        name: "request-components-table",
        components: {CreateRequestComponentModal},
        props: {
            showCreateFeature: {
                type: Boolean,
                default: false
            },
            guid: {
                type: String,
                default: null
            }
        },
        data: function () {
            return {
                requestGuid: this.guid,
                data: []
            }
        },
        mounted: function () {
            let self = this;

            // If this is a request-specific 
            if (self.requestGuid) {
                ComponentService.allByRequest(self.requestGuid)
                    .then(function (res) {
                        self.data = res.data;
                    });
            } else {
                // Else load everything because there's nothing specific
                ComponentService.all()
                    .then(function (res) {
                        self.data = res.data;
                    });
            }
        }
    }
</script>

<style scoped>

</style>
