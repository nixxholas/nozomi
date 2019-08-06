<template>
    <div>
      <section class="hero is-medium is-bold">
        <div class="hero-body">
          <div class="container">
              <carousel class="tile is-ancestor" :autoplay="true" :paginationEnabled="false"
                        :perPage="1" :perPageCustom="[[768, 2]]">
                <slide class="tile is-parent" v-for="datum in currencyTypeTable.data">
                  <article class="tile is-child" style="width: 100%">
                    <p class="title" v-if="datum.parentName">{{ datum.parentName + ' ' + datum.componentType }}</p>
                    <p class="title" v-else>{{datum.componentType }}</p>

                    <tv-lw-chart :payload="datum.historical" magnetTip="true" fit-content="true"
                                   :showTimeScale="false" :height="'30vh'" intradayData="true"
                                   :data-name="datum.componentType"></tv-lw-chart>
                  </article>
                </slide>
              </carousel>
              <b-loading :is-full-page="false" :active.sync="currencyTypeTable.loading" :can-cancel="false"></b-loading>
          </div>
        </div>
      </section>

      <section class="section">
        <b-tabs>
          <b-tab-item label="FIAT" icon="cash-usd">
            <b-table
              :data="fiatTable.data"

              paginated
              backend-pagination
              :total="fiatTable.total"
              :per-page="fiatTable.perPage"
              @page-change="onPageChange"
              aria-next-label="Next page"
              aria-previous-label="Previous page"
              aria-page-label="Page"
              aria-current-label="Current page">
              <template slot="empty">
                <section class="section">
                  <div class="content has-text-grey has-text-centered">
                    <p>
                      <b-icon
                        icon="emoticon-sad"
                        size="is-large">
                      </b-icon>
                    </p>
                    <p>No FIAT data yet.</p>
                  </div>
                </section>
              </template>
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
                <b-table-column field="volume" label="Volume" sortable>
                  {{ props.row.dailyVolume | numeralFormat('$0[.]00') }}
                </b-table-column>
                <b-table-column field="dailyAvgPctChange" label="Change" sortable>
                  {{ props.row.dailyAvgPctChange | numeralFormat('0[.]0%') }}
                </b-table-column>
                <b-table-column field="chart" label="Trend" sortable>
                  <trend
                    class="chart"
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
            <b-loading :is-full-page="false" :active.sync="fiatTable.loading" :can-cancel="false"></b-loading>
          </b-tab-item>
          <b-tab-item label="Cryptocurrency" icon="bitcoin">
            <b-table
              :data="cryptoTable.data"

              paginated
              backend-pagination
              :total="cryptoTable.total"
              :per-page="cryptoTable.perPage"
              @page-change="onPageChange"
              aria-next-label="Next page"
              aria-previous-label="Previous page"
              aria-page-label="Page"
              aria-current-label="Current page">
              <template slot="empty">
                <section class="section">
                  <div class="content has-text-grey has-text-centered">
                    <p>
                      <b-icon
                        icon="emoticon-sad"
                        size="is-large">
                      </b-icon>
                    </p>
                    <p>No FIAT data yet.</p>
                  </div>
                </section>
              </template>
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
                <b-table-column field="volume" label="Volume" sortable>
                  {{ props.row.dailyVolume | numeralFormat('$0[.]00') }}
                </b-table-column>
                <b-table-column field="dailyAvgPctChange" label="Change" sortable>
                  {{ props.row.dailyAvgPctChange | numeralFormat('0[.]0%') }}
                </b-table-column>
                <b-table-column field="chart" label="Trend">
                  <trend
                    class="trend"
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
            <b-loading :is-full-page="false" :active.sync="cryptoTable.loading" :can-cancel="false"></b-loading>
          </b-tab-item>
        </b-tabs>
      </section>
    </div>
</template>

<script>
import { Carousel, Slide } from 'vue-carousel';

export default {
  data () {
    return {
      currencyTypeTable: {
        loading: false,
        data: [],
      },
      fiatTable: {
        data: [],
        total: 0,
        loading: false,
        page: 1,
        perPage: 20
      },
      cryptoTable: {
        data: [],
        total: 0,
        loading: false,
        page: 1,
        perPage: 20
      }
    }
  },
  components: {
    Carousel,
    Slide
  },
  methods: {
    async loadData() {
      try {
        this.currencyTypeTable.loading = true;

        // Load Currency Type data
        const currencyTypesResponse = await this.$axios.get('/api/CurrencyType/GetAll/0');

        if (currencyTypesResponse.status === 200)
          this.currencyTypeTable.data = currencyTypesResponse.data;

        this.currencyTypeTable.loading = false;
      } catch (error) {
        console.error(error);
        this.data = [];
        this.total = 0;
        this.loading = false;
        throw error;
      }
    },
    async loadFiatData() {
      try {
        this.fiatTable.loading = true;

        // Load all currencies
        const currenciesResponse = await this.$axios.get('/api/Currency/GetAllDetailed/' + (this.fiatTable.page - 1)
          + '?currencyType=FIAT');

        if (currenciesResponse.status === 200)
          this.fiatTable.data = currenciesResponse.data;

        this.fiatTable.loading = false;
      } catch (e) {

      }
    },
    async loadCryptoData() {
      try {
        this.cryptoTable.loading = true;

        // Load all currencies
        const currenciesResponse = await this.$axios.get('/api/Currency/GetAllDetailed/' + (this.cryptoTable.page - 1));

        if (currenciesResponse.status === 200)
          this.cryptoTable.data = currenciesResponse.data;

        this.cryptoTable.loading = false;
      } catch (e) {

      }
    },
    onPageChange(page) {
      this.cryptoTable.page = page;
      this.loadData();
    }
  },
  mounted() {
    this.loadData();
    this.loadFiatData();
    this.loadCryptoData();
  }

}
</script>

<style>

</style>
