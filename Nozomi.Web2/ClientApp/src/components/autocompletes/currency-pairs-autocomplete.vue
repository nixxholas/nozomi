<template>
    <section>
        <b-autocomplete
                :data="data.filter(e => e.mainTicker && e.counterTicker)"
                v-model="currencyPairGuid"
                placeholder="e.g. EURUSD"
                :custom-formatter="getCurrencyPairTickerPairStr"
                :loading="isLoading"
                @select="option => selected = option"
                @typing="getAsyncData"
                @infinite-scroll="getMoreAsyncData">

            <template slot-scope="props">
                <div class="media">
                    <div class="media-content">
                        {{ props.option.mainTicker + props.option.counterTicker }}
                        <br>
                        <small v-if="props.option.source">
                            From <b><i v-if="props.option.source.name">{{ props.option.source.name }}</i></b>
                        </small>
                    </div>
                </div>
            </template>
        </b-autocomplete>
    </section>
</template>

<script>
    import debounce from 'lodash/debounce';
    import CurrencyPairService from "@/services/CurrencyPairService";

export default {
    name: 'currency-pairs-autocomplete',
    props: {
        incomingCurrencyPairGuid: {
            type: String,
            default: null
        }
    },
    data: function () {
        return {
            isLoading: false,
            data: [],
            page: 0,
            query: '',
            selected: null,
            currencyPairGuid: null,
        }
    },
    watch: {
        selected: function (newVal) {
            // Safetynet
            if (newVal && newVal.guid) {
                this.currencyPairGuid = newVal.guid;

                // https://www.telerik.com/blogs/how-to-emit-data-in-vue-beyond-the-vuejs-documentation
                this.$emit('input', newVal.guid); // Emit the value out for v-model support
            }
        }
    },
    methods: {
        getCurrencyPairTickerPairStr: function (obj) {
            if (!obj)
                return '';

            return obj.mainTicker + obj.counterTicker + " from " + obj.source.name + "";
        },
        getAsyncData: function (query) {
            let self = this;
            self.isLoading = true;

            console.dir(query);
            CurrencyPairService.search(query, this.page, 50)
                .then(function (data) {
                    self.data = data;
                    
                    self.page++;
                    // this.totalPages = data.total_pages
                })
                .catch((error) => {
                    throw error;
                })
                .finally(() => {
                    self.isLoading = false;
                })
        },
        getMoreAsyncData: function () {
            this.getAsyncData(this.query);
        }
    },
    created: function () {
        let self = this;

        // Synchronously call for data
        self.isLoading = true;
        CurrencyPairService.all()
            .then(function (response) {
                self.data = response;
            })
            .catch(function (error) {
                // handle error
                self.methods.authenticateOidc(self.currentRoute);
            })
            .finally(function () {
                // always executed
                self.isLoading = false;
            });
    }
}
</script>