<template>
  <div>
    <button class="button is-primary"
            @click="isModalActive = true" v-if="currency == null">
      Create
    </button>
    <button class="button is-warning"
            @click="isModalActive = true" v-else>
      Edit
    </button>

    <b-modal has-modal-card trap-focus :active.sync="isModalActive">
      <b-loading :active.sync="isModalLoading" :can-cancel="false"></b-loading>
      <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
      <form v-on:submit.prevent="create()" class="has-text-justified">
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">Create a currency</p>
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
                Slug
              </template>
              <b-input
                      type="text"
                      placeholder=""
                      v-model="form.slug"
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
                Denomination Name
              </template>
              <b-input
                      type="text"
                      placeholder=""
                      v-model="form.denominationName"
                      expanded>
              </b-input>
            </b-field>

            <b-field>
              <template slot="label">
                Denominations
              </template>
              <b-input
                      type="number"
                      placeholder=""
                      v-model="form.denominations"
                      expanded>
              </b-input>
            </b-field>

            <b-field>
              <template slot="label">
                Description
              </template>
              <b-input maxlength="300"
                       type="textarea"
                       v-model="form.description"></b-input>
            </b-field>

            <b-field>
              <template slot="label">
                Logo Path
              </template>
              <b-input
                      type="text"
                      placeholder=""
                      v-model="form.logoPath"
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
            <button class="button is-primary" type="submit" :disabled="!types && types.length <= 0">Submit</button>
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
    import CurrencyTypeService from "@/services/CurrencyTypeService";

    export default {
        name: "currency-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            currency: Object
        },
      data: function () {
        return {
          isModalActive: false,
          isModalLoading: false,
          form: {
            sourceType: "",
            abbreviation: "",
            slug: "",
            name: "",
            denominations: 0,
            denominationName: "",
            logoPath: "",
            description: "",
            apiDocsUrl: ""
          },
          types: [],
          typesIsLoading: false
        }
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
                          slug: "",
                          name: "",
                          denominations: 0,
                          denominationName: "",
                          logoPath: "",
                          description: "",
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
        mounted: function () {
            let self = this;

            // Synchronously call for data
            self.typesIsLoading = true;
            CurrencyTypeService.getAll()
                .then(function (res) {
                    self.types = res;
                    self.typesIsLoading = false;

                  // If currency isn't null, it means we're editing an existing one.
                  if (self.currency) {
                    self.form = self.currency; // Set first

                    // Update the source type
                    if (self.types && self.currency.currencyTypeGuid 
                            && self.types.filter(t => t.guid === self.currency.currencyTypeGuid).length > 0) {
                      self.form.sourceType = self.types.filter(t => t.guid === self.currency.currencyTypeGuid)[0].guid; 
                    }
                    
                    console.dir(self.form);
                  }
                });
        }
    }
</script>

<style scoped>

</style>
