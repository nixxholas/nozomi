<template>
  <div class="container is-fluid">
    <div class="tile is-ancestor">
      <div class="tile is-vertical">
        <div class="tile">
          <div class="tile is-parent is-vertical">
            <b-notification aria-close-label="Close notification">
              <p class="title">Welcome to <b>Cabin</b>!</p>
              <p class="subtitle">Nozomi's Cabin is designed to make interacting and processing with APIs simple and easy to do.
                By the end of our beta, APIs can be interacted by providing the API path, properties within it and the data you
                want to process and that's it!</p>
            </b-notification>
            <article class="tile is-child notification is-warning">
              <p class="title">Favourites</p>
              <p class="subtitle is-italic">Coding in progress..</p>
            </article>
          </div>
          <div class="tile is-parent">
            <article class="tile is-child notification is-info">
              <p class="title"></p>
              <p class="subtitle">What more should we do?</p>
              <figure class="image is-4by3">
                <img src="https://bulma.io/images/placeholders/640x480.png">
              </figure>
            </article>
          </div>
        </div>
        <div class="tile is-parent">
          <article class="tile is-child notification is-danger">
            <b-field group-multiline positon="is-left">
              <div class="control">
                <p class="title">Requests</p>
              </div>
            </b-field>
            <b-field position="is-right">
              <div class="control">
                <CreateRequestComponent @created="createdNewRequest"></CreateRequestComponent>
              </div>
            </b-field>
            <div class="content">
              <b-table
                :data="requestData"
                :columns="requestColumns"
                detailed
                detail-key="guid">
                <template slot-scope="props">
                  <b-table-column field="requestType" label="Type">
                    <b-tag type="is-dark">
                      {{ getRequestType(props.row.requestType) }}
                    </b-tag>
                  </b-table-column>
                  <b-table-column field="responseType" label="Response Type">
                    <b-tag type="is-dark">
                      {{ getResponseType(props.row.responseType) }}
                    </b-tag>
                  </b-table-column>
                  <b-table-column field="dataPath" label="API Url">
                    <a class="has-text-info" :href="props.row.dataPath">{{ props.row.dataPath }}</a>
                  </b-table-column>
                  <b-table-column field="delay" label="Delay">
                    <b-tag type="is-info">
                      {{ props.row.delay }} ms
                    </b-tag>
                  </b-table-column>
                  <b-table-column field="failureDelay" label="Failure Delay">
                    <b-tag type="is-warning">
                      {{ props.row.failureDelay }} ms
                    </b-tag>
                  </b-table-column>
                  <b-table-column field="actions" label="">
                    <div class="buttons">
                      <b-button type="is-danger"
                                icon-left="delete">
                        Delete
                      </b-button>
                    </div>
                  </b-table-column>
                </template>
                <template slot="detail" slot-scope="props">
                  <b-taglist attached>
                    <b-tag type="is-dark">Unique ID</b-tag>
                    <b-tag type="is-info">{{ props.row.guid }}</b-tag>
                  </b-taglist>
                  <nav class="level is-mobile">
                    <div class="level-item has-text-centered">
                      <div>
                        <p class="heading">Status</p>
                        <p class="title"
                           v-bind:class="{ 'has-text-danger': !props.row.isEnabled,
                           'has-text-success': props.row.isEnabled }">
                          {{ props.row.isEnabled ? "Active" : "Disabled" }}
                        </p>
                      </div>
                    </div>
<!--                    <div class="level-item has-text-centered">-->
<!--                      <div>-->
<!--                        <p class="heading">Following</p>-->
<!--                        <p class="title">123</p>-->
<!--                      </div>-->
<!--                    </div>-->
<!--                    <div class="level-item has-text-centered">-->
<!--                      <div>-->
<!--                        <p class="heading">Followers</p>-->
<!--                        <p class="title">456K</p>-->
<!--                      </div>-->
<!--                    </div>-->
<!--                    <div class="level-item has-text-centered">-->
<!--                      <div>-->
<!--                        <p class="heading">Likes</p>-->
<!--                        <p class="title">789</p>-->
<!--                      </div>-->
<!--                    </div>-->
                  </nav>
                </template>
                <template slot="empty">
                  <section class="section">
                    <div class="content has-text-grey has-text-centered">
                      <p>
                        <b-icon
                          icon="emoticon-sad"
                          size="is-large">
                        </b-icon>
                      </p>
                      <p>Nothing here.</p>
                    </div>
                  </section>
                </template>
              </b-table>
            </div>
          </article>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
    import store from '../../store/index';
    import { mapActions } from 'vuex';
    import CreateRequestComponent from '../dashboard/forms/create-request-modal';

    export default {
        name: "Dashboard",
        components: { CreateRequestComponent },
        data: function() {
            return {
                requestTypes: [],
                responseTypes: [],
                requestData: [],
                requestColumns: [
                    {
                        field: 'guid',
                        label: 'ID',
                        width: '40',
                    },
                    {
                        field: 'requestType',
                        label: 'Type',
                    },
                    {
                        field: 'responseType',
                        label: 'Response Type',
                    },
                    {
                        field: 'dataPath',
                        label: 'URL',
                    },
                    {
                        field: 'delay',
                        label: 'Delay',
                        centered: true,
                        numeric: true
                    },
                    {
                        field: 'failureDelay',
                        label: 'Failure Delay',
                        centered: true,
                        numeric: true
                    }
                ]
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            createdNewRequest: function (value) {
                if (value)
                    this.updateRequests();
            },
            updateRequests: function () {
                let self = this;

                // Synchronously call for data
                this.$axios.get('/api/Request/GetAll', {
                    headers: {
                        Authorization: "Bearer " + store.state.oidcStore.access_token
                    }
                })
                    .then(function (response) {
                        self.requestData = response.data;
                    })
                    .catch(function (error) {
                        // handle error
                        self.methods.authenticateOidc(self.currentRoute);
                    })
                    .finally(function () {
                        // always executed
                        self.isLoading = false;
                    });
            },
            getRequestType: function(val) {
                let result = "-";

                this.requestTypes.forEach(function(item){
                    if (item.value === val) {
                        result = item.key;
                    }
                });

                return result;
            },
            getResponseType: function(val) {
                let result = "-";

                this.responseTypes.forEach(function(item){
                    if (item.value === val) {
                        result = item.key;
                    }
                });

                return result;
            }
        },
        beforeMount: function() {
            this.updateRequests();

            let self = this;

            // Setup Request types
            this.$axios.get('/api/RequestType/All', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    self.requestTypes = response.data.data.value;
                })
                .catch(function (error) {
                    // handle error
                    self.methods.authenticateOidc(self.currentRoute);
                })
                .finally(function () {
                    // always executed
                    self.isLoading = false;
                });

            // Setup Response types
            this.$axios.get('/api/ResponseType/All', {
                headers: {
                    Authorization: "Bearer " + store.state.oidcStore.access_token
                }
            })
                .then(function (response) {
                    self.responseTypes = response.data.data.value;
                })
                .catch(function (error) {
                    // handle error
                    self.methods.authenticateOidc(self.currentRoute);
                })
                .finally(function () {
                    // always executed
                    self.isLoading = false;
                });
        }
    }
</script>

<style scoped>

</style>
