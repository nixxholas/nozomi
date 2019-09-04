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
      <b-navbar-item href="/docs">
        <icon :icon="['fa', 'atlas']" class="mr-2 menu-icon" />
        <span>Documentation</span>
      </b-navbar-item>
      <b-navbar-item tag="router-link" :to="route.path" v-for="(route, index) in routes"
                   v-if="index > 0 && index < 2"
                   :key="index">
        <icon :icon="route.icon" class="mr-2 menu-icon" />
        <span>{{ route.display }}</span>
      </b-navbar-item>
    </template>

    <template slot="end">
      <b-navbar-item tag="div">
        <b-button type="is-primary" v-if="hasWeb3" @click="login()">
          <span>Sign in with</span>
          <b-icon
            icon="ethereum"
            size="is-small">
          </b-icon>
        </b-button>
        <b-button type="is-warning" v-else @click="login()">Login</b-button>
      </b-navbar-item>
    </template>
  </b-navbar>
</template>

<script>
    import { routes } from '../router/routes';
    import Web3 from 'web3';

    export default {
      data () {
        return {
          routes,
          collapsed: true
        }
      },
      mounted: function() {
        // Get all "navbar-burger" elements
        const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

        // Check if there are any navbar burgers
        if ($navbarBurgers.length > 0) {

          // Add a click event on each of them
          $navbarBurgers.forEach( el => {
            el.addEventListener('click', () => {

              // Get the target from the "data-target" attribute
              const target = el.dataset.target;
              const $target = document.getElementById(target);

              // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
              el.classList.toggle('is-active');
              $target.classList.toggle('is-active');

            });
          });
        }
      },
      methods: {
        toggleCollapsed: function () {
        },
          async authWeb3() {
              // Modern dapp browsers...
              if (window.ethereum) {
                  try {
                      // Propagate Web3
                      window.web3 = new Web3(ethereum);
                      await window.ethereum.enable();
                      window.ethereum.autoRefreshOnNetworkChange = false;

                      // Obtain the user accounts
                      let accounts = await window.web3.eth.getAccounts();

                      // Solid, now let's obtain all of its data
                      //let option = { from: accounts[0] };


                  } catch (error) {
                      // User denied account access...
                      this.$buefy.notification.open({
                          duration: 5000,
                          message: `Your browser may not be supporting Web3 properly.`,
                          position: 'is-bottom-right',
                          type: 'is-danger',
                          hasIcon: true
                      });
                  }
              }
              // Legacy dapp browsers...
              else if (window.web3) {
                  window.web3 = new Web3(web3.currentProvider);
                  // Acccounts always exposed
                  web3.eth.sendTransaction({/* ... */});
              }
              // Non-dapp browsers...
              else {
                  this.$buefy.notification.open({
                      duration: 5000,
                      message: `Non-Ethereum browser detected. You should consider trying MetaMask!`,
                      position: 'is-bottom-right',
                      type: 'is-warning',
                      hasIcon: true
                  });
              }
          },
          hasWeb3() {
              try {
                  return window.ethereum || window.web3;
              } catch (e) {
                  // User does not have a Web3-supportive Plugin/Browser.
                  return false;
              }
          },
        async login() {
            if (this.hasWeb3()) {
                await this.authWeb3();
            } else {
                this.$buefy.notification.open({
                    duration: 5000,
                    message: `Your browser does not support Web3!`,
                    position: 'is-bottom-right',
                    type: 'is-danger',
                    hasIcon: true
                });
            }
        }
      }
    }
</script>

<style scoped>
</style>
