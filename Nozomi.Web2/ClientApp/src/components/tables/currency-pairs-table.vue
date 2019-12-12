<template>
    <b-table
            :data="data"
            :loading="loading"

            :mobile-cards="mobile"
            :per-page="perPage"
            :total="count"
            @page-change="onPageChange"
            aria-current-label="Current page"
            aria-next-label="Next page"
            aria-page-label="Page"
            aria-previous-label="Previous page"
            backend-pagination
            paginated>
        <template slot="empty">
            <section class="section">
                <div class="content has-text-grey has-text-centered">
                    <p>
                        <b-icon
                                icon="frown"
                                size="is-large">
                        </b-icon>
                    </p>
                    <p>No pairs yet.</p>
                </div>
            </section>
        </template>
        <template slot-scope="props">
            <b-table-column field="tickerPair" label="Pair" sortable>
                {{ props.row.mainTicker + props.row.counterTicker }}
            </b-table-column>
            <b-table-column field="source" label="Source" sortable>
                {{ props.row.source.name }}
            </b-table-column>
            <b-table-column field="price" label="Price" 
                            v-if="props.row.analysedComponents.filter(ac => ac.type === 10).length > 0">
                <div v-if="props.row.analysedComponents.filter(ac => ac.type === 10)[0].uiFormatting">
                    {{ props.row.analysedComponents.filter(ac => ac.type === 10)[0].value
                    | numeralFormat(props.row.analysedComponents.filter(ac => ac.type === 10)[0].uiFormatting.toString()) }}
                </div>
                <div v-else>
                    {{ props.row.analysedComponents.filter(ac => ac.type === 10)[0].value }}
                </div>
            </b-table-column>
        </template>
    </b-table>
</template>

<script>
    import ComponentService from "@/services/ComponentService";
    import CurrencyPairService from "@/services/CurrencyPairService";
    
    export default {
        name: "currency-pairs-table",
        props: {
            mobile: {
                default: true,
                type: Boolean
            },
            perPage: {
                default: 50,
                type: Number
            },
            mainTicker: {
                default: null,
                type: String
            },
            sourceGuid: {
                default: null,
                type: String
            }
        },
        data: function() {
            return {
                loading: true,
                count: 0,
                data: [],
                page: 0,
                sortField: 'tickerPair',
                defaultSortOrder: 'desc',
                sortOrder: 'desc',
            }
        },
        mounted: function() {
            let self = this;
            
            CurrencyPairService.getCount(self.mainTicker)
                .then(function(res) {
                    self.count = res;
            });
            
            CurrencyPairService.all(self.page, self.perPage, self.sourceGuid, self.mainTicker,
                self.defaultSortOrder === "asc", self.sortField)
                .then(function(res) {
                    self.data = res;
                });
            
            self.loading = false;
        },
        methods: {
            onPageChange(page) {
                let self = this;

                CurrencyPairService.all(page - 1, self.perPage, self.sourceGuid, self.mainTicker,
                    self.defaultSortOrder === "asc", self.sortField)
                    .then(function(res) {
                        self.data = res;
                    });
            }
        }
    }
</script>

<style scoped>

</style>