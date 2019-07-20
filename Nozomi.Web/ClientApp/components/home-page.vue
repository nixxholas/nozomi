<template>
    <div>
      <b-table
        :data="data"
        :loading="loading"

        paginated
        backend-pagination
        :total="total"
        :per-page="perPage"
        @page-change="onPageChange"
        aria-next-label="Next page"
        aria-previous-label="Previous page"
        aria-page-label="Page"
        aria-current-label="Current page">
        <template slot-scope="props">
          <b-table-column field="name" label="Name" sortable>
<!--            <img :src="props.row.logoPath" class="mr-1" style="width: 24px; height: 24px; vertical-align: bottom;"/>-->
            {{ props.row.name }}
          </b-table-column>
          <b-table-column field="marketCap" label="Market Cap" sortable>
            {{ props.row.marketCap | numeralFormat('$0 a') }}
          </b-table-column>
          <b-table-column field="price" label="Price" sortable>
            {{ props.row.averagePrice | numeralFormat('$0[.]00') }}
          </b-table-column>
          <b-table-column field="chart" label="Trend" sortable>
            {{props.row}}
            <trend
              :data="props.row.historical"
              :gradient="['#6fa8dc', '#42b983', '#2c3e50']"
              auto-draw
              smooth
              v-if="props.row.historical != null"
            >
            </trend>
            <b-tag type="is-danger" size="is-medium" v-else>No data available</b-tag>
          </b-table-column>
        </template>
      </b-table>
    </div>
</template>

<script>
export default {
  data () {
    return {
      data: [],
      total: 0,
      loading: false,
      page: 1,
      perPage: 20
    }
  },
  methods: {
    async loadData() {
      this.loading = true;

      try {
        const response = await this.$axios.get('/api/Currency/GetAllDetailed/' + (this.page - 1));
        console.log(response);

        this.data = response.data;
        this.loading = false;
      } catch (error) {
        console.error(error);
        this.data = [];
        this.total = 0;
        this.loading = false;
        throw error;
      }
    },
    onPageChange(page) {
      this.page = page;
      this.loadData()
    }
  },
  mounted() {
    this.loadData();
  }
}
</script>

<style>

</style>
