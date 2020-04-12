<template>
    <div class="container is-fluid">
        <p class="title is-2">Cabin</p>
        <p class="subtitle is-4">Welcome to your dashboard</p>
        
        <b-tabs type="is-boxed">
            <b-tab-item label="Categorisation Entities">
                <nav class="level">
                    <div class="level-left">
                        <div class="level-item">
                            <div class="content">
                                <p class="title is-3">Categorisation Entities</p>
                                <p class="subtitle is-5">Manage</p>
                            </div>
                        </div>
                    </div>
                    <div class="level-right">
                        <div class="level-item">
                            <SourceTypeModal @created="createdNewSourceType" />
                        </div>
                    </div>
                </nav>
                
                <SourceTypesTable ref="sourceTypeTable" />
            </b-tab-item>
            
            <b-tab-item label="Request Entities">
                <nav class="level">
                    <div class="level-left">
                        <div class="level-item">
                            <div class="content">
                                <p class="title is-3">Request Entities</p>
                                <p class="subtitle is-5">Manage</p>
                            </div>
                        </div>
                    </div>
                    <div class="level-right">
                        <div class="level-item">
                            <CreateRequestComponent @created="createdNewRequest" />
                        </div>
                    </div>
                </nav>
                
                <RequestsTable ref="reqTable" />
            </b-tab-item>
            
            <b-tab-item label="Compute Entities">
                <nav class="level">
                    <div class="level-left">
                        <div class="level-item">
                            <div class="content">
                                <p class="title is-3">Compute Entities</p>
                                <p class="subtitle is-5">Manage</p>
                            </div>
                        </div>
                    </div>
                    <div class="level-right">
                        <div class="level-item">
                            <CurrencyTypeModal />
                        </div>
                    </div>
                </nav>
                
                <CurrencyTypesTable />
            </b-tab-item>
            
            <b-tab-item label="Analytics" disabled>
                <p>Coming Soon!</p>
            </b-tab-item>
        </b-tabs>
    </div>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    // Request imports
    import CreateRequestComponent from '@/components/modals/request-modal';
    import CurrencyTypeModal from '@/components/modals/currency-type-modal';
    import SourceTypeModal from '@/components/modals/source-type-modal';
    import CurrencyTypesTable from '@/components/tables/currency-types-table';
    import RequestsTable from '@/components/tables/requests-table';
    import SourceTypesTable from '@/components/tables/source-types-table';

    export default {
        name: "Dashboard",
        components: { CreateRequestComponent, CurrencyTypeModal, SourceTypeModal, 
            CurrencyTypesTable, RequestsTable, SourceTypesTable },
        data: function () {
            return {
                user: this.oidcUser
            }
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcUser'
            ])
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            createdNewRequest: function(value) {
                if (value)
                    this.$refs.reqTable.updateRequests();
            },
            createdNewSourceType: function() {
                this.$refs.sourceTypeTable.update();
            },
        },
        mounted: function () {
        }
    }
</script>

<style scoped>

</style>
