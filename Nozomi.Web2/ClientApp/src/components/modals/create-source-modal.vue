<template>
  <div>
    <button class="button is-primary"
            @click="isModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isModalActive">
      <b-loading :active.sync="isModalLoading" :can-cancel="false"></b-loading>
      <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
      <form v-on:submit.prevent="create()" class="has-text-justified">
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">Create a source</p>
          </header>
          <section class="modal-card-body">
            <b-field label="Type">
              <b-select placeholder="Pick one!" v-model="form.sourceType">
                <option
                  v-for="option in types"
                  :value="option.guid"
                  :key="option.name">
                  {{ option.name }}
                </option>
              </b-select>
            </b-field>

            <b-field>
              <template slot="label">
                Abbreviation
              </template>
              <b-input
                type="text"
                placeholder=""
                v-model="form.abbreviation"
                expanded>
              </b-input>
            </b-field>

            <b-field>
              <template slot="label">
                Name
              </template>
              <b-input
                type="text"
                placeholder=""
                v-model="form.name"
                expanded>
              </b-input>
            </b-field>

            <b-field>
              <template slot="label">
                API Documentation URL
              </template>
              <b-input
                type="url"
                placeholder=""
                v-model="form.apiDocsUrl"
                expanded>
              </b-input>
            </b-field>
          </section>

          <footer class="modal-card-foot">
            <button class="button" type="button" @click="isModalActive = false">Close</button>
            <button class="button is-primary" type="submit">Submit</button>
          </footer>
        </div>
      </form>
    </b-modal>
  </div>
</template>

<script>
    import store from '../../store/index';
    import {mapActions} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import SourceTypeService from "../../services/SourceTypeService";

    export default {
        name: "create-source-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            currencyId: Number,
            currencyPairId: Number,
            currencyTypeId: Number
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            create: function () {
                this.isModalLoading = true;

                let self = this;
                this.$axios.post('/api/Source/Create', self.form, {
                    headers: {
                        Authorization: "Bearer " + store.state.oidcStore.access_token
                    }
                })
                    .then(function (response) {
                        // Reset the form data regardless
                        self.form = {
                            sourceType: "",
                            abbreviation: "",
                            name: "",
                            apiDocsUrl: ""
                        };

                        if (response.status === 200) {
                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Source successfully created!`,
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });

                            // Inform the parent that a new request has been created
                            // https://forum.vuejs.org/t/passing-data-back-to-parent/1201
                            self.$emit('created', true);
                        }
                    })
                    .catch(function (error) {
                        //console.log(error);
                        Notification.open({
                            duration: 2500,
                            message: `Please make sure your entry is correctly filled!`,
                            position: 'is-bottom-right',
                            type: 'is-danger',
                            hasIcon: true
                        });
                    })
                    .finally(function () {
                        // always executed
                        self.isModalLoading = false;
                    });
            }
        },
        beforeCreate: function () {
            let self = this;

            // Synchronously call for data
            self.typesIsLoading = true;
            SourceTypeService.getAll()
                .then(function (res) {
                    self.types = res;
                    self.typesIsLoading = false;
                });
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                form: {
                    sourceType: "",
                    abbreviation: "",
                    name: "",
                    apiDocsUrl: ""
                },
                types: [],
                typesIsLoading: false
            }
        }
    }
</script>

<style scoped>

</style>
