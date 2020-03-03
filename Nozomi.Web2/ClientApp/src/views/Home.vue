<template>
  <div>
    <section class="hero is-medium is-bold"
             v-if="currencyTypeTable !== null && currencyTypeTable.data !== null && currencyTypeTable.data.length > 0">
      <div class="hero-body">
        <div class="container">
<!--          <Carousel class="tile is-ancestor" :autoplay="true" :paginationEnabled="false"-->
<!--                    :perPage="1" :perPageCustom="[[768, 2]]">-->
<!--            <Slide class="tile is-parent" v-for="datum in currencyTypeTable.data"-->
<!--                   v-if="datum.historical != null && datum.count > 0">-->
<!--              <article class="tile is-child" style="width: 100%">-->
<!--                <p class="title" v-if="datum.parentName">{{ datum.parentName + ' ' + datum.componentType }}</p>-->
<!--                <p class="title" v-else>{{datum.componentType }}</p>-->

<!--                <tv-lw-chart :payload="datum.historical" magnetTip="true" fit-content="true"-->
<!--                             :showTimeScale="false" :height="'30vh'" intradayData="true"-->
<!--                             :data-name="datum.componentType"></tv-lw-chart>-->
<!--              </article>-->
<!--            </Slide>-->
<!--          </Carousel>-->
        </div>
      </div>
    </section>

    <section class="section">
<!--      <b-tabs>-->
<!--        <b-tab-item label="FIAT" icon="money-bill-wave">-->
<!--          <CurrencyTable type="FIAT Cash" />-->
<!--        </b-tab-item>-->
<!--        <b-tab-item label="Cryptocurrency" icon-pack="fab" icon="bitcoin">-->
<!--          <CurrencyTable type="Cryptocurrency" />-->
<!--        </b-tab-item>-->
<!--      </b-tabs>-->
        <b-tabs v-if="currencyTypes && currencyTypes.length > 0" v-model="activeTab">
            <b-tab-item v-for="(currencyType, index) in currencyTypes" 
                    :label="currencyType.name">
                <CurrencyTable :type="currencyType.name" :isActive="activeTab === index" />
            </b-tab-item>
        </b-tabs>
    </section>
  </div>
</template>

<script>
    import {mapGetters} from 'vuex';
    import CoreService from "@/services/CoreService";
    import CurrencyService from "@/services/CurrencyService";
    import ComponentService from "@/services/ComponentService";
    import CurrencyTable from '@/components/tables/currencies-table';
    import CurrencyTypeService from "@/services/CurrencyTypeService";
    // import {Carousel, Slide} from 'vue-carousel';

    export default {
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated'
            ]),
        },
        data() {
            return {
                activeTab: 0,
                currencyTypeTable: {
                    loading: false,
                    data: []
                },
                currencyTypes: []
            }
        },
        components: {
          CurrencyTable
            //Carousel,
            //Slide
        },
        methods: {
            onPageChange(page) {
            },
            getComponentValue(dataset, type) {
                return ComponentService.getComponentValue(dataset, type);
            }
        },
        mounted() {
            let self = this;

            // if (this.oidcIsAuthenticated)
            //     CoreService.getUserDetails()
            //         .then(function (result) {
            //             console.dir(result);
            //         });
            
            CurrencyTypeService.all()
            .then(function(res) {
                self.currencyTypes = res.data;
            })
        }

    }
</script>

<style>

</style>
