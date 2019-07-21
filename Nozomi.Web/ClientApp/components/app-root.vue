<template>
  <div id="app">
    <nav-menu params="route: route"></nav-menu>

    <div class="container is-fullhd">
      <router-view></router-view>
    </div>

    <footer class="footer" style="position: absolute; bottom: 0; width: 100%;">
      <div class="content container">
        <div class="columns is-mobile">
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
