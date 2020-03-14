<template>
    <div>
        <h1 class="title">Currencies</h1>
        <p class="subtitle">We support any type of currencies</p>

        <b-field grouped group-multiline v-if="oidcIsAuthenticated">
            <div class="control">
                <CurrencyModel @created="newCurrencyCreated"/>
            </div>
        </b-field>

        <CurrencyTable :currency-last-updated="currencyLastUpdated" />
    </div>
</template>

<script>
    import {mapGetters} from 'vuex';
    import CurrencyModel from '@/components/modals/currency-modal';
    import CurrencyTable from '@/components/tables/currencies-table';

    export default {
        name: "currencies-component",
        components: {
            CurrencyModel,
            CurrencyTable
        },
        data() {
            return {
                currencyLastUpdated: null
            };
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated'
            ])
        },
        methods: {
            newCurrencyCreated(value) {
                if (value) {
                    this.currencyLastUpdated = Date.now();
                }
            }
        }
    }
</script>