<template>
  <b-table
    :loading="isLoading"
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
        <div class="level-item has-text-centered">
          <div>
            <p class="heading">Request Component</p>
            <p>
              <CreateRCComponent v-bind:guid="props.row.guid"></CreateRCComponent>
            </p>
          </div>
        </div>
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
              icon="sad-cry"
              size="is-large">
            </b-icon>
          </p>
          <p>Nothing here.</p>
        </div>
      </section>
    </template>
  </b-table>
</template>

<script>
    import store from '@/store/index';
    // Request Component imports
    import CreateRCComponent from '@/components/modals/create-request-component-modal';

    export default {
        name: "requests-table",
        components: { CreateRCComponent },
        data: function () {
            return {
                isLoading: true,
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
            updateRequests: function () {
                this.isLoading = true;
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

                this.isLoading = false;
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
