<template>
    <div v-bind:class="{ section: isSection }" class="is-parent-container has-background-white">
        <WebsocketCommandModal :has-navbar="isSection"
                               :request-guid="requestGuid"
                               @added="addNewProperty"
                               @created="reload"/>
        
        <b-table detailed
                 detail-key="guid"
                 :loading="tableLoading"
                 :data="data">
            <template slot-scope="props">
                <b-table-column field="type" label="Type" sortable centered>
                    <span class="tag is-info"
                          v-if="propertyTypes && propertyTypes.length > 0">
                        {{ propertyTypes.filter(e => e.value === props.row.type)[0].key }}
                    </span>
                </b-table-column>

                <b-table-column field="name" label="Name">
                    {{ props.row.name }}
                </b-table-column>

                <b-table-column field="delay" label="Delay">
                    {{ props.row.delay >= 0 ? props.row.delay : "" }}
                </b-table-column>

                <b-table-column field="isEnabled" label="Enabled">
                    <b-checkbox v-model="props.row.isEnabled" disabled/>
                </b-table-column>
                
                <b-table-column v-if="props.row.guid" field="guid" label="">
                    <WebsocketCommandModal :command="props.row" @updated="reload" @deleted="reload"/>
                </b-table-column>
            </template>
            <template slot="detail" slot-scope="props">
                <WebsocketCommandPropertyModal :has-navbar="true"
                                               :command-guid="props.row.guid"
                                               @created="reload"/>
                <!--TODO: Make sure the table component updates the rows in UI as well-->
                <WebsocketCommandPropertyTable
                        @pushed="reload"
                        :properties="props.row.properties"
                        :show-create-feature="false" />
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
                        <p>No commands yet.</p>
                    </div>
                </section>
            </template>
        </b-table>
    </div>
</template>

<script>
    import WebsocketCommandPropertyTypeService from "@/services/WebsocketCommandPropertyTypeService";
    import WebsocketCommandModal from "../modals/websocket-command-modal";
    import WebsocketCommandPropertyModal from "../modals/websocket-command-property-modal";
    import WebsocketCommandPropertyTable from "../tables/websocket-command-properties-table";
    import WebsocketCommandService from "@/services/WebsocketCommandService";
    import WebsocketCommandPropertyService from "@/services/WebsocketCommandPropertyService";

    export default {
        name: "websocket-command-table",
        components: {WebsocketCommandModal, WebsocketCommandPropertyModal, WebsocketCommandPropertyTable},
        props: {
            showCreateFeature: {
                type: Boolean,
                default: false
            },
            requestGuid: {
                type: String,
                default: null
            },
            isSection: {
                type: Boolean,
                default: true
            },
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
            
            WebsocketCommandPropertyTypeService.all()
                .then(function (res) {
                    if (res.status && res.status === 200) {
                        self.propertyTypes = res.data;

                        // If this is a request-specific 
                        if (self.requestGuid) {
                            WebsocketCommandService.viewByRequest(self.requestGuid)
                                .then(function (res) {
                                    self.data = res.data;
                                });
                        }
                    }
                })
                .finally(function () {
                    self.tableLoading = false;
                });
        },
        methods: {
            addNewProperty(payload) {
                if (!this.data)
                    this.data = [];
                
                this.data.push(payload);
            },
            reload: function () {
                let self = this;
                self.tableLoading = true;

                WebsocketCommandService.viewByRequest(self.requestGuid)
                    .then(function (res) {
                        self.data = res.data;
                    })
                    .finally(function () {
                        self.tableLoading = false;
                    });
            },
            reloadProperties: function(commandGuid) {
                let self = this;
                
                if (commandGuid) {
                    WebsocketCommandPropertyService.getByCommand(commandGuid)
                    .then(function (res) {
                        if (res.data) {
                            // TODO: Ensure that filtered collection can be updated.
                            self.data.filter(e => e.guid === commandGuid)[0].properties = res.data;
                            console.dir(self.data.filter(e => e.guid === commandGuid)[0].properties);
                            console.dir(res.data);
                        }
                    })
                }
            },
        },
    }
</script>

<style scoped>
    .is-parent-container {
        padding: 1rem 1.5rem;
        margin-bottom: 3rem;
    }
</style>
