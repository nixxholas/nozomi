<template>
  <div class="container">
    <section class="hero">
      <div class="hero-body">
        <div class="container">
          <h1 class="title">
            Sources
          </h1>
          <h2 class="subtitle">
            A look at where all of our data came from.
          </h2>
        </div>
      </div>
    </section>

    <b-field grouped group-multiline v-if="oidcIsAuthenticated">
      <div class="control">
        <CreateSourceModal @created="createdNewSource"></CreateSourceModal>
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

        <b-table-column field="sourceTypeGuid" label="Type" v-if="typeData !== null && typeData.length > 0">
          {{ typeData.filter(e => e.guid == props.row.sourceTypeGuid)[0].name }}
        </b-table-column>
      </template>
    </b-table>
  </div>
</template>

<script>
    import { mapGetters } from 'vuex';
  import CreateSourceModal from '@/components/modals/create-source-modal';
  import SourceService from "../../services/SourceService";
  import SourceTypeService from "../../services/SourceTypeService";

    export default {
        name: "source-index",
        components: { CreateSourceModal },
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
            createdNewSource: function (value) {
                if (value) {
                    let self = this;
                    self.dataLoading = true;
                    SourceService.getAll()
                        .then(function (res) {
                            self.data = res;

                            self.dataLoading = false;
                        });
                }
            }
        },
        mounted: function() {
            let self = this;
            SourceService.getAll()
                .then(function (res) {
                    self.data = res;

                    self.dataLoading = false;
                });

            SourceTypeService.getAll()
                .then(function (res) {
                    self.typeData = res;
                })
        }
    }
</script>

<style scoped>

</style>
