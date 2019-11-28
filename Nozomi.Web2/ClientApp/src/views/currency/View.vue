<template>
    <div class="container">
        <section class="hero" v-if="data">
            <div class="hero-body">
                <div class="columns is-desktop is-multiline">
                    <div class="column">
                        <nav class="level is-full">
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
                    </div>

                    <div class="column is-full" v-if="data.description !== null">
                        <b-message>
                            {{ data.description }}
                        </b-message>
                    </div>
                </div>

                <div class="tile is-ancestor notification">
                    <div class="tile is-parent is-vertical is-3">
                        <div class="tile is-child" v-if="hasAccess">
                            <p class="heading">Have a component to add?</p>
                            <p class="is-4">
                                <CreateAcComponentModal :currency-id="data.id"></CreateAcComponentModal>
                            </p>
                        </div>
                        <div class="tile is-child">
                            <p class="heading">Market Cap</p>
                            <p class="title is-4">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
                        </div>
                        <div class="tile is-child" v-for="rComp in data.requestComponents">
                            <p class="heading">{{ rComp.name }}</p>
                            <p class="title is-4">{{ rComp.value }}</p>
                        </div>
                        <div class="tile is-child" v-for="property in data.properties" v-if="property.type !== null">
                            <p class="heading">{{ property.type }}</p>
                            <p class="title is-6">{{ property.value }}</p>
                        </div>
                    </div>
                    <div class="tile is-parent">
                        <div class="tile is-child is-vertical">
                            <b-tabs @change="onTabChange" type="is-toggle" v-model="activeTab">
                                <b-tab-item label="Chart">
                                    <div class="chart" ref="chart"></div>
                                </b-tab-item>

                                <b-tab-item label="Markets">
                                    <b-table
                                            :data="marketData"
                                            :loading="isMarketDataLoading"

                                            :mobile-cards="true"
                                            :per-page="perPage"
                                            :total="totalMarketDataCount"
                                            @page-change="onMarketDataPageChange"
                                            aria-current-label="Current page"
                                            aria-next-label="Next page"
                                            aria-page-label="Page"
                                            aria-previous-label="Previous page"
                                            backend-pagination
                                            paginated>
                                        <template slot="empty">
                                            <section class="section">
                                                <div class="content has-text-grey has-text-centered">
                                                    <p>
                                                        <b-icon
                                                                icon="frown"
                                                                size="is-large">
                                                        </b-icon>
                                                    </p>
                                                    <p>No FIAT data yet.</p>
                                                </div>
                                            </section>
                                        </template>
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
                                    <b-table
                                            :data="historic.data"

                                            :mobile-cards="true"
                                            :per-page="historic.perPage"
                                            :total="historic.dataCount"
                                            @page-change="onHistoricalDataPageChange"
                                            aria-current-label="Current page"
                                            aria-next-label="Next page"
                                            aria-page-label="Page"
                                            aria-previous-label="Previous page"
                                            backend-pagination
                                            paginated>
                                        <template slot="empty">
                                            <section class="section">
                                                <div class="content has-text-grey has-text-centered">
                                                    <p>
                                                        <b-icon
                                                                icon="frown"
                                                                size="is-large">
                                                        </b-icon>
                                                    </p>
                                                    <p>No FIAT data yet.</p>
                                                </div>
                                            </section>
                                        </template>
                                        <template slot-scope="props">
                                            <b-table-column field="time" label="Timestamp" sortable>
                                                {{ $moment.unix(props.row.time).format('MMMM Do YYYY, h:mm:ss a') }}
                                            </b-table-column>
                                            <b-table-column field="price" label="Price">
                                                {{ props.row.value | numeralFormat('$0[.]00') }}
                                            </b-table-column>
                                        </template>
                                    </b-table>
                                    <b-loading :active.sync="historic.loading" :can-cancel="false"
                                               :is-full-page="false"></b-loading>
                                </b-tab-item>
                            </b-tabs>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="hero is-large" v-else>
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">
                        Damn.. Something's wrong
                    </h1>
                    <h2 class="subtitle">
                        Seems like the currency you're looking for doesn't exist..
                    </h2>
                </div>
            </div>
        </section>
        <b-loading :active.sync="loading" :can-cancel="false" :is-full-page="false"></b-loading>
    </div>
