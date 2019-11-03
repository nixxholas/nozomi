<template>
  <div>
    <section class="hero is-medium is-bold"
             v-if="currencyTypeTable !== null && currencyTypeTable.data !== null && currencyTypeTable.data.length > 0">
      <div class="hero-body">
        <div class="container">
          <Carousel class="tile is-ancestor" :autoplay="true" :paginationEnabled="false"
                    :perPage="1" :perPageCustom="[[768, 2]]">
            <Slide class="tile is-parent" v-for="datum in currencyTypeTable.data"
                   v-if="datum.historical != null && datum.count > 0">
              <article class="tile is-child" style="width: 100%">
                <p class="title" v-if="datum.parentName">{{ datum.parentName + ' ' + datum.componentType }}</p>
                <p class="title" v-else>{{datum.componentType }}</p>

                <tv-lw-chart :payload="datum.historical" magnetTip="true" fit-content="true"
                             :showTimeScale="false" :height="'30vh'" intradayData="true"
                             :data-name="datum.componentType"></tv-lw-chart>
              </article>
            </Slide>
          </Carousel>
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
                       :src="props.row.logoPath" class="mr-1"
                       style="width: 24px; height: 24px; vertical-align: bottom;"/>
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
                {{ props.row.dailyAvgPctChange | numeralFormat('0[.]0') }}%
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
                  <p>No Crypto data available yet.</p>
                </div>
              </section>
            </template>
            <template slot-scope="props">
              <b-table-column field="name" label="Name" sortable>
                <router-link :to="`/currency/${props.row.slug}`">
                  <img v-if="props.row.logoPath != null"
                       :src="props.row.logoPath" class="mr-1"
                       style="width: 24px; height: 24px; vertical-align: bottom;"/>
                  {{ props.row.name }}
                </router-link>
              </b-table-column>
              <b-table-column field="marketCap" label="Market Cap" sortable>
                {{ getComponentValue(props.row.components, 1) | numeralFormat('$0 a') }}
              </b-table-column>
              <b-table-column field="price" label="Price" sortable>
                {{ getComponentValue(props.row.components, 10) | numeralFormat('$0[.]00') }}
              </b-table-column>
              <b-table-column field="volume" label="Volume" sortable>
                {{ getComponentValue(props.row.components, 80) | numeralFormat('$0[.]00') }}
              </b-table-column>
              <b-table-column field="dailyAvgPctChange" label="Change" sortable>
                {{ getComponentValue(props.row.components, 70) | numeralFormat('0[.]0') }}%
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
                <b-tag type="is-danger" size="is-small" v-else>Trends are currently disabled!</b-tag>
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
  import CurrencyService from "../services/CurrencyService";
  import {Carousel, Slide} from 'vue-carousel';

    export default {
        data() {
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
                    perPage: 50
                },
                cryptoTable: {
                    data: [],
                    total: 0,
                    loading: false,
                    page: 1,
                    perPage: 50
                }
            }
        },
        components: {
            Carousel,
            Slide
        },
        methods: {
            onPageChange(page) {
                this.cryptoTable.loading = true;
                this.cryptoTable.page = page;

                let self = this;
                CurrencyService.getCurrencyData(page, this.cryptoTable.perPage, "CRYPTO", "MarketCap", ["MarketCap", "CurrentAveragePrice",
                    "DailyVolume", "DailyPricePctChange"], true)
                    .then(function (result) {
                        self.cryptoTable.data = result;
                        self.cryptoTable.loading = false;

                        // Miscellaneous loading
                        CurrencyService.getCurrencyCount("CRYPTO")
                            .then(function (result) {
                                self.cryptoTable.total = result;
                            });
                    });
            },
            getComponentValue(dataset, type) {
                if (dataset && dataset.length > 0) {
                    let res = dataset.filter(c => c.type === type);

                    if (res && res.length > 0) {
                        return res[0].value;
                    }
                }

                return null;
            }
        },
        mounted() {
            let self = this;

            self.cryptoTable.loading = true;
            CurrencyService.getCurrencyData(self.cryptoTable.page, self.cryptoTable.perPage, "CRYPTO", "MarketCap",
                ["MarketCap", "CurrentAveragePrice", "DailyVolume", "DailyPricePctChange"], true)
                .then(function (result) {
                    self.cryptoTable.data = result;
                    self.cryptoTable.loading = false;

                    // Miscellaneous loading
                    CurrencyService.getCurrencyCount("CRYPTO")
                        .then(function (result) {
                            self.cryptoTable.total = result;
                        });
                });

            self.fiatTable.loading = true;
            CurrencyService.getCurrencyData(self.fiatTable.page, self.fiatTable.perPage, "FIAT", "MarketCap",
                ["MarketCap", "CurrentAveragePrice", "DailyVolume", "DailyPricePctChange"], true)
                .then(function (result) {
                    self.fiatTable.data = result;
                    self.fiatTable.loading = false;

                    // Miscellaneous loading
                    CurrencyService.getCurrencyCount("FIAT")
                        .then(function (result) {
                            self.fiatTable.total = result;
                        });
                });
        }

    }
</script>

<style>

</style>
