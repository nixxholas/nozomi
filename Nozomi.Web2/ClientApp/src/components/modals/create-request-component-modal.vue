<template>
  <div>
    <button class="button is-primary"
            @click="isModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isModalActive">
      <b-loading :active.sync="isModalLoading" :can-cancel="false" />
      <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
      <form v-on:submit.prevent="create()" class="has-text-justified">
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">Create a request component</p>
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

            <b-field label="Identifier">
              <b-input
                type="text"
                placeholder=""
                v-model="form.identifier"
                expanded>
              </b-input>
            </b-field>

            <b-field label="Query">
              <b-input
                type="text"
                placeholder=""
                v-model="form.queryComponent"
                expanded>
              </b-input>
            </b-field>

            <b-field grouped>
              <b-field label="Denominated Value">
                <b-switch v-model="form.isDenominated" />
              </b-field>
              <b-field label="Ignore Anomalies">
                <b-switch v-model="form.anomalyIgnorance" />
              </b-field>
              <b-field label="Stash Historical">
                <b-switch v-model="form.storeHistoricals" />
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
    import store from '../../store/index';
    import { mapActions } from 'vuex';
    import { NotificationProgrammatic as Notification } from 'buefy';
    import ComponentService from "@/services/ComponentService";
    import ComponentTypeService from "@/services/ComponentTypeService";

    export default {
        name: "create-rc-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            guid: ""
        },
      data: function () {
        return {
          isModalActive: false,
          isModalLoading: false,
          form: {
            type: 0,
            identifier: "",
            queryComponent: "",
            isDenominated: false,
            anomalyIgnorance: false,
            storeHistoricals: false,
            requestId: this.guid
          },
          componentTypes: [],
          componentTypesIsLoading: false
        }
      },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            create: function() {
                this.isModalLoading = true;
                let self = this;
                
                ComponentService.create(self.form)
                    .then(function (response) {
                        // Reset the form data regardless
                        self.form = {
                            type: 0,
                            identifier: "",
                            queryComponent: "",
                            isDenominated: false,
                            anomalyIgnorance: false,
                            storeHistoricals: false
                        };

                        if (response.status === 200) {
                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: `Request successfully created!`,
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
                        console.dir(error);
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
            ComponentTypeService.all()
                .then(function (response) {
                    self.componentTypes = response.data;
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
    }
</script>

<style scoped>

</style>
