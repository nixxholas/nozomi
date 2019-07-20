<template>
  <nav class="navbar is-transparent">
    <div class="navbar-brand">
      <router-link class="navbar-item" to="/">
        <img src="https://bulma.io/images/bulma-logo.png" alt="Nozomi: Data, real quick." width="112" height="28">
      </router-link >
      <div class="navbar-burger burger" data-target="navbarMenu">
        <span></span>
        <span></span>
        <span></span>
      </div>
    </div>
<!--    <li class="nav-item" v-for="(route, index) in routes" :key="index">-->
<!--      <router-link :to="route.path" exact-active-class="active">-->
<!--        <icon :icon="route.icon" class="mr-2 menu-icon" /><span>{{ route.display }}</span>-->
<!--      </router-link>-->
<!--    </li>-->

    <div id="navbarMenu" class="navbar-menu">
      <div class="navbar-start">
        <div class="navbar-item has-dropdown is-hoverable">
          <span class="navbar-link">
            Metrics
          </span>
          <div class="navbar-dropdown is-boxed">
            <a class="navbar-item" href="/docs">
              <icon :icon="['fa', 'atlas']" class="mr-2 menu-icon" />
              <span>Documentation</span>
            </a>
            <router-link class="navbar-item" :to="route.path" v-for="(route, index) in routes"
                         v-if="index > 0"
                         :key="index">
              <icon :icon="route.icon" class="mr-2 menu-icon" /><span>{{ route.display }}</span>
            </router-link>
          </div>
        </div>
      </div>

      <div class="navbar-end">
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
      </div>
    </div>
  </nav>
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
