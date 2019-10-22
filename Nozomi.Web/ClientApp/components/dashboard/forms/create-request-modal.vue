<template>
  <div>
    <button class="button is-primary"
            @click="isCreateRequestModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isCreateRequestModalActive">
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">Create a request</p>
        </header>
        <section class="modal-card-body">
          <form action="">
            <RequestTypeDrowdown v-model="form.type"></RequestTypeDrowdown>
            <ResponseTypeDropdown v-model="form.responseType"></ResponseTypeDropdown>
            <b-field label="URL">
              <b-input
                type="url"
                placeholder="https://nozomi.one/api/Ticker/GetAllAsync/0"
                v-bind="form.apiUrl"
                required
                expanded>
              </b-input>
            </b-field>
            <b-field>
              <template slot="label">
                Delay <span class="has-text-primary is-italic">(In milliseconds)</span>
              </template>
              <b-input
                type="number"
                placeholder="Delay between each update in milliseconds"
                v-bind="form.delay"
                expanded
                required>
              </b-input>
            </b-field>
            <b-field>
              <template slot="label">
                Retry Delay <span class="has-text-primary is-italic">(In milliseconds)</span>
              </template>
              <b-input
                type="number"
                placeholder="Retry attempt delay in milliseconds"
                v-bind="form.failureDelay"
                required
                expanded>
              </b-input>
            </b-field>
            <b-tabs v-model="form.requestParentType" expanded class="has-text-dark">
              <b-tab-item label="Currency">
                <b-field>

                </b-field>
              </b-tab-item>
              <b-tab-item label="Currency Pair"></b-tab-item>
              <b-tab-item label="Currency Type"></b-tab-item>
            </b-tabs>
          </form>
        </section>
        <footer class="modal-card-foot">
          <button class="button" type="button" @click="isCreateRequestModalActive = false">Close</button>
          <button class="button is-primary">Submit</button>
        </footer>
      </div>
    </b-modal>
  </div>
</template>

<script>
    import RequestTypeDrowdown from '../../elements/request-type-dropdown';
    import ResponseTypeDropdown from "../../elements/response-type-dropdown";

    export default {
        name: "create-request-modal",
        components: {ResponseTypeDropdown, RequestTypeDrowdown},
        props: {
            // Nothing yet
        },
        beforeCreate: {

        },
        data: function () {
            return {
                isCreateRequestModalActive: false,
                form: {
                    activeTab: 0, // For requestParentType
                    type: 0,
                    responseType: 1,
                    apiUrl: '',
                    delay: 0,
                    failureDelay: 0,
                    requestParentType: 0
                },
                formHelper: {},
                currencies: []
            }
        }
    }
</script>

<style scoped>

</style>
