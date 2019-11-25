<template>
    <div>
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
                <CurrencyModal class="mb-4" v-if="oidcIsAuthenticated"></CurrencyModal>
            </div>
        </b-field>

        <b-table
                :data="data"
                :current-page.sync="currentPage"
                :per-page="perPage"
                :loading="dataLoading"
                :paginated="true"

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
                    {{ props.row.name }}
                </b-table-column>
                
                <b-table-column field="type" label="Type" sortable>
                    {{ getType(props.row.currencyTypeGuid).name }}
                </b-table-column>

<!--                <b-table-column field="sourceTypeGuid" label="Type" v-if="typeData !== null && typeData.length > 0">-->
<!--                    {{ typeData.filter(e => e.guid == props.row.sourceTypeGuid)[0].name }}-->
<!--                </b-table-column>-->
            </template>
        </b-table>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import CurrencyModal from '@/components/modals/currency-modal';
    import CurrencyService from "../../services/CurrencyService";
    import CurrencyTypeService from "../../services/CurrencyTypeService";

    export default {
        name: "currency-index",
        components: {
            CurrencyModal
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
                currentPage: 1,
                perPage: 50,
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
            }
        },
        mounted: function() {
            let self = this;
            CurrencyService.listAll()
                .then(function (res) {
                    self.data = res;

                    self.dataLoading = false;
                });

            CurrencyTypeService.getAll()
                .then(function (res) {
                    self.typeData = res;
                })
        }
    }
</script>

<style scoped>

</style>