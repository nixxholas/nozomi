<template>
  <div>
    <button v-if="sourceType != null"
            class="button is-warning"
            @click="isModalActive = true">
      Modify
    </button>
    <button v-else
            class="button is-primary"
            @click="isModalActive = true">
      Create
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isModalActive">
      <b-loading :active.sync="isModalLoading" :can-cancel="false" />
      <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
      <form v-on:submit.prevent="push()" class="has-text-justified">
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title" v-if="sourceType && form && form.name">
              Modify {{ form.name }}
            </p>
            <p class="modal-card-title" v-else>Create a source type</p>
          </header>
          <section class="modal-card-body">
            <b-field v-if="sourceType !== null">
              <b-input
                      type="hidden"
                      v-model="form.guid">
              </b-input>
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
        name: "source-type-modal",
        props: {
          sourceType: {
            type: Object,
            default: null
          },
          currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
        },
      beforeMount: function() {
          if (this.sourceType) {
            let sType = this.sourceType;
            
            if (sType.guid)
              this.form.guid = sType.guid;
            
            if (sType.abbreviation)
              this.form.abbreviation = sType.abbreviation;
            
            if (sType.name)
              this.form.name = sType.name;
          }
      },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            push: function () {
                this.isModalLoading = true;

                let self = this;
                this.$axios.post('/api/SourceType/Create', self.form, {
                    headers: {
                        Authorization: "Bearer " + store.state.oidcStore.access_token
                    }
                })
                    .then(function (response) {
                        // Reset the form data regardless
                        self.form = {
                            abbreviation: "",
                            name: ""
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
                        if (error && error.response) {
                          switch (error.response.status) {
                            case 401:
                            case 403:
                              Notification.open({
                                duration: 2500,
                                message: `You do not have the permissions for this!`,
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                              });
                              break;
                            case 408:
                              Notification.open({
                                duration: 2500,
                                message: `Oh no.. something is not right with our connection to the server!`,
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                              });
                              break;
                          }                      
                        } else {
                          Notification.open({
                            duration: 2500,
                            message: `Please make sure your entry is correctly filled!`,
                            position: 'is-bottom-right',
                            type: 'is-danger',
                            hasIcon: true
                          });
                        }
                    })
                    .finally(function () {
                        // always executed
                        self.isModalLoading = false;
                    });
            }
        },
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                form: {
                  guid: "",
                  abbreviation: "",
                  name: ""
                }
            }
        }
    }
</script>

<style scoped>

</style>
