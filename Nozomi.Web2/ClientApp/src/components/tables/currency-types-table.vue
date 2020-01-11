<template>
  <b-table
    :data="data"
    :loading="dataLoading">

    <template slot-scope="props">
      <b-table-column field="typeShortForm" label="Abbreviation" width="40">
        {{ props.row.typeShortForm }}
      </b-table-column>

      <b-table-column field="name" label="Name">
        {{ props.row.name }}
      </b-table-column>
      
      <b-table-column label="">
        <CurrencyTypeModal :currency-type="props.row" @created="update" />
      </b-table-column>
    </template>

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
  </b-table>
</template>

<script>
  import CurrencyTypeModal from "@/components/modals/currency-type-modal";
  import CurrencyTypeService from "@/services/CurrencyTypeService";

    export default {
        name: "currency-types-table",
      components: { CurrencyTypeModal },
        data() {
            return {
                dataLoading: true,
                data: []
            }
        },
        beforeMount: function() {
            let self = this;

            CurrencyTypeService.all()
                .then(function(res) {
                    self.data = res.data;

                    self.dataLoading = false;
                });
        },
      methods: {
        update: function() {
          let self = this;

          CurrencyTypeService.all()
                  .then(function(res) {
                    self.data = res.data;

                    self.dataLoading = false;
                  });
        }
      }
    }
</script>

<style scoped>

</style>
