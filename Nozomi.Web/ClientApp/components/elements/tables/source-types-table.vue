<template>
  <b-table
    :data="data"
    :loading="dataLoading">

    <template slot-scope="props">
      <b-table-column field="abbreviation" label="Abbreviation" width="40">
        {{ props.row.abbreviation }}
      </b-table-column>

      <b-table-column field="name" label="Name">
        {{ props.row.name }}
      </b-table-column>
    </template>

    <template slot="empty">
      <section class="section">
        <div class="content has-text-grey has-text-centered">
          <p>
            <b-icon
              icon="emoticon-sad"
              size="is-large">
            </b-icon>
          </p>
          <p>Nothing here.</p>
        </div>
      </section>
    </template>
  </b-table>
</template>

<script>
  import SourceTypeService from "../../../services/SourceTypeService";

    export default {
        name: "source-types-table",
        data() {
            return {
                dataLoading: true,
                data: []
            }
        },
        beforeMount: function() {
            let self = this;

            SourceTypeService.getAll()
                .then(function(res) {
                    self.data = res;

                    self.dataLoading = false;
                });
        }
    }
</script>

<style scoped>

</style>
