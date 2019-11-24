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
        <div class="columns is-desktop">
          <div class="column">
            © 2019 Nozomi One Pte. Ltd. All rights reserved.
          </div>
          <div class="column has-text-right">
            <p class="small text-primary">Nozomi Alpha Build - 24 Nov 2019</p>
            <strong>Nozomi</strong> by <a href="https://nixholas.com">Nicholas Chen</a>.
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script>
  import NavMenu from './nav-menu'

  export default {
    components: {
      'nav-menu': NavMenu
    },
    data () {
      return {
        buildTime: ''
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
