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
        <icon :icon="['fa', 'atlas']" class="mr-2 menu-icon"/>
        <span>Documentation</span>
      </b-navbar-item>
      <b-navbar-item tag="router-link" :to="route.path" v-for="(route, index) in routes"
                     v-if="index > 0 && index < 2"
                     :key="index">
        <icon :icon="route.icon" class="mr-2 menu-icon"/>
        <span>{{ route.display }}</span>
      </b-navbar-item>
    </template>

    <template slot="end">
      <b-navbar-item tag="div" v-if="!this.isLoggedIn">
        <b-button type="is-primary" v-if="hasWeb3" @click="this.signIn()" :loading="loginLoading">
          <span>Sign in with</span>
          <b-icon
            icon="ethereum"
            size="is-small">
          </b-icon>
        </b-button>
        <b-button type="is-warning" v-else @click="this.signIn()" :loading="loginLoading">Login</b-button>
      </b-navbar-item>
      <b-navbar-item tag="div" v-else>
        <b-button type="is-info"
         icon-left="view-dashboard">
          <span>Dashboard</span>
        </b-button>
      </b-navbar-item>
    </template>
<!--    {{ this.getUserExplicitly() }}-->
  </b-navbar>
</template>

<script>
    import { routes } from '../router/routes';
    import { mapActions, mapGetters } from 'vuex';
    import Web3 from 'web3';

    export default {
        data() {
            return {
                routes,
                loginLoading: false,
                collapsed: true
            }
        },
        computed: {
            ...mapGetters(['isLoggedIn'
                // , 'getUserExplicitly'
            ]),
            ...mapActions(['signIn'])
        },
        methods: {
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
                            let self = this;

                            // https://stackoverflow.com/questions/37576685/using-async-await-with-a-foreach-loop
                            // await Promise.all(accounts.map(async (account) => {
                            // }));

                            // let shaMsg = window.web3.utils.sha3(authMsg);
                            let signed = await window.web3.eth.personal.sign(authMsg, accounts[0],
                                function (err, sig) {
                                    if (err) {
                                        self.$buefy.notification.open({
                                            duration: 3000,
                                            message: `Hey! Don't manipulate any authentication data!`,
                                            position: 'is-bottom-right',
                                            type: 'is-danger',
                                            hasIcon: true
                                        });
                                    }
                                });

                            let web3Payload = {
                                "claimerAddress": accounts[0],
                                "signature": signed,
                                "rawMessage": authMsg
                            };

                            store.dispatch('login', web3Payload);
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

                    // Then run it
                    // store.dispatch('login');
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
            }
        }
    }
</script>

<style scoped>
</style>
