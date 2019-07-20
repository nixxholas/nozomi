<template>
    <div>
      <b-table
        :data="data"
        :loading="loading"

        paginated
        backend-pagination
        :total="total"
        :per-page="perPage"
        @page-change="onPageChange"
        aria-next-label="Next page"
        aria-previous-label="Previous page"
        aria-page-label="Page"
        aria-current-label="Current page">

      </b-table>
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
    loadData() {
      this.loading = true;

      // Optionally the request above could also be done as
      this.$axios.get('/api/Currency/GetAllDetailed/' + (this.page - 1))
        .then(function (response) {
          console.dir(response);
        })
        .catch(function (error) {
          console.dir(error);
        })
        .then(function () {
          // always executed
        });
    },
    onPageChange(page) {
      this.page = page;
      this.loadData()
    }
  },
  mounted() {
    console.dir("Loading");
    this.loadData();
  }
}
</script>

<style>

</style>
