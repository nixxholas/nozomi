<template>
  <b-navbar :transparent="true">
    <template slot="brand">
      <b-navbar-item tag="router-link" to="/">
        <img
          src="@/assets/logo.png"
          alt="Nozomi: Data, real quick."
          style="width: 112px; height: 28px;"
        >
      </b-navbar-item>
    </template>

    <template slot="start">
      <b-navbar-item tag="router-link" 
                     :to="route.path" 
                     v-for="(route, index) in routes"
                     :key="index"
                     v-if="route.meta.onNav">
        <b-icon v-if="route.meta.icon"
                :icon="route.meta.icon" 
                class="mr-half-em"/>
        <span>{{ route.display }}</span>
      </b-navbar-item>
    </template>

    <template slot="end">
      <b-navbar-item tag="div" v-if="!oidcIsAuthenticated">
        <b-button type="is-primary"
                  v-if="hasWeb3"
                  icon-pack="fab"
                  icon-right="ethereum"
                  @click="authenticateOidc(currentRoute)" 
                  :loading="loginLoading">
          Sign in with 
        </b-button>
        <b-button type="is-warning" v-else @click="authenticateOidc(currentRoute)" :loading="loginLoading">Login</b-button>
      </b-navbar-item>
      <b-navbar-item tag="div" class="buttons" v-else>
        <b-button type="is-info"
                  icon-left="columns"
                  tag="router-link"
                  to="/dashboard">
          <span>Dashboard</span>
        </b-button>
        <b-button type="is-danger"
                  icon-left=""
                  @click="signOutOidc()">
          <span>Logout</span>
        </b-button>
      </b-navbar-item>
    </template>
  </b-navbar>
</template>

<script>
    import { routes } from '@/routing/routes';
    import { mapActions, mapGetters } from 'vuex';

    export default {
        data() {
            return {
                routes,
                loginLoading: false,
                collapsed: true,
                currentRoute: window.location.href // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            }
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcAuthenticationIsChecked',
                'oidcUser',
                'oidcIdToken',
                'oidcIdTokenExp'
            ]),
            hasAccess: function() {
                return this.oidcIsAuthenticated || this.$route.meta.isPublic
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            hasWeb3() {
                try {
                    return window.ethereum || window.web3;
                } catch (e) {
                    // User does not have a Web3-supportive Plugin/Browser.
                    return false;
                }
            }
        }
    }
</script>

<style scoped>
  .mr-quarter-em {
    margin-right: .25em;
  }
  .mr-half-em {
    margin-right: .5em;
  }
</style>
