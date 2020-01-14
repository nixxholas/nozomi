<template>
    <div>
        <b-table
                :data="data"
                :current-page.sync="currentPage"
                :per-page="perPage"
                :loading="dataLoading"

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

                <b-table-column field="currencyType" label="Type" sortable>
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
        <b-loading :active.sync="dataLoading"/>
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
            displayComponents: []
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
                dataLoading: true,
                data: [],
                dataCount: 0,
                sortField: 'name',
                defaultSortOrder: 'asc',
                sortOrder: 'asc',
                typeData: []
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
        },
        mounted: function () {
            let self = this;
            let sortAscending = self.sortOrder === "asc";

            CurrencyService.getCurrencyCount(self.type)
                .then(function (res) {
                    self.dataCount = res;

                    CurrencyService.listAll(self.currentPage - 1, self.perPage,
                        self.type, sortAscending, self.sortFieldEnum)
                        .then(function (res) {
                            self.data = res;
                        });
                })
                .catch(function (err) {
                    console.dir(err);
                });

            CurrencyTypeService.all()
                .then(function (res) {
                    self.typeData = res.data;

                    self.dataLoading = false;
                })
        }
    }
</script>

<style scoped>

</style>