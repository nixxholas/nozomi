<template>
    <div v-bind:class="{ section: isSection }">
        <b-navbar :spaced="isSection">
            <template slot="brand">
                <b class="has-text-dark">Command Properties</b>
            </template>
            <template v-if="showCreateFeature"
                      slot="end">
                <WebsocketCommandPropertyModal :child-mode="true" 
                                               :command-guid="commandGuid"
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
                        {{ propertyTypes.filter(e => e.value == props.row.type)[0].key }}
                    </span>
                </b-table-column>

                <b-table-column field="key" label="Key">
                    {{ props.row.key }}
                </b-table-column>

                <b-table-column field="value" label="Value">
                    {{ props.row.value ? props.row.value : "" }}
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
                        <p>No properties yet.</p>
                    </div>
                </section>
            </template>
        </b-table>
    </div>
</template>

<script>
    import WebsocketCommandPropertyService from "@/services/WebsocketCommandPropertyService";
    import WebsocketCommandPropertyTypeService from "@/services/WebsocketCommandPropertyTypeService";
    import WebsocketCommandPropertyModal from "../modals/websocket-command-property-modal";

    export default {
        name: "websocket-command-properties-table",
        components: {WebsocketCommandPropertyModal},
        props: {
            showCreateFeature: {
                type: Boolean,
                default: false
            },
            commandGuid: {
                type: String,
                default: null
            },
            isSection: {
                type: Boolean,
                default: true
            },
            properties: {
                type: Array,
                default: []
            }
        },
        data: function () {
            return {
                tableLoading: true,
                data: this.properties,
                propertyTypes: []
            }
        },
        mounted: function () {
            let self = this;
            
            WebsocketCommandPropertyTypeService.all()
                .then(function (res) {
                    if (res.status && res.status === 200) {
                        self.propertyTypes = res.data;

                        // If this is a command-specific 
                        if (self.commandGuid) {
                            WebsocketCommandPropertyService.getByCommand(self.commandGuid)
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
                this.$emit('pushed', payload);
            },
            reload: function () {
                let self = this;
                self.tableLoading = true;

                if (self.commandGuid) {
                    WebsocketCommandPropertyService.getByCommand(self.commandGuid)
                        .then(function (res) {
                            self.data = res.data;
                        })
                        .finally(function () {
                            self.tableLoading = false;
                        });
                } else {
                    console.dir("Can't seem to obtain the commandGuid.");
                    self.tableLoading = false;
                }
            }
        },
    }
</script>

<style scoped>

</style>
