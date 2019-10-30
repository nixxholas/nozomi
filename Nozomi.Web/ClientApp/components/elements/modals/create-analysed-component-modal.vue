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
            <p class="modal-card-title">Create an analysed component</p>
          </header>
          <section class="modal-card-body">
            <b-field label="Type">
              <b-select placeholder="Pick one!" v-model="form.type">
                <option
                  v-for="option in componentTypes"
                  :value="option.value"
                  :key="option.key">
                  {{ option.key }}
                </option>
              </b-select>
            </b-field>

            <b-field label="UI Formatting">
              <b-input
                type="text"
                placeholder=""
                v-model="form.uiFormatting"
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
                v-model="form.delay"
                expanded
                required>
              </b-input>
            </b-field>

            <b-field grouped>
              <b-field label="Denominated Value">
                <b-switch v-model="form.isDenominated"
                          true-value="Yes"
                          false-value="No">
                  {{ form.isDenominated }}
                </b-switch>
              </b-field>
              <b-field label="Stash Historical">
                <b-switch v-model="form.storeHistoricals"
                          true-value="Yes"
                          false-value="No">
                  {{ form.storeHistoricals }}
                </b-switch>
              </b-field>
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
    import store from '../../../store/index';
    import { mapActions } from 'vuex';
    import { NotificationProgrammatic as Notification } from 'buefy';

    export default {
        name: "create-ac-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            currencyId: 0,
            currencyPairId : 0,
            currencyTypeId: 0
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            create: function() {
                this.isModalLoading = true;

                let self = this;
                this.$axios.post('/api/AnalysedComponent/Create', self.form, {
                    headers: {
                        Authorization: "Bearer " + store.state.oidcStore.access_token
                    }
                })
                    .then(function (response) {
                        // Reset the form data regardless
                        self.form = {
                            type: 0,
                            uiFormatting: "",
                            isDenominated: false,
                            storeHistoricals: false,
                            currencyId: 0,
                            currencyPairId : 0,
                            currencyTypeId: 0
                        };

                        if (response.status === 200) {
                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Component successfully created!`,
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
            self.componentTypesIsLoading = true;
            this.$axios.get('/api/AnalysedComponent/AllTypes', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    self.componentTypes = response.data.data.value;
                })
                .catch(function (error) {
                    // handle error
                    self.methods.authenticateOidc(self.currentRoute);
                })
                .finally(function () {
                    // always executed
                    self.componentTypesIsLoading = false;
                });
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                form: {
                    type: 0,
                    uiFormatting: "",
                    isDenominated: false,
                    storeHistoricals: false,
                    currencyId: this.currencyId,
                    currencyPairId : this.currencyPairId,
                    currencyTypeId: this.currencyTypeId
                },
                componentTypes: [],
                componentTypesIsLoading: false
            }
        }
    }
</script>

<style scoped>

</style>
