<template>
    <div class="section">
        <b-navbar :spaced="true">
            <template slot="brand">
                <b class="has-text-dark">Properties</b>
            </template>
            <template v-if="showCreateFeature && requestGuid"
                      slot="end">
                <RequestPropertyModal :request-guid="requestGuid"/>
            </template>
        </b-navbar>
        <b-table :data="data">
            <template slot-scope="props">
                <b-table-column field="type" label="Type" sortable centered>
                    <span class="tag is-info" 
                          v-if="propertyTypes && propertyTypes.length > 0">
                        {{ propertyTypes.filter(e => e.value == props.row.type)[0].key }}
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

                <b-table-column field="key" label="Key">
                    {{ props.row.key }}
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
                        <p>No properties yet.</p>
                    </div>
                </section>
            </template>
        </b-table>
    </div>
</template>

<script>
    import RequestPropertyService from "../../services/RequestPropertyService";
    import RequestPropertyModal from "@/components/modals/request-property-modal";

    export default {
        name: "request-properties-table",
        components: {RequestPropertyModal},
        props: {
            showCreateFeature: {
                type: Boolean,
                default: false
            },
            requestGuid: {
                type: String,
                default: null
            }
        },
        data: function () {
            return {
                data: [],
                propertyTypes: []
            }
        },
        mounted: function () {
            let self = this;

            // If this is a request-specific 
            if (self.requestGuid) {
                RequestPropertyService.getAllByRequest(self.requestGuid)
                    .then(function (res) {
                        self.data = res;
                    });
            }
        }
    }
</script>

<style scoped>

</style>
