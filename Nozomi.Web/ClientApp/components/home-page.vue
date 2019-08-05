<template>
    <div>
      <section class="hero is-medium is-bold">
        <div class="hero-body">
          <div class="container">
              <carousel class="tile is-ancestor" :autoplay="true" :paginationEnabled="false"
                        :perPage="1" :perPageCustom="[[768, 2]]">
                <slide class="tile is-parent" v-for="datum in currencyTypeData">
                  <article class="tile is-child" style="width: 100%">
                    <p class="title" v-if="datum.parentName">{{ datum.parentName + ' ' + datum.componentType }}</p>
                    <p class="title" v-else>{{datum.componentType }}</p>

                    <tv-lw-chart :payload="datum.historical" magnetTip="true" fit-content="true"
                                   :showTimeScale="false" :height="'30vh'" intradayData="true"
                                   :data-name="datum.componentType"></tv-lw-chart>
                  </article>
                </slide>
              </carousel>
          </div>
        </div>
        <b-loading :is-full-page="false" :active.sync="currencyTypeData.length === 0" :can-cancel="false"></b-loading>
      </section>

      <section class="section">
        <b-table
          :data="data"

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
                :data="props.row.averagePriceHistory" :radius="24"
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
        <b-loading :is-full-page="false" :active.sync="loading" :can-cancel="false"></b-loading>
      </section>
    </div>
</template>

<script>
import { Carousel, Slide } from 'vue-carousel';

export default {
  data () {
    return {
      data: [],
      currencyTypeData: [],
      currencyTypeLoading: false,
      total: 0,
      loading: false,
      page: 1,
      perPage: 20
    }
  },
  components: {
    Carousel,
    Slide
  },
  methods: {
    async loadData() {
      this.loading = true;
      this.currencyTypeLoading = true;

      try {
        // Load all currencies
        const currenciesResponse = await this.$axios.get('/api/Currency/GetAllDetailed/' + (this.page - 1));

        if (currenciesResponse.status === 200)
          this.data = currenciesResponse.data;

        this.loading = false;

        // Load Currency Type data
        const currencyTypesResponse = await this.$axios.get('/api/CurrencyType/GetAll/0');

        if (currencyTypesResponse.status === 200)
          this.currencyTypeData = currencyTypesResponse.data;

        this.currencyTypeLoading = false;
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
