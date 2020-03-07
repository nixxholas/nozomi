<template>
    <div>
        <b-table
                :data="data"
                :current-page.sync="currentPage"
                :per-page="perPage"

                paginated
                backend-pagination
                :total="dataCount"
                @page-change="onPageChange"

                backend-sorting
                :default-sort-direction="defaultSortOrder"
                :default-sort="[sortField, sortOrder]"
                @sort="onSort"

                default-sort="name"
                aria-next-label="Next page"
                aria-previous-label="Previous page"
                aria-page-label="Page"
                aria-current-label="Current page">
            <template slot="empty">
                <section class="section">
                    <div class="content has-text-grey has-text-centered">
                        <p>
                            <b-icon
                                    icon="frown"
                                    size="is-large">
                            </b-icon>
                        </p>
                        <p>Nothing here.</p>
                    </div>
                </section>
            </template>

            <template slot-scope="props">
                <b-table-column field="name" label="Name" sortable>
                    <router-link :to="`/currency/${props.row.slug}`">
                        <img v-if="props.row.logoPath != null"
                             :src="props.row.logoPath" class="mr-1"
                             style="width: 24px; height: 24px; vertical-align: bottom;"/>
                        {{ props.row.name }}
                    </router-link>
                </b-table-column>

                <b-table-column v-if="!type" field="currencyType" label="Type" sortable>
                    {{ getType(props.row.currencyTypeGuid).name }}
                </b-table-column>

                <b-table-column
                        v-for="component in props.row.components"
                        v-if="props.row.components && props.row.components.length > 0 
                        && displayComponents && displayComponents.length > 0"
                        :visible="componentIsDisplayable(component.type)">
                    {{ component }}
                </b-table-column>

                <b-table-column v-if="oidcIsAuthenticated">
                    <CurrencyModal :currency="props.row"/>
                </b-table-column>
            </template>
        </b-table>
        <b-loading :active.sync="dataLoading" :is-full-page="false" />
    </div>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    import CurrencyModal from '@/components/modals/currency-modal';
    import CurrencyService from "@/services/CurrencyService";
    import CurrencyTypeService from "@/services/CurrencyTypeService";

    export default {
        name: "currencies-table",
        props: {
            type: null,
            perPage: {
                default: 50,
                type: Number
            },
            displayComponents: [],
            isActive: {
                type: Boolean,
                default: null
            },
            currencyTypes: {
                type: Array,
                default: null
            }
        },
        components: {
            CurrencyModal
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated', 'oidcUser'
            ]),
            sortFieldEnum: function () {
                switch (this.sortField) {
                    case "name":
                        return 1;
                    case "abbreviation":
                        return 2;
                    case "slug":
                        return 3;
                    case "currencyType":
                        return 4;
                    default:
                        return 0;
                }
            }
        },
        data: function () {
            return {
                currentPage: 1,
                dataLoading: false,
                data: [],
                dataCount: 0,
                sortField: 'name',
                defaultSortOrder: 'asc',
                sortOrder: 'asc',
                typeData: []
            }
        },
        watch: {
            isActive: function(){ 
                this.getTableData();
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            componentIsDisplayable(type) {
                if (this.displayComponents && this.displayComponents.length > 0) {
                    for (let i = 0; i < this.displayComponents.length; i++) {
                        if (this.displayComponents[i] === type)
                            return true;
                    }
                }

                return false;
            },
            getType(guid) {
                const types = this.typeData;
                if (types && types.length > 0)
                    for (var i = 0; i < types.length; i++) {
                        if (types[i].guid === guid)
                            return types[i];
                    }

                return "Null"
            },
            onPageChange: function (page) {
                let self = this;
                self.dataLoading = true;

                CurrencyService.listAll(page - 1, self.perPage, self.type)
                    .then(function (res) {
                        self.data = res;

                        self.dataLoading = false;
                    });
            },
            onSort(field, order) {
                let self = this;
                self.dataLoading = true;

                self.sortField = field;
                self.sortOrder = order;

                let sortAscending = order === "asc";

                CurrencyService.listAll(self.currentPage - 1, self.perPage, self.type, sortAscending, self.sortFieldEnum)
                    .then(function (res) {
                        self.data = res;

                        self.dataLoading = false;
                    });
            },
            shouldLoadData() {
                // Loads table data when mounted when there is no active props given
                if (this.isActive === null)
                    return true;

                return this.isActive && !this.dataLoading &&
                    (this.data.length === 0 || this.typeData.length === 0);
            },
            async getTableData() {
                if (!this.shouldLoadData())
                    return;
                
                this.dataLoading = true;
                const sortAscending = this.sortOrder === "asc";
                
                try {
                    const [
                        currencyCount, 
                        currencyList
                    ] = await Promise.all([
                        CurrencyService.getCurrencyCount(this.type),
                        CurrencyService.listAll(this.currentPage - 1, this.perPage, this.type, sortAscending, this.sortFieldEnum)
                    ]);
                    
                    this.dataCount = currencyCount;
                    this.data = currencyList;
                } catch(e) {
                    this.$buefy.toast.open({
                        message: "An error occurred on our side, please try again.",
                        type: "is-danger",
                        position: "is-bottom-right"
                    });
                }
                
                this.dataLoading = false;
            },
            async getTypesData() {
                if (this.currencyTypes === null) {
                    try {
                        this.typeData = await CurrencyTypeService.all();
                    } catch(e) {
                        this.$buefy.toast.open({
                            message: "An error occurred on our side, please try again.",
                            type: "is-danger",
                            position: "is-bottom-right"
                        });    
                    }
                }
            }
        },
        mounted: function () {
            this.getTableData();
            this.getTypesData();
            
            // Use prop typeData as it is already loaded in parent component
            if (this.currencyTypes !== null) {
                this.typeData = this.currencyTypes;
            }
        }
    }
</script>

<style scoped>

</style>