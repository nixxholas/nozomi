<template>
    <div class="container">
        <h1 class="title">Demo</h1>
        <p class="subtitle">Explore a part of our services we offer</p>

        <div class="box">
            <b-tabs type="is-toggle-rounded" v-model="activeTab" expanded>
                
                <b-tab-item v-for="(tab, index) in tabs" :key="tab.name" :label="tab.name">
                    <keep-alive>
                        <component :is="tab.component" v-if="activeTab === index" />
                    </keep-alive>
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
                tabs: [
                    {
                        name: "Overview",
                        component: AggregatesComponent
                    },
                    {
                        name: "Currencies",
                        component: CurrenciesComponent
                    },
                    {
                        name: "Sources",
                        component: SourcesComponent
                    }
                ],
            };
        },
    }
</script>