<template>
    <div>

    </div>
</template>

<script>
export default {
  data () {
    return {
      data: [],
      total: 0,
      loading: false,
      page: 1,
      perPage: 20
    }
  },
  methods: {
    async loadData() {
      this.loading = true;

      // Optionally the request above could also be done as
      let result = await axios.get(
        '/user', {
        params: {
          ID: 12345
        }
      })
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        })
        .then(function () {
          // always executed
        });
    }
  }
}
</script>

<style>

</style>
