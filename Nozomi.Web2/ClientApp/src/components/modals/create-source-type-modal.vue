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
            <p class="modal-card-title">Create a source type</p>
          </header>
          <section class="modal-card-body">
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
        name: "create-source-type-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            create: function () {
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
        data: function () {
            return {
                isModalActive: false,
                isModalLoading: false,
                form: {
                    abbreviation: "",
                    name: ""
                }
            }
        }
    }
</script>

<style scoped>

</style>
