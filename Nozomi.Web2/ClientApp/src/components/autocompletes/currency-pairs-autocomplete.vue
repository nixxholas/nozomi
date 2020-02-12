<template>
    <section>
        <b-autocomplete
                :data="data.filter(e => e.mainTicker && e.counterTicker)"
                v-model="query"
                placeholder="e.g. EURUSD"
                :custom-formatter="getCurrencyPairTickerPairStr"
                :loading="isLoading"
                @select="option => selected ? selected = option : selected = null"
                @typing="getAsyncData"
                @infinite-scroll="getMoreAsyncData">
            <template slot-scope="props">
                <div class="media">
                    <div class="media-content">
                        {{ props.option.mainTicker + props.option.counterTicker }}
                        <br>
                        <small v-if="props.option.sourceGuid && getSource(props.option.sourceGuid)">
                            From <b><i>{{ getSource(props.option.sourceGuid).name }}</i></b>
                        </small>
                    </div>
                </div>
            </template>
        </b-autocomplete>
    </section>
</template>

<script>
    import CurrencyPairService from "@/services/CurrencyPairService";
    import SourceService from "@/services/SourceService";

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
                sources: []
            }
        },
        watch: {
            selected: function (newVal) {
                // Safetynet
                if (newVal && newVal.guid) {
                    // https://www.telerik.com/blogs/how-to-emit-data-in-vue-beyond-the-vuejs-documentation
                    this.$emit('input', newVal.guid); // Emit the value out for v-model support
                }
            }
        },
        methods: {
            getCurrencyPairTickerPairStr: function (obj) {
                if (obj && obj.mainTicker && obj.counterTicker && obj.sourceGuid) {
                    // Always set the final object for emission first
                    this.selected = obj;
                    
                    let source = this.getSource(obj.sourceGuid);

                    if (source)
                        return obj.mainTicker + obj.counterTicker + " from " + source.name;
                    else
                        return obj.mainTicker + obj.counterTicker;
                }

                // Reset everything if something is off
                this.selected = null;
                return '';
            },
            getAsyncData: function (query) {
                let self = this;
                self.isLoading = true;

                CurrencyPairService.search(query, this.page, 50)
                    .then(function (data) {
                        self.data = data;

                        // self.page++;
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
            },
            getSource: function(guid) {
                if (this.sources && this.sources.length > 0) {
                    return this.sources.filter(e => e.guid === guid)[0];
                }
                
                return null;
            }
        },
        created: function () {
            let self = this;

            // Synchronously call for data
            self.isLoading = true;
            CurrencyPairService.all()
                .then(function (response) {
                    self.data = response;
                    
                    if (self.incomingCurrencyPairGuid && response 
                        && response.length > 0) {
                        self.selected = response.filter(e => e.guid === self.incomingCurrencyPairGuid)[0];
                        
                        if (self.selected.source)
                            self.query = self.selected.mainTicker + self.selected.counterTicker + " from " + self.selected.source.name;
                        else
                            self.selected = self.selected.mainTicker + self.selected.counterTicker;
                    }
                })
                .catch(function (error) {
                    // handle error
                    self.methods.authenticateOidc(self.currentRoute);
                })
                .finally(function () {
                    // always executed
                    self.isLoading = false;
                });
        
            SourceService.getAll()
                .then(function (response) {
                    self.sources = response;
                })
        }
    }
</script>