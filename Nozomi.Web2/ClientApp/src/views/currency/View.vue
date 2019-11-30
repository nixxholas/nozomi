<template>
    <div class="container">
        <section class="hero" v-if="this.data">
            <div class="hero-body">
                <div class="tile is-ancestor">
                    <div class="tile is-parent is-vertical is-3">
                        <div class="tile is-child">
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
                            </div>
                        </div>

                        <!--                        <div class="level-right">-->
                        <!--                            <div class="level-item">-->
                        <!--                                <h1 class="subtitle">-->
                        <!--                                    {{ data.averagePrice | numeralFormat('$0[.]00') }}-->
                        <!--                                </h1>-->
                        <!--                            </div>-->
                        <!--                        </div>-->

                        <div class="tile is-child" v-if="data.description !== null">
                            <b-message>
                                {{ data.description }}
                            </b-message>
                        </div>

                        <div class="tile is-child" v-if="hasAccess">
                            <p class="heading">Have a component to add?</p>
                            <p class="is-4">
                                <CreateAcComponentModal :currency-id="data.id"></CreateAcComponentModal>
                            </p>
                        </div>

                        <!--                        <div class="tile is-child">-->
                        <!--                            <p class="heading">Market Cap</p>-->
                        <!--                            <p class="title is-4">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>-->
                        <!--                        </div>-->

                        <div class="tile is-child"
                             v-if="data && data.components && data.components.length > 0"
                             v-for="comp in data.components">
                            <div v-if="comp.value">
                                <p class="heading">{{ getTypeByKey(comp.type) }}</p>
                                <p class="title is-4" v-if="!comp.uiFormatting">{{ comp.value }}</p>
                                <p class="title is-4" v-else>{{ comp.value | numeralFormat(comp.uiFormatting) }}</p>
                            </div>
                        </div>
                    </div>
                    <div class="tile is-parent">
                        <div class="tile is-child is-vertical">
                            <b-tabs @change="onTabChange" type="is-toggle" v-model="activeTab">
                                <b-tab-item label="Information">
                                    <div class="tile is-child" v-for="property in data.properties"
                                         v-if="property.type !== null">
                                        <p class="heading">{{ property.type }}</p>
                                        <p class="title is-6">{{ property.value }}</p>
                                    </div>
                                </b-tab-item>

                                <b-tab-item label="Chart">
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
    import AnalysedHistoricItemService from "@/services/AnalysedHistoricItemService";
    import SourceService from '@/services/SourceService';

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
                this.typeData = await AnalysedComponentService.getTypes();

                // Propagate the currency first
                const response = await this.$axios.get('/api/Currency/Get/' + this.slug);

                if (response) {
                    this.data = response.data;

                    if (!this.data || !this.data.slug) {
                        this.data = null;
                        this.loading = false;
                        console.dir("Terminating further load..");
                        return;
                    }

                    // Then, obtain the currency's sources count for pagination
                    this.sources.dataCount = await SourceService.countByCurrency(this.data.slug);

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
            // TODO: Test
            async loadHistoricalData(page) {
                if (this.data.components && this.data.components.length > 0)
                    return;

                this.historic.loading = true;

                try {
                    let priceTypeKey = this.getTypeByValue("Price");
                    if (!priceTypeKey)
                        return; // No type named price is found

                    let priceComponent;
                    for (let i = 0; i < this.data.components.length; i++) {
                        if (this.data.components[i].type === priceTypeKey)
                            priceComponent = this.data.components[i];
                    }

                    if (priceComponent) {
                        this.historic.data = await AnalysedHistoricItemService.list(priceComponent.guid, this.historic.page, this.historic.perPage);
                    }

                    // const response = await this.$axios.get('/api/Currency/Historical/' + this.data.slug + '/'
                    //     + (this.historic.page - 1) + '/' + this.historic.perPage);
                    //
                    // this.historic.data = response.data.data;
                    // this.historic.dataCount = response.data.pages * this.historic.perPage;
                    this.historic.loading = false;
                } catch (error) {
                    console.error(error);
                    this.historic.data = [];
                    this.historic.dataCount = 0;
                    this.historic.loading = false;
                    throw error;
                }
            },
            onHistoricalDataPageChange(page) {
                console.dir("Current historic page: " + this.historic.page);
                // this.historic.page = page;
                this.loadHistoricalData(page);
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