</template>

<script>
    import {createChart} from 'lightweight-charts';
    import {mapGetters} from 'vuex';
    import CreateAcComponentModal from '@/components/modals/create-analysed-component-modal';
    import AnalysedComponentService from "@/services/AnalysedComponentService";

    export default {
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcIdTokenExp'
            ]),
            hasAccess: function () {
                return this.oidcIsAuthenticated
            },
        },
        props: ['slug'],
        components: {CreateAcComponentModal},
        beforeMount: async function () {
            this.loading = true;

            try {
                console.dir(AnalysedComponentService.getTypes());
                
                const response = await this.$axios.get('/api/Currency/Get/' + this.slug);

                if (response) {
                    this.data = response.data;
                    //this.series[0].data = response.data.data.averagePriceHistory;

                    // Chart setup
                    // let chart = createChart(this.$refs.chart, {
                    //     width: this.$refs.chart.offsetWidth,
                    //     height: this.$refs.chart.offsetHeight
                    // });
                    // let areaSeries = chart.addAreaSeries();
                    // areaSeries.setData(this.series[0].data);
                    // chart.timeScale().fitContent();

                    // Chart watermarking
                    // chart.applyOptions({
                    //     priceScale: {
                    //         autoScale: false,
                    //         scaleMargins: {
                    //             top: 0.1,
                    //             bottom: 0.1,
                    //         },
                    //     },
                    //     timeScale: {
                    //         lockVisibleTimeRangeOnResize: true,
                    //         rightBarStaysOnScroll: true,
                    //         borderVisible: false,
                    //         borderColor: '#fff000',
                    //         timeVisible: true,
                    //         secondsVisible: false,
                    //     },
                    //     watermark: {
                    //         color: 'black',
                    //         visible: true,
                    //         text: '7 Day Chart',
                    //         fontSize: 16,
                    //         horzAlign: 'left',
                    //         vertAlign: 'bottom',
                    //     },
                    // });
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
        // mounted: function () {
        // },
        methods: {
            async loadMarketData() {
                this.isMarketDataLoading = true;

                try {
                    const response = await this.$axios.get('/api/Source/GetCurrencySources/' + this.data.slug
                        + '?page=' + (this.marketDataPage - 1));

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
            async loadHistoricalData() {
                this.historic.loading = true;

                try {
                    const response = await this.$axios.get('/api/Currency/Historical/' + this.data.slug + '/'
                        + (this.historic.page - 1) + '/' + this.historic.perPage);

                    this.historic.perPage = response.data.elementsPerPage;
                    this.historic.data = response.data.data;
                    this.historic.dataCount = response.data.pages * this.historic.perPage;
                    this.historic.loading = false;
                } catch (error) {
                    console.error(error);
                    this.historic.data = [];
                    this.historic.dataCount = 0;
                    this.historic.loading = false;
                    throw error;
                }
            },
            onMarketDataPageChange(page) {
                this.marketDataPage = page;
                this.loadMarketData()
            },
            onHistoricalDataPageChange(page) {
                this.historic.page = page;
                this.loadHistoricalData();
            },
            onTabChange(index) {
                switch (index) {
                    case 1:
                        this.loadMarketData();
                        break;
                    case 2:
                        this.loadHistoricalData();
                        break;
                }
            }
        },
        data() {
            return {
                activeTab: 0,
                isLoading: false,
                data: {},
                isMarketDataLoading: false,
                marketDataPage: 1,
                marketData: [],
                totalMarketDataCount: 0,
                perPage: 20,
                historic: {
                    loading: false,
                    data: [],
                    dataCount: 0,
                    page: 1,
                    perPage: 25
                },
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
