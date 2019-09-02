<template>
  <div id="app">
    <b-message type="is-warning" has-icon>
      <b>We're currently in the Release Candidate phase.</b> General UI and Major Features will be constantly updated.
      <br>
      <b v-if="!hasWeb3">Your browser is incompatible with our authentication engine.</b>
    </b-message>
    <nav-menu params="route: route"></nav-menu>

    <div class="container is-fullhd" style="flex: 1; width: 100%">
      <router-view></router-view>
    </div>

    <footer class="footer" style="bottom: 0; width: 100%;">
      <div class="content container">
        <div class="columns is-desktop">
        <div class="column">
          © 2019 Nozomi One Pte. Ltd. All rights reserved.
        </div>
        <div class="column has-text-right">
          <p class="small text-primary">Nozomi Alpha Build - {{ $moment(buildTime).fromNow() }}</p>
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
      async beforeMount() {
        try {
          let buildTimeApi = await this.$axios.get('/api/Core/GetCurrentBuildTime');

          if (buildTimeApi.status === 200)
            this.buildTime = buildTimeApi.data;
        } catch (e) {
          console.dir("Couldn't get the build time.");
        }
      },
      data () {
        return {
          buildTime: '',
          hasWeb3: window.ethereum || window.web3,
        }
      },
      methods: {
      },
    }
</script>

<style>
</style>
