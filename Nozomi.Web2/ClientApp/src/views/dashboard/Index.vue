<template>
    <div class="container is-fluid">
        <div class="tile is-ancestor">
            <div class="tile is-vertical">
                <div class="tile">
                    <div class="tile is-parent is-vertical">
                        <b-notification aria-close-label="Close notification">
                            <p class="title">Welcome to <b>Cabin</b>!</p>
                            <p class="subtitle">Nozomi's Cabin is designed to make interacting and processing with APIs
                                simple and easy to do.
                                By the end of our beta, APIs can be interacted by providing the API path, properties
                                within it and the data you
                                want to process and that's it!</p>
                        </b-notification>
                        <article class="tile is-child notification is-warning">
                            <p class="title">Favourites</p>
                            <p class="subtitle is-italic">Coding in progress..</p>
                        </article>
                    </div>
                    <div class="tile is-parent">
                        <article class="tile is-child notification is-info">
                            <p class="title">Source Types</p>
                            <p class="subtitle">
                                <CreateSourceTypeModal></CreateSourceTypeModal>
                            </p>
                            <SourceTypesTable></SourceTypesTable>
                        </article>
                    </div>
                </div>
                <div class="tile is-parent">
                    <article class="tile is-child notification is-danger">
                        <b-field group-multiline positon="is-left">
                            <div class="control">
                                <p class="title">Requests</p>
                            </div>
                        </b-field>
                        <b-field position="is-right">
                            <div class="control">
                                <CreateRequestComponent @created="createdNewRequest"></CreateRequestComponent>
                            </div>
                        </b-field>
                        <section>
                            <RequestsTable ref="reqTable" />
                        </section>
                    </article>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    import store from '@/store/index';
    // Request imports
    import CreateRequestComponent from '@/components/modals/request-modal';
    import CreateSourceTypeModal from '@/components/modals/source-type-modal';
    import RequestsTable from '@/components/tables/requests-table';
    import SourceTypesTable from '@/components/tables/source-types-table';

    export default {
        name: "Dashboard",
        components: {CreateRequestComponent, CreateSourceTypeModal, RequestsTable, SourceTypesTable},
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
            createdNewRequest: function (value) {
                if (value)
                    this.$refs.reqTable.updateRequests();
            }
        },
        mounted: function () {
        }
    }
</script>

<style scoped>

</style>
