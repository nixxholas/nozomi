<template>
    <div class="is-parent-container section has-background-white">
        <div class="level is-paddingless">
            <div class="level-left">
                <div class="level-item">
                    <h1 class="title is-5">Analysed Components</h1>
                </div>
            </div>
            
            <div class="level-right" v-if="showCreateFeature">
                <div class="level-item">
                    <AnalysedComponentModal 
                            :currency-slug="currencySlug"
                            :currency-pair-guid="currencyPairGuid"
                            :currency-type-short-form="currencyTypeShortForm"
                            @created="refetchAnalysedComponents"
                    />
                </div>    
            </div>
        </div>
        
        <b-table :data="data">
            <template slot-scope="props">
                <b-table-column field="type" label="Type" sortable centered>
                    <span class="tag is-info"
                          v-if="componentTypes && componentTypes.length > 0">
                        <strong>{{ componentTypes.filter(e => e.key == props.row.type)[0].value }}</strong>
                    </span>
                </b-table-column>

                <b-table-column field="delay" label="Interval">
                    <i>{{ props.row.delay }}</i>
                </b-table-column>

                <b-table-column field="storeHistoricals" label="Store Historicals">
                    <b-icon v-if="props.row.storeHistoricals" icon="check"/>
                    <b-icon v-else icon="times"/>
                </b-table-column>

                <b-table-column field="isDenominated" label="Denominated">
                    <b-icon v-if="props.row.isDenominated" icon="check"/>
                    <b-icon v-else icon="times"/>
                </b-table-column>

                <b-table-column field="isFailing" label="Failing">
                    <b-icon v-if="props.row.isFailing" icon="check"/>
                    <b-icon v-else icon="times"/>
                </b-table-column>

                <b-table-column field="value" label="Value">
                    {{ props.row.value ? props.row.value : "" }}
                </b-table-column>

                <b-table-column>
                    <AnalysedComponentModal :guid="props.row.guid"/>
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
    import AnalysedComponentModal from '@/components/modals/analysed-component-modal';
    import AnalysedComponentService from "@/services/AnalysedComponentService";
    import AnalysedComponentTypeService from "@/services/AnalysedComponentTypeService";

    export default {
        name: "analysed-components-table",
        components: {AnalysedComponentModal},
        props: {
            showCreateFeature: {
                type: Boolean,
                default: false
            },
            currencySlug: {
                type: String,
                default: null
            },
            currencyPairGuid: {
                type: String,
                default: null
            },
            currencyTypeShortForm: {
                type: String,
                default: null
            }
        },
        data: function () {
            return {
                page: 1,
                data: [],
                componentTypes: []
            }
        },
        mounted: function () {
            let self = this;

            // Load all parent's ACs
            if (self.currencySlug || self.currencyPairGuid || self.currencyTypeShortForm) {
                this.fetchAnalysedComponents();
            } else {
                // TODO: Implement non-filtered table
            }

            AnalysedComponentTypeService.all()
                .then(function (res) {
                    if (res && res.data && res.data.length > 0)
                        self.componentTypes = res.data;
                }).catch(function (err) {
                console.dir(err);
            });
        },
        methods: {
            async refetchAnalysedComponents(isAddedSuccessfully) {
                if (isAddedSuccessfully) {
                    console.warn("Refetching data");
                    this.fetchAnalysedComponents();
                }
            },
            async fetchAnalysedComponents() {
                try {
                    const response = await AnalysedComponentService.all(this.currencySlug, this.currencyPairGuid, 
                        this.currencyTypeShortForm);
                    this.data = response.data;
                } catch(e) { }
            }
        }
    }
</script>

<style scoped>
    .is-parent-container {
        padding: 1rem 1.5rem;
    }
</style>
