<template>
  <div>
    <section class="hero">
      <div class="hero-body">
        <nav class="level">
          <div class="level-left">
            <div class="level-item">
              <h1 class="title">
                {{ data.name }}
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
        <div class="container">
          <h2 class="subtitle" v-if="data.description != null">
            {{ data.description }}
          </h2>
        </div>
      </div>
    </section>

    {{ this.slug }}
    {{ this.data }}
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
    data () {
      return {
        isLoading: false,
        data: {}
      }
    }
  }
</script>

<style scoped>

</style>
