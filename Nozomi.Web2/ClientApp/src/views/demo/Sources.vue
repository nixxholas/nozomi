<template>
    <div>
        <h1 class="title">Sources</h1>
        <h2 class="subtitle">Where our data is retrieved</h2>

        <b-field grouped group-multiline v-if="oidcIsAuthenticated">
            <div class="control">
                <CreateSourceModal @created="newSourceCreated"/>
            </div>
        </b-field>

        <b-table :data="data"
                 :current-page.sync="currentPage"
                 :per-page="perPage"
                 :loading="dataLoading"
                 :paginated="true"
                 default-sort="name"
                 aria-next-label="Next page"
                 aria-previous-label="Previous page"
                 aria-page-label="Page"
                 aria-current-label="Current page">
            >
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

                <b-table-column field="sourceTypeGuid" label="Type" v-if="typeData !== null && typeData.length > 0">
                    {{ typeData.filter(e => e.guid == props.row.sourceTypeGuid)[0].name }}
                </b-table-column>
            </template>
        </b-table>
    </div>
</template>

<script>
    import {mapGetters} from 'vuex';
    import CreateSourceModal from '@/components/modals/create-source-modal';
    import SourceService from '@/services/SourceService';
    import SourceTypeService from '@/services/SourceTypeService';

    export default {
        components: {
            CreateSourceModal
        },
        data: function () {
            return {
                dataLoading: false,
                data: [],
                currentPage: 1,
                perPage: 50,
                typeData: [],
            };
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated'
            ])
        },
        methods: {
            async getSources() {
                this.dataLoading = true;

                try {
                    this.data = await SourceService.getAll();
                } catch (e) {
                }

                this.dataLoading = false;
            },
            async getSourceTypes() {
                this.dataLoading = true;

                try {
                    this.typeData = await SourceTypeService.getAll();
                } catch (e) {
                }

                this.dataLoading = false;
            },
            newSourceCreated(value) {
                if (value) {
                    this.getSources();
                }
            }
        },
        mounted() {
            this.getSources();
            this.getSourceTypes();
        },
    }
</script>