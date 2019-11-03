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
            // TODO: Evaluate whether if this is still needed here or not
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
            loadCurrencyData(page = 1, type = "CRYPTO", sortType = "MarketCap", typesToTake = ["MarketCap"]) {
                let self = this;
                return new Promise((resolve, reject) => {
                    let result;
                    this.$axios.get('/api/Currency/All?' +
                        this.arrayToString("typesToTake", typesToTake), {
                        params: {
                            currencyType: type,
                            itemsPerIndex: 50,
                            index: (page - 1),
                            sortType: sortType, // 1 = Market cap
                            orderDescending: true,
                            //typesToTake: this.arrayToString("typesToTake", [ "MarketCap" ]) // https://wsvincent.com/javascript-convert-array-to-string/
                        }
                    }).then(function (response) {
                        result = response.data;
                        resolve(result);
                    }).catch(function (error) {
                        reject(error);
                    });
                });
            },
            getCurrencyCount(type) {
                let self = this;
                return new Promise((resolve, reject) => {
                    let result;
                    this.$axios.get('/api/Currency/GetCountByType', {
                        params: {
                            currencyType: type,
                        }
                    }).then(function (response) {
                        result = response.data;
                        resolve(result);
                    }).catch(function (error) {
                        reject(error);
                    });
                });
            },
            onPageChange(page) {
                this.cryptoTable.loading = true;
                this.cryptoTable.page = page;

                let self = this;
                this.loadCurrencyData(page, "CRYPTO", "MarketCap", ["MarketCap", "CurrentAveragePrice",
                    "DailyVolume", "DailyPricePctChange"])
                    .then(function (result) {
                        self.cryptoTable.data = result;
                        self.cryptoTable.loading = false;

                        // Miscellaneous loading
                        self.getCurrencyCount("CRYPTO")
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
            },
            // Formats the array to the output shown below.
            // arrName[0]=1050&arrName[1]=2000
            arrayToString(arrName, arr) {
                let str = '';
                arr.forEach(function(element, index) {
                    str += arrName + "[" + index + "]=" + element;
                    if (index !== (arr.length - 1)) { // If the index is not beyond the array's size
                        str += '&'
                    }
                });
                return str;
            }
        },
        mounted() {
            let self = this;

            self.cryptoTable.loading = true;
            this.loadCurrencyData(self.cryptoTable.page, "CRYPTO", "MarketCap", ["MarketCap", "CurrentAveragePrice",
                "DailyVolume", "DailyPricePctChange"])
                .then(function (result) {
                    self.cryptoTable.data = result;
                    self.cryptoTable.loading = false;

                    // Miscellaneous loading
                    self.getCurrencyCount("CRYPTO")
                        .then(function (result) {
                            self.cryptoTable.total = result;
                        });
                });
            // this.fiatTable.data = this.loadCurrencyData("FIAT");
            this.loadFiatData();
        }

    }
</script>

<style>

</style>
