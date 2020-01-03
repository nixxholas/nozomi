<template>
    <section>
        <b-autocomplete
                v-if="data && data.length > 0 && data[0] !== null"
                :data="data.filter(e => e.mainTicker && e.counterTicker)"
                v-model="currencyPairGuid"
                placeholder="e.g. EURUSD"
                :custom-formatter="getCurrencyPairTickerPairStr"
                :loading="isLoading"
                @select="option => selected = option">

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
        <b-message v-else>Oh no.. There aren't any currency pairs at the moment..
        </b-message>
    </section>
</template>

<script>
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