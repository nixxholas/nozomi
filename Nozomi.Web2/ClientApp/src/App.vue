<template>
  <div id="app">
    <b-message type="is-warning" has-icon>
      <b>We're currently in the Release Candidate phase.</b> General UI and Major Features will be constantly updated.
      <br>
      <b v-if="!hasWeb3()">Your browser is incompatible with our authentication engine.</b>
    </b-message>
    <nav-menu params="route: route"></nav-menu>

    <div class="container is-fullhd" style="flex: 1; width: 100%">
      <router-view></router-view>
    </div>

    <footer class="footer mt-4" style="bottom: 0; width: 100%;">
      <div class="content container">
        <div class="columns is-1 is-centered is-desktop">
          <div class="column">
            <img
                    src="@/assets/logo.png"
                    alt="Nozomi: Data, real quick."
                    style="width: 112px; height: 28px;"
            ><br>
            © Nozomi One Pte. Ltd.<br>
          </div>
          <div class="column">
            <h6 class="title">Company</h6>
            <router-link to="/about">About</router-link><br>
          </div>
          <div class="column">
            <h6 class="title">Resources</h6>
            <a href="/docs">Documentation</a>
          </div>
          <div class="column"></div>
          <div class="column">
            <b-tag type="is-info" class="mb-1">8 Dec 2019 Preview</b-tag><br>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script>
  import NavMenu from './nav-menu';

  export default {
    components: {
      'nav-menu': NavMenu,
    },
    data () {
      return {
        buildTime: '',
      }
    },
    methods: {
      hasWeb3() {
        try {
          return window.ethereum || window.web3;
        } catch (e) {
          // User does not have a Web3-supportive Plugin/Browser.
          return false;
        }
      }
    },
  }
</script>

<style>
  #app {
    display: flex;
    min-height: 100vh;
    flex-direction: column;
  }
</style>
