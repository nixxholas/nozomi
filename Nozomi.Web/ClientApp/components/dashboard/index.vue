<template>
  <div class="container is-fluid">
    <div class="tile is-ancestor">
      <div class="tile is-parent">
        <b-menu class="tile is-child">
          <b-menu-list label="Menu">
            <b-menu-item icon="information-outline" label="Info"></b-menu-item>
            <b-menu-item icon="account" label="My Account">
              <b-menu-item
                label="Account data"
                icon="account-edit"
                tag="router-link"
                to="/dashboard/modify-account"></b-menu-item>
              <b-menu-item
                label="Addresses"
                icon="pound">
              </b-menu-item>
            </b-menu-item>
          </b-menu-list>
          <b-menu-list>
            <b-menu-item
              label="Expo"
              icon="link"
              tag="router-link"
              to="/expo">
            </b-menu-item>
          </b-menu-list>
          <b-menu-list label="Actions">
            <b-menu-item label="Logout"></b-menu-item>
          </b-menu-list>
        </b-menu>
      </div>
      <div class="tile is-vertical is-10">
        <div class="tile">
          <div class="tile is-parent is-vertical">
            <b-notification aria-close-label="Close notification">
              <p class="title">Welcome to <b>Cabin</b>!</p>
              <p class="subtitle">Nozomi's Cabin is designed to make interacting and processing with APIs simple and easy to do.
                By the end of our beta, APIs can be interacted by providing the API path, properties within it and the data you
                want to process and that's it!</p>
            </b-notification>
            <article class="tile is-child notification is-warning">
              <p class="title">...tiles</p>
              <p class="subtitle">Bottom tile</p>
            </article>
          </div>
          <div class="tile is-parent">
            <article class="tile is-child notification is-info">
              <p class="title">Middle tile</p>
              <p class="subtitle">With an image</p>
              <figure class="image is-4by3">
                <img src="https://bulma.io/images/placeholders/640x480.png">
              </figure>
            </article>
          </div>
        </div>
        <div class="tile is-parent">
          <article class="tile is-child notification is-danger">
            <p class="title">Requests</p>
            <div class="content">
              <b-table :data="requestData" :columns="requestColumns">
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
    export default {
        name: "Dashboard",
        data: function() {
            return {
                requestData: [
                    {
                        'id': 1,
                        'first_name': 'Jesse',
                        'last_name': 'Simmons',
                        'date': '2016-10-15 13:43:27',
                        'gender': 'Male'
                    },
                    {
                        'id': 2,
                        'first_name': 'John',
                        'last_name': 'Jacobs',
                        'date': '2016-12-15 06:00:53',
                        'gender': 'Male'
                    },
                    {
                        'id': 3,
                        'first_name': 'Tina',
                        'last_name': 'Gilbert',
                        'date': '2016-04-26 06:26:28',
                        'gender': 'Female'
                    },
                    {
                        'id': 4,
                        'first_name': 'Clarence',
                        'last_name': 'Flores',
                        'date': '2016-04-10 10:28:46',
                        'gender': 'Male'
                    },
                    {
                        'id': 5,
                        'first_name': 'Anne',
                        'last_name': 'Lee',
                        'date': '2016-12-06 14:38:38',
                        'gender': 'Female'
                    }
                ],
                requestColumns: [
                    {
                        field: 'id',
                        label: 'ID',
                        width: '40',
                        numeric: true
                    },
                    {
                        field: 'first_name',
                        label: 'First Name',
                    },
                    {
                        field: 'last_name',
                        label: 'Last Name',
                    },
                    {
                        field: 'date',
                        label: 'Date',
                        centered: true
                    },
                    {
                        field: 'gender',
                        label: 'Gender',
                    }
                ]
            }
        }
    }
</script>

<style scoped>

</style>
