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
<!--    <li class="nav-item" v-for="(route, index) in routes" :key="index">-->
<!--      <router-link :to="route.path" exact-active-class="active">-->
<!--        <icon :icon="route.icon" class="mr-2 menu-icon" /><span>{{ route.display }}</span>-->
<!--      </router-link>-->
<!--    </li>-->

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
          <a class="button is-light">
            Log in
          </a>
        </div>
      </b-navbar-item>
    </template>
<!--    <div id="navbarMenu" class="navbar-menu">-->
<!--      <div class="navbar-start">-->
<!--      </div>-->

<!--      <div class="navbar-end">-->
<!--        <div class="navbar-item">-->
<!--          <div class="field is-grouped">-->
<!--            <p class="control">-->
<!--              <a class="bd-tw-button button" data-social-network="Twitter" data-social-action="tweet" data-social-target="http://localhost:4000" target="_blank" href="https://twitter.com/intent/tweet?text=Bulma: a modern CSS framework based on Flexbox&amp;hashtags=bulmaio&amp;url=http://localhost:4000&amp;via=jgthms">-->
<!--              <span class="icon">-->
<!--                <i class="fab fa-twitter"></i>-->
<!--              </span>-->
<!--              <span>-->
<!--                Tweet-->
<!--              </span>-->
<!--              </a>-->
<!--            </p>-->
<!--            <p class="control">-->
<!--              <a class="button is-primary" href="https://github.com/jgthms/bulma/releases/download/0.7.5/bulma-0.7.5.zip">-->
<!--              <span class="icon">-->
<!--                <i class="fas fa-download"></i>-->
<!--              </span>-->
<!--                <span>Download</span>-->
<!--              </a>-->
<!--            </p>-->
<!--          </div>-->
<!--        </div>-->
<!--      </div>-->
<!--    </div>-->
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
        }
      }
    }
</script>

<style scoped>
</style>
