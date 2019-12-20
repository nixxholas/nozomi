<template>
    <div class="container">
        <section class="hero">
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">
                        Currencies
                    </h1>
                    <h2 class="subtitle">
                        
                    </h2>
                </div>
            </div>
        </section>

        <b-field grouped group-multiline v-if="oidcIsAuthenticated">
            <div class="control">
                <CurrencyModal class="mb-4" v-if="oidcIsAuthenticated" />
            </div>
        </b-field>
        
        <CurrencyTable />
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import CurrencyModal from '@/components/modals/currency-modal';
    import CurrencyService from "../../services/CurrencyService";
    import CurrencyTypeService from "../../services/CurrencyTypeService";
    import CurrencyTable from "@/components/tables/currencies-table";

    export default {
        name: "currency-index",
        components: {
            CurrencyModal,
            CurrencyTable
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcAuthenticationIsChecked',
                'oidcUser',
                'oidcIdToken',
                'oidcIdTokenExp'
            ])
        },
        data() {
            return {
                dataLoading: true,
                data: [],
                dataCount: 0,
                currentPage: 1,
                perPage: 50,
                sortField: 'name',
                defaultSortOrder: 'desc',
                sortOrder: 'desc',
                typeData: []
            }
        },
        methods: {
            getType(guid) {
                const types = this.typeData;
                if (types && types.length > 0)
                    for (var i = 0; i < types.length; i++) {
                        if (types[i].guid === guid)
                            return types[i];
                    }
                
                return "Null"
            },
            createdNewSource: function (value) {
                if (value) {
                    let self = this;
                    self.dataLoading = true;
                    CurrencyService.listAll()
                        .then(function (res) {
                            self.data = res;

                            self.dataLoading = false;
                        });
                }
            },
            onCurrencyTablePageChange: function (page) {
                let self = this;
                self.dataLoading = true;

                CurrencyService.listAll(page - 1, self.perPage)
                    .then(function (res) {
                        self.data = res;
                        
                        self.dataLoading = false;
                    });
            },
            onCurrencyTableSort(field, order) {
                let self = this;
                self.dataLoading = true;
                
                self.sortField = field;
                self.sortOrder = order;
                
                let sortAscending = order === "asc";
                
                CurrencyService.listAll(self.currentPage - 1, self.perPage, sortAscending, self.sortField)
                    .then(function (res) {
                        self.data = res;

                        self.dataLoading = false;
                    });
            },
        },
        mounted: function() {
            let self = this;
            CurrencyService.getCurrencyCount(null)
                .then(function (res) {
                    self.dataCount = res;
                });
            
            CurrencyService.listAll()
                .then(function (res) {
                    self.data = res;
                });

            CurrencyTypeService.getAll()
                .then(function (res) {
                    self.typeData = res;

                    self.dataLoading = false;
                })
        }
    }
</script>

<style scoped>

</style>