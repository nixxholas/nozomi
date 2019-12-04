<template>
    <div class="container">        
        <section class="hero" v-if="this.data">
            <div class="hero-body">
                    <b-navbar class="pb-4" 
                              spaced
                              :mobile-burger="false">
                        <template slot="brand">
                            <b-navbar-item tag="div" class="level-item">
                                <img
                                        class="image is-64x64 mr-3"
                                        :src="'/' + data.logoPath"
                                        alt="Currency"
                                        v-if="data && data.logoPath"
                                >
                                <p class="title is-4">{{ data.name }}</p>
                            </b-navbar-item>
                        </template>
                        <template slot="start">
                        </template>

                        <template slot="end">
                            <b-navbar-item tag="div">

                            </b-navbar-item>
                        </template>
                    </b-navbar>
                
                <div class="tile is-ancestor box container">
                    <div class="tile is-parent">
                        <div class="tile is-child is-vertical">
                            <b-tabs @change="onTabChange" type="is-toggle" v-model="activeTab">
                                <b-tab-item label="Information">
                                    <section class="section" v-if="data.description !== null">
                                        <div class="container">
                                            <b-message>
                                                {{ data.description }}
                                            </b-message>
                                        </div>
                                    </section>
                                    
                                    <section class="section"
                                             v-if="(data && data.components && data.components.length > 0) || hasAccess">
                                        <div class="container">

                                            <div class="tile is-child" v-if="hasAccess">
                                                <p class="heading">Have a component to add?</p>
                                                <p class="is-4">
                                                    <CreateAcComponentModal :currency-id="data.id"></CreateAcComponentModal>
                                                </p>
                                            </div>
                                            
                                            <div class="columns is-multiline is-desktop" v-if="data && data.components && data.components.length > 0">
                                                <div class="column is-one-quarter" v-for="comp in data.components">
                                                    <div v-if="comp.value">
                                                        <p class="heading">{{ getTypeByKey(comp.type).value }}</p>
                                                        <p class="title is-4" v-if="!comp.uiFormatting">{{ comp.value }}</p>
                                                        <p class="title is-4" v-else>{{ comp.value | numeralFormat(comp.uiFormatting) }}</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                    
                                    <section class="section" v-if="data.properties && data.properties.length > 0">
                                        <div class="container">
                                            <h1 class="title is-4">Properties</h1>
                                            <div class="columns is-multiline is-mobile">
                                                <div class="column is-one-quarter" v-for="property in data.properties"
                                                     v-if="property.type !== null">
                                                    <p class="heading">{{ property.type }}</p>
                                                    <p class="title is-6">{{ property.value }}</p>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </b-tab-item>

                                <b-tab-item label="Chart" :visible="series && series.data && series.data.length > 0">
                                    <div class="chart" ref="chart"></div>
                                </b-tab-item>

                                <b-tab-item label="Markets">
                                    <b-table
                                            :data="sources.data"
                                            :loading="sources.loading"

                                            :mobile-cards="true"
                                            :per-page="sources.perPage"
                                            :total="sources.dataCount"
                                            @page-change="loadMarketData"
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
                                                    <p>No market data yet.</p>
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
                                            @page-change="loadHistoricalData"
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
                                                    <p>No historical data yet.</p>
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
        <section class="hero is-large" v-else-if="!loading && loadFail">
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
    import AnalysedHistoricItemService from "@/services/AnalysedHistoricItemService";
    import CurrencyService from "@/services/CurrencyService";
    import SourceService from '@/services/SourceService';
    import Converter from '@/helpers/converter';

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
        beforeMount: function () {
            let self = this;
            self.loading = true;

            try {
                AnalysedComponentService.getTypes()
                    .then(function (res) {
                        self.typeData = res;
                    });

                // Propagate the currency first
                this.$axios.get('/api/Currency/Get/' + this.slug)
                    .then(function (res) {
                        self.data = res.data;

                        self.loading = false;
                    })
                    .catch(function (err) {
                        self.loadFail = true;
                    });

                if (self.data) {
                    if (!self.data || !self.data.slug) {
                        self.data = null;
                        self.loading = false;
                        return;
                    }

                    // Obtain the currency's sources count for sources pagination
                    SourceService.countByCurrency(this.data.slug)
                        .then(function (result) {
                            self.sources.dataCount = result
                        });
                    
                    // Obtain the currency's historical count for historical pagination
                    CurrencyService.getPairCount(this.data.slug)
                        .then(function (result) {
                            self.historic.dataCount = result;
                        });

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
            } catch (error) {
                console.error(error);
                this.data = null;
                this.total = 0;
                this.loading = false;
                throw error;
            }
        },
        // mounted: function () {
        // },
        methods: {
            getTypeByKey(key) {
                if (!key)
                    return null;

                for (let i = 0; i < this.typeData.length; i++) {
                    if (this.typeData[i].key === key)
                        return this.typeData[i];
                }

                return null;
            },
            getTypeByValue(value) {
                if (!value)
                    return null;

                for (let i = 0; i < this.typeData.length; i++) {
                    if (this.typeData[i].value === value)
                        return this.typeData[i];
                }

                return null;
            },
            async loadMarketData(page) {
                this.sources.loading = true;
                if (!page || page <= 0) // Bad logic, reset check
                    page = 1;

                try {
                    // First, check and make sure there's actually a source for this before proceeding.
                    this.sources.dataCount = await SourceService.countByCurrency(this.data.slug);
                    
                    if (!this.sources.dataCount || this.sources.dataCount <= 0) {
                        this.sources.loading = false;
                        return; // Don't even bother updating if the count is invalid or non-existent.
                    }
                    
                    // Obtain all of the currency's sources.
                    this.sources.data = await SourceService.listByCurrency(this.data.slug, (page - 1), this.sources.perPage);

                    this.sources.loading = false;
                } catch (error) {
                    console.error(error);
                    // TODO: Spawn a notification error.
                    this.sources.data = [];
                    this.sources.dataCount = 0;
                    this.sources.loading = false;
                    throw error;
                }
            },
            async loadHistoricalData(page) {
                this.historic.loading = true;
                if (!page || page <= 0) // Bad logic, reset check
                    page = 1;

                try {
                    let self = this;
                    // Sync the historic count first
                    CurrencyService.getPairCount(this.data.slug)
                        .then(function (res) {
                            if (!Converter.isInt(res))
                                self.historic.dataCount = 0;
                            else
                                self.historic.dataCount = res;
                        });

                    if (this.historic.dataCount <= 0) {
                        self.historic.loading = false;
                        return; // No items, obtain for what?
                    }
                    
                    let priceTypeKey = this.getTypeByValue("Price");
                    if (!priceTypeKey) {
                        self.historic.loading = false;
                        return; // No type named price is found
                    }

                    let priceComponent;
                    for (let i = 0; i < this.data.components.length; i++) {
                        if (this.data.components[i].type === priceTypeKey)
                            priceComponent = this.data.components[i];
                    }

                    if (priceComponent) {
                        AnalysedHistoricItemService.list(priceComponent.guid, this.historic.page, this.historic.perPage)
                            .then(function (res) {
                                if (!Array.isArray(res))
                                    self.historic.data = [];
                                else
                                    self.historic.data = res;
                            });
                    } else {
                        // Didn't have to load because there ain't any historic items..
                    }
                    
                    this.historic.loading = false;
                } catch (error) {
                    console.error(error);
                    this.historic.data = [];
                    this.historic.dataCount = 0;
                    this.historic.loading = false;
                    throw error;
                }
            },
            onTabChange(index) {
                switch (index) {
                    case 2:
                        if (!this.sources.data || this.sources.data.length === 0) {
                            this.loadMarketData();
                        }
                        break;
                    case 3:
                        if (!this.historic.data || this.historic.data.length === 0) {
                            this.loadHistoricalData();
                        }
                        break;
                }
            }
        },
        data() {
            return {
                activeTab: 0,
                isLoading: false,
                loadFail: false,
                data: {},
                sources: {
                    loading: false,
                    data: [],
                    dataCount: 0,
                    page: 1,
                    perPage: 50
                },
                historic: {
                    loading: false,
                    data: [],
                    dataCount: 0,
                    page: 1,
                    perPage: 50
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
                }],
                typeData: []
            }
        }
    }
</script>

<style scoped>

</style>
