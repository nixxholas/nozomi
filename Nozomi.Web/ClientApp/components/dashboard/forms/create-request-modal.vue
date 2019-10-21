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
            </form>
          </section>
          <footer class="modal-card-foot">
            <button class="button" type="button" @click="$parent.close()">Close</button>
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
        components: { ResponseTypeDropdown, RequestTypeDrowdown },
        props: {
            // Nothing yet
        },
        data: function() {
            return {
                isCreateRequestModalActive: false,
                form: {
                    type: 0,
                    responseType: 0,
                    apiUrl: '',
                    delay: 0,
                    failureDelay: 0,
                },
            }
        }
    }
</script>

<style scoped>

</style>
