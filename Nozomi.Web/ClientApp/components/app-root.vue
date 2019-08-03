<template>
  <div id="app">
    <b-message type="is-info" has-icon>
      <b>We're currently in the test line phase.</b> Major design and layout changes will be made frequently and will not be fixed
      until beta which will help us to develop rapidly.
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
        return {}
      }
    }
</script>

<style>
</style>
