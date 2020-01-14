<template>
  <b-navbar type="is-light" :transparent="true" style="padding: .5em; margin-bottom: .5em;">
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
      <b-navbar-item tag="div" v-else>
        <b-dropdown
                v-model="navigation"
                position="is-bottom-left"
                aria-role="menu">
          <a
                  class="navbar-item"
                  slot="trigger"
                  role="button">
            <span class="mr-2">Menu</span>
            <b-icon size="is-small" icon="chevron-down" style="vertical-align: middle;" />
          </a>

          <b-dropdown-item custom aria-role="menuitem">
            Logged as <b>{{ this.oidcUser.preferred_username }}</b>
          </b-dropdown-item>
          <hr class="dropdown-divider">

          <b-dropdown-item value="dashboard" aria-role="menuitem">
            <router-link tag="div" to="/dashboard">
<!--              <b-icon icon="columns" />-->
              Dashboard
            </router-link>
          </b-dropdown-item>
          
          <hr class="dropdown-divider">
          
          <b-dropdown-item value="settings" aria-role="menuitem">
              <router-link tag="div" to="/settings">
                  <!--            <b-icon icon="cog" />-->
                  Settings
              </router-link>
          </b-dropdown-item>
          <b-dropdown-item value="logout" aria-role="menuitem"
                           @click="signOutOidc()">
<!--            <b-icon icon="sign-out-alt" />-->
            Logout
          </b-dropdown-item>
        </b-dropdown>
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
              navigation: null,
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
