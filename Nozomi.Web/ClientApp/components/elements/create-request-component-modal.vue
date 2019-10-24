<template>
  <div>
    <button class="button is-primary"
            @click="isCreateModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isCreateModalActive">
    </b-modal>
  </div>
</template>

<script>
    export default {
        name: "create-request-component-modal",
        data: function() {
            return {
                isCreateModalActive: false
            }
        }
    }
</script>

<style scoped>

</style>
