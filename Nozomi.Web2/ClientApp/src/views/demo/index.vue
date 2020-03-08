<template>
    <div class="container">
        <h1 class="title">Demo</h1>
        <p class="subtitle">Explore a part of our services we offer</p>

        <div class="box">
            <b-tabs type="is-toggle-rounded" v-model="activeTab" expanded>
                <b-tab-item label="Overview">
                    <AggregatesComponent/>
                </b-tab-item>

                <b-tab-item label="Currencies">
                    <CurrenciesComponent/>
                </b-tab-item>

                <b-tab-item label="Sources">
                    <SourcesComponent/>
                </b-tab-item>
            </b-tabs>
        </div>
    </div>
</template>

<script>
    import AggregatesComponent from './Aggregates';
    import CurrenciesComponent from './Currencies';
    import SourcesComponent from './Sources';

    export default {
        components: {
            AggregatesComponent,
            CurrenciesComponent,
            SourcesComponent,
        },
        data: function () {
            return {
                activeTab: 0,
            };
        },
    }
</script>