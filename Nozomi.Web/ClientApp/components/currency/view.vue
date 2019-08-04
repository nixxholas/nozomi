<template>
  <div>
    <section class="hero">
      <div class="hero-body">
        <nav class="level">
          <div class="level-left">
            <div class="level-item">
              <figure class="image is-64x64" v-if="data.logoPath != null">
                <img :src="'/' + data.logoPath"/>
              </figure>
            </div>
            <div class="level-item">
              <h1 class="title">
                {{ data.name }}
              </h1>
            </div>
            <div class="level-item">
              <h1 class="title">
                <b-tag type="is-info">{{ data.abbreviation }}</b-tag>
              </h1>
            </div>
          </div>

          <div class="level-right">
            <div class="level-item">
              <h1 class="subtitle">
                {{ data.averagePrice | numeralFormat('$0[.]00') }}
              </h1>
            </div>
          </div>
        </nav>
        <div class="tile is-ancestor notification">
          <div class="tile is-vertical is-3">
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
          </div>
          <div class="tile is-parent">
            <div class="tile is-vertical">
              <div class="tile is-child">
                <b-tabs type="is-toggle" v-model="activeTab">
                  <b-tab-item label="Chart">
                    <div class="chart" ref="chart"></div>
                  </b-tab-item>

                  <b-tab-item label="Markets">
                    <b-table
                      :data="marketData"
                      :loading="isMarketDataLoading"

                      paginated
                      backend-pagination
                      :total="totalMarketDataCount"
                      :per-page="perPage"
                      @page-change="onMarketDataPageChange"
                      aria-next-label="Next page"
                      aria-previous-label="Previous page"
                      aria-page-label="Page"
                      aria-current-label="Current page">
                      <template slot-scope="props">
                        <b-table-column field="name" label="Name" sortable>
                          {{ props.row.name }}
                        </b-table-column>
                        <b-table-column field="abbreviation" label="Abbreviation" sortable>
                          {{ props.row.abbreviation }}
                        </b-table-column>
                      </template>
                    </b-table>
                  </b-tab-item>

                  <b-tab-item label="Historical Data">
                    Soon
                  </b-tab-item>
                </b-tabs>
              </div>
            </div>
          </div>
          <h2 class="subtitle" v-if="data.description != null">
          </h2>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
  import { createChart, isBusinessDay } from 'lightweight-charts';

  export default {
    props: ['slug'],
    beforeMount: async function () {
      this.loading = true;

      try {
        const response = await this.$axios.get('/api/Currency/Detailed/' + this.slug);

        this.data = response.data.data;

        if (response.data.data.averagePriceHistory !== null) {
          this.series[0].data = response.data.data.averagePriceHistory;

          // Chart setup
          let chart = createChart(this.$refs.chart, {
            width: this.$refs.chart.offsetWidth,
            height: this.$refs.chart.offsetHeight
          });
          let areaSeries = chart.addAreaSeries();
          areaSeries.setData(this.series[0].data);
          chart.timeScale().fitContent();

          // Chart watermarking
          chart.applyOptions({
            priceScale: {
              autoScale: false,
              scaleMargins: {
                top: 0.1,
                bottom: 0.1,
              },
            },
            timeScale: {
              lockVisibleTimeRangeOnResize: true,
              rightBarStaysOnScroll: true,
              borderVisible: false,
              borderColor: '#fff000',
              timeVisible: true,
              secondsVisible: false,
            },
            watermark: {
              color: 'black',
              visible: true,
              text: '7 Day Chart',
              fontSize: 16,
              horzAlign: 'left',
              vertAlign: 'bottom',
            },
          });
        }

        this.loading = false;
      } catch (error) {
        console.error(error);
        this.data = [];
        this.total = 0;
        this.loading = false;
        throw error;
      }
    },
    mounted: function () {
      this.loadMarketData();
    },
    methods: {
      async loadMarketData() {
        this.loading = true;

        try {
          const response = await this.$axios.get('/api/Source/GetCurrencySources/BTC?page=' + (this.marketDataPage - 1));

          this.marketData = response.data.data;
          this.isMarketDataLoading = false;
        } catch (error) {
          console.error(error);
          this.marketData = [];
          this.totalMarketDataCount = 0;
          this.isMarketDataLoading = false;
          throw error;
        }
      },
      onMarketDataPageChange(page) {
        this.marketDataPage = page;
        this.loadData()
      }
    },
    data () {
      return {
        activeTab: 0,
        isLoading: false,
        data: {},
        isMarketDataLoading: false,
        marketDataPage: 1,
        marketData: [],
        totalMarketDataCount: 0,
        perPage: 20,
        // Chart data
        options: {
          chart: {
            id: 'price-chart'
          },
          // xaxis: {
          //   categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998]
          // }
        },
        series: [{
          name: 'Price',
          data: []
        }]
      }
    }
  }
</script>

<style scoped>

</style>
