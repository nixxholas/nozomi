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
        <div class="buttons">
          <a class="button is-light" @click="login">
            Log in
          </a>
        </div>
      </b-navbar-item>
    </template>
  </b-navbar>
</template>

<script>
    import { routes } from '../router/routes'

    export default {
      data () {
        return {
          routes,
          collapsed: true
        }
      },
      mounted: function() {
        console.dir(routes);
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
        login() {
          this.$buefy.notification.open({
            duration: 5000,
            message: `Authentication functionality is coming soon!`,
            position: 'is-bottom-right',
            type: 'is-warning',
            hasIcon: true
          })
        }
      }
    }
</script>

<style scoped>
</style>
