<template>
    <div class="container is-fluid">
        <h1 class="title">Cabin</h1>
        <h2 class="subtitle">Welcome to your dashboard</h2>
        
        <b-notification aria-close-label="Close notification">
            <p class="title">Welcome to <b>Cabin</b>!</p>
            <p class="subtitle">Nozomi's Cabin is designed to make interacting and processing with APIs
                simple and easy to do.
                By the end of our beta, APIs can be interacted by providing the API path, properties
                within it and the data you
                want to process and that's it!</p>
        </b-notification>
        
        <b-tabs 
            size="is-medium"
            type="is-boxed"
            vertical
            expanded>

            <b-tab-item label="Categorisation Entities">
                Nunc nec velit nec libero vestibulum eleifend.
                Curabitur pulvinar congue luctus.
                Nullam hendrerit iaculis augue vitae ornare.
                Maecenas vehicula pulvinar tellus, id sodales felis lobortis eget.
            </b-tab-item>
            <b-tab-item label="Request Entities">
                Nunc nec velit nec libero vestibulum eleifend.
                Curabitur pulvinar congue luctus.
                Nullam hendrerit iaculis augue vitae ornare.
                Maecenas vehicula pulvinar tellus, id sodales felis lobortis eget.
            </b-tab-item>
            <b-tab-item label="Compute Entities">
                Nunc nec velit nec libero vestibulum eleifend.
                Curabitur pulvinar congue luctus.
                Nullam hendrerit iaculis augue vitae ornare.
                Maecenas vehicula pulvinar tellus, id sodales felis lobortis eget.
            </b-tab-item>
            <b-tab-item label="Analytics" disabled>
                Nunc nec velit nec libero vestibulum eleifend.
                Curabitur pulvinar congue luctus.
                Nullam hendrerit iaculis augue vitae ornare.
                Maecenas vehicula pulvinar tellus, id sodales felis lobortis eget.
            </b-tab-item>
        </b-tabs>
        <div class="tile is-ancestor is-vertical">
                <div class="tile">
                    <div class="tile is-parent is-vertical">
                        
<!--                        <article class="tile is-child notification is-warning">-->
<!--                            <p class="title">Favourites</p>-->
<!--                            <p class="subtitle is-italic">Coding in progress..</p>-->
<!--                        </article>-->
                    </div>
                </div>
                <div class="tile">
                    <div class="tile is-parent">
                        <article class="tile is-child notification is-dark">
                            <p class="title">Source Types</p>
                            <p class="subtitle">
                                <SourceTypeModal @created="createdNewSourceType" />
                            </p>
                            <SourceTypesTable ref="sourceTypeTable" />
                        </article>
                    </div>
                    <div class="tile is-parent">
                        <article class="tile is-child notification is-dark">
                            <p class="title">Currency Types</p>
                            <p class="subtitle">
                                <CurrencyTypeModal />
                            </p>
                            <CurrencyTypesTable />
                        </article>
                    </div>
                </div>
                <div class="tile is-parent">
                    <article class="tile is-child notification is-dark">
                        <b-field group-multiline positon="is-left">
                            <div class="control">
                                <p class="title">Requests</p>
                            </div>
                        </b-field>
                        <b-field position="is-right">
                            <div class="control">
                                <CreateRequestComponent @created="createdNewRequest" />
                            </div>
                        </b-field>
                        <section>
                            <RequestsTable ref="reqTable" />
                        </section>
                    </article>
                </div>
        </div>
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
