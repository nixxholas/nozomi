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
        <b-button type="is-primary" v-if="hasWeb3" @click="login()" :loading="loginLoading">
          <span>Sign in with</span>
          <b-icon
            icon="ethereum"
            size="is-small">
          </b-icon>
        </b-button>
        <b-button type="is-warning" v-else @click="login()" :loading="loginLoading">Login</b-button>
      </b-navbar-item>
    </template>
  </b-navbar>
</template>

<script>
    import { routes } from '../router/routes';
    import Web3 from 'web3';
    import axios from 'axios';

    export default {
      data () {
        return {
          routes,
          loginLoading: false,
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
              this.loginLoading = true;

              try {
                  // Modern dapp browsers...
                  if (window.ethereum) {
                      // Propagate Web3
                      window.web3 = new Web3(ethereum);
                      await window.ethereum.enable();
                      window.ethereum.autoRefreshOnNetworkChange = false;

                      // Obtain the user accounts
                      let authMsg = 'This is a Nozomi auth message';
                      let accounts = await window.web3.eth.getAccounts();

                      // Ensure that the user is holding the wallet/s by asking him to unlock his
                      // account with a random message.
                      // https://ethereum.stackexchange.com/questions/48489/how-to-prove-that-a-user-owns-their-public-key-for-free=
                      if (accounts != null && accounts.length > 0) {
                          accounts.forEach(function (account) {
                              let shaMsg = window.web3.utils.sha3(authMsg);
                              let signed = window.web3.eth.accounts.sign(account, shaMsg,
                                  function (err, sig) {
                                      this.$buefy.notification.open({
                                          duration: 5000,
                                          message: `There was an error validating your account.`,
                                          position: 'is-bottom-right',
                                          type: 'is-danger',
                                          hasIcon: true
                                      });
                                  });

                              console.dir("User signed data: ");
                              console.dir(signed);

                              // Validate the signed object on server side and provide an auth
                              let result = axios.post('/api/auth/ethauth',
                                  JSON.stringify({
                                      claimerAddress: account,
                                      signature: signed,
                                      rawMessage: authMsg
                                  }),
                                  {
                                      headers: {
                                          "Content-type": "application/json",
                                      }
                                  }
                              );

                              console.dir("result: " + result);
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
              } catch (error) {
                  console.dir(error);

                  // User denied account access...
                  this.$buefy.notification.open({
                      duration: 5000,
                      message: `Your browser may not be supporting Web3 properly.`,
                      position: 'is-bottom-right',
                      type: 'is-danger',
                      hasIcon: true
                  });
              }

              this.loginLoading = false;
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
            if (this.hasWeb3() && !this.loginLoading) {
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
