<template>
    <div v-bind:class="{ section: isSection }">
        <b-navbar :spaced="isSection">
            <template slot="brand">
                <b class="has-text-dark">Websocket Commands</b>
            </template>
            <template v-if="showCreateFeature"
                      slot="end">
                <WebsocketCommandModal :child-mode="true" 
                                               :request-guid="requestGuid"
                                               @added="addNewProperty"
                                               @created="reload"/>
            </template>
        </b-navbar>
        <b-table :loading="tableLoading"
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
                
                <b-table-column v-if="props.row.guid" field="actions">
                    <WebsocketCommandPropertyModal :guid="props.row.guid" @updated="reload" @deleted="reload"/>
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
    import WebsocketCommandService from "@/services/WebsocketCommandService";

    export default {
        name: "websocket-command-table",
        components: {WebsocketCommandModal},
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
            }
        },
    }
</script>

<style scoped>

</style>
