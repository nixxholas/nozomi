<template>
    <div class="is-parent-container section has-background-white">
        <div class="level">
            <div class="level-left">
                <div class="level-item">
                    <h1 class="title is-5">Properties</h1>
                </div>
            </div>
            <div class="level-right" v-if="showCreateFeature && requestGuid">
                <div class="level-item">
                    <RequestPropertyModal :request-guid="requestGuid" @created="reload"/>
                </div>
            </div>
        </div>
        
        <b-table :loading="tableLoading"
                 :data="data">
            <template slot-scope="props">
                <b-table-column field="type" label="Type" sortable centered>
                    <span class="tag is-info"
                          v-if="propertyTypes && propertyTypes.length > 0">
                        {{ propertyTypes.filter(e => e.value == props.row.type)[0].key }}
                    </span>
                </b-table-column>

                <b-table-column field="key" label="Key">
                    {{ props.row.key }}
                </b-table-column>

                <b-table-column field="value" label="Value">
                    {{ props.row.value ? props.row.value : "" }}
                </b-table-column>
                
                <b-table-column field="actions">
                    <RequestPropertyModal :guid="props.row.guid" @updated="reload" @deleted="reload"/>
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
    import RequestPropertyTypeService from "@/services/RequestPropertyTypeService";

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
                tableLoading: true,
                data: [],
                propertyTypes: []
            }
        },
        mounted: function () {
            let self = this;

            // If this is a request-specific 
            if (self.requestGuid) {
                RequestPropertyTypeService.all()
                    .then(function (res) {
                        if (res.status && res.status === 200) {
                            self.propertyTypes = res.data;

                            RequestPropertyService.getAllByRequest(self.requestGuid)
                                .then(function (res) {
                                    self.data = res.data;
                                });
                        }
                    })
                    .finally(function () {
                        self.tableLoading = false;
                    });
            }
        },
        methods: {
            reload: function () {
                let self = this;
                self.tableLoading = true;

                RequestPropertyService.getAllByRequest(self.requestGuid)
                    .then(function (res) {
                        self.data = res.data;
                    })
                    .finally(function () {
                        self.tableLoading = false;
                    });
            }
        },
    }
</script>

<style scoped>
    .is-parent-container {
        padding: 1rem 1.5rem;
        margin-bottom: 3rem;
    }
</style>
