﻿<template>
  <div>
    <b-notification v-if="oidcIsAuthenticated && !oidcUser.email_verified" 
                    class="is-warning is-marginless is-centered"
                    @close="authenticateOidcSilent"
    >
      <div class="container is-centered is-flex-column">
        <div>
          Please verify your account by visiting your inbox (<strong>{{ oidcUser.email }}</strong>)
        </div>
        <div class="is-flex is-centered">
          Did not receive our email?
          <b-button @click="sendConfirmationEmail" type="is-text">
            Resend Email
          </b-button>
        </div>
      </div>
    </b-notification>
    
    <b-navbar type="container" :transparent="true" style="padding: .5em; margin-bottom: .5em;">
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
        <b-navbar-item>
          <b-tag type="is-warning">EAP 21 March 2020</b-tag>
        </b-navbar-item>
      </template>

      <template slot="end">
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
        
        <b-navbar-item tag="div" v-if="!oidcIsAuthenticated">
          <b-button type="is-primary"
                    outlined
                    v-if="hasWeb3"
                    @click="authenticateOidc(currentRoute)"
                    :loading="loginLoading">
            Login
          </b-button>
          <b-button type="is-warning" v-else @click="authenticateOidc(currentRoute)" :loading="loginLoading">Login</b-button>
        </b-navbar-item>
        <b-navbar-dropdown label="Menu" :right="true" v-else>
          <b-navbar-item>
            Logged as <strong class="pl-1">{{ oidcUser.preferred_username }}</strong>
          </b-navbar-item>

          <hr class="dropdown-divider">

          <b-navbar-item tag="router-link" to="/dashboard">
            Dashboard
          </b-navbar-item>

          <hr class="dropdown-divider">

          <b-navbar-item tag="router-link" to="/settings">
            Settings
          </b-navbar-item>

          <b-navbar-item @click="signOutOidc()">
            Logout
          </b-navbar-item>
        </b-navbar-dropdown>
      </template>
    </b-navbar>
  </div>
</template>

<script>
    import { routes } from '@/routing/routes';
    import { mapActions, mapGetters } from 'vuex';
    import NozomiAuthService from '@/services/NozomiAuthService';

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
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc', 'authenticateOidcSilent']),
            hasWeb3() {
                try {
                    return window.ethereum || window.web3;
                } catch (e) {
                    // User does not have a Web3-supportive Plugin/Browser.
                    return false;
                }
            },
            async sendConfirmationEmail() {              
              try {
                const result = await NozomiAuthService.resendConfirmationEmail();
                
                this.$buefy.notification.open({
                  message: result,
                  type: 'is-success',
                  position: 'is-bottom-right'
                });
              } catch(e) {}
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
  .is-centered {
    align-items: center;
    justify-content: center;
  }
  .is-flex-column {
    display: flex;
    flex-direction: column;
  }
</style>
