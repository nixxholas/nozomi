<template>
  <div>
    <section class="hero">
      <div class="hero-body">
        <nav class="level">
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
        <div class="tile is-ancestor notification">
          <div class="tile is-vertical is-3">
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
            <div class="tile is-child">
              <p class="heading">Market Cap</p>
              <p class="title">{{ data.marketCap | numeralFormat('$0[.]00 a') }}</p>
            </div>
          </div>
          <div class="tile is-parent">
            <div class="tile is-vertical">
              <div class="tile is-child">
                <b-tabs type="is-toggle" v-model="activeTab">
                  <b-tab-item label="Chart">
                    <apexchart width="100%" type="line" :options="options" :series="series"></apexchart>
                  </b-tab-item>

                  <b-tab-item label="Markets">
                    Soon
                  </b-tab-item>

                  <b-tab-item label="Historical Data">
                    Soon
                  </b-tab-item>
                </b-tabs>
              </div>
            </div>
          </div>
          <h2 class="subtitle" v-if="data.description != null">
          </h2>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
  export default {
    props: ['slug'],
    beforeMount: async function () {
      this.loading = true;

      try {
        const response = await this.$axios.get('/api/Currency/Detailed/' + this.slug);
        console.log(response);

        this.data = response.data.data;
        this.loading = false;
      } catch (error) {
        console.error(error);
        this.data = [];
        this.total = 0;
        this.loading = false;
        throw error;
      }
    },
    created: function () {

    },
    data () {
      return {
        activeTab: 0,
        isLoading: false,
        data: {},
        // Chart data
        options: {
          chart: {
            id: 'vuechart-example'
          },
          xaxis: {
            categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998]
          }
        },
        series: [{
          name: 'series-1',
          data: [30, 40, 45, 50, 49, 60, 70, 91]
        }]
      }
    }
  }
</script>

<style scoped>

</style>
