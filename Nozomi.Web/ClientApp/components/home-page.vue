<template>
    <div>
      <section class="hero is-medium is-bold">
        <div class="hero-body">
          <div class="container">
            <div v-for="datum in currencyTypeData">
              <h1 class="title">
                {{ datum.componentType }}
              </h1>
              <h2 class="subtitle">
                <trend
                  :data="datum.historical"
                  :gradient="['#6fa8dc', '#42b983', '#2c3e50']"
                  auto-draw
                  smooth
                  v-if="datum.historical != null"
                />
              </h2>
            </div>
          </div>
        </div>
      </section>

      <section class="section">
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
              <router-link :to="`/currency/${props.row.slug}`">
                <img v-if="props.row.logoPath != null"
                     :src="props.row.logoPath" class="mr-1" style="width: 24px; height: 24px; vertical-align: bottom;"/>
                {{ props.row.name }}
              </router-link>
            </b-table-column>
            <b-table-column field="marketCap" label="Market Cap" sortable>
              {{ props.row.marketCap | numeralFormat('$0 a') }}
            </b-table-column>
            <b-table-column field="price" label="Price" sortable>
              {{ props.row.averagePrice | numeralFormat('$0[.]00') }}
            </b-table-column>
            <b-table-column field="chart" label="Trend" sortable>
              <trend
                :data="props.row.averagePriceHistory"
                :gradient="['#6fa8dc', '#42b983', '#2c3e50']"
                auto-draw
                smooth
                v-if="props.row.averagePriceHistory != null"
              >
              </trend>
              <b-tag type="is-danger" size="is-medium" v-else>No data available</b-tag>
            </b-table-column>
          </template>
        </b-table>
      </section>
    </div>
</template>

<script>
import { createChart } from 'lightweight-charts';

export default {
  data () {
    return {
      data: [],
      currencyTypeData: [],
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
        // Load all currencies
        const currenciesResponse = await this.$axios.get('/api/Currency/GetAllDetailed/' + (this.page - 1));

        if (currenciesResponse.status === 200)
          this.data = currenciesResponse.data;

        // Load Currency Type data
        const currencyTypesResponse = await this.$axios.get('/api/CurrencyType/GetAll/0');

        if (currencyTypesResponse.status === 200)
          this.currencyTypeData = currencyTypesResponse.data;

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
