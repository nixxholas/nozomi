<template>
  <b-navbar :transparent="true">
    <template slot="brand">
      <b-navbar-item tag="router-link" to="/">
        <img
          src="/assets/logo.png"
          alt="Nozomi: Data, real quick."
          style="width: 112px; height: 28px;"
        >
      </b-navbar-item>
    </template>

    <template slot="start">
      <b-navbar-item tag="router-link" :to="route.path" v-for="(route, index) in routes"
                     :key="index"
                     v-if="route.meta.onNav">
        <icon :icon="route.icon" class="mr-2 menu-icon" v-if="route.icon"/>
        <span>{{ route.display }}</span>
      </b-navbar-item>
      <b-navbar-item href="/docs">
        <icon :icon="['fa', 'atlas']" class="mr-2 menu-icon"/>
        <span>Documentation</span>
      </b-navbar-item>
    </template>

    <template slot="end">
      <b-navbar-item tag="div" v-if="!oidcIsAuthenticated">
        <b-button type="is-primary" v-if="hasWeb3" @click="authenticateOidc(currentRoute)" :loading="loginLoading">
          <span>Sign in with</span>
          <b-icon
            icon="ethereum"
            size="is-small">
          </b-icon>
        </b-button>
        <b-button type="is-warning" v-else @click="authenticateOidc(currentRoute)" :loading="loginLoading">Login</b-button>
      </b-navbar-item>
      <b-navbar-item tag="div" class="buttons" v-else>
        <b-button type="is-info"
                  icon-left="view-dashboard"
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
</style>
