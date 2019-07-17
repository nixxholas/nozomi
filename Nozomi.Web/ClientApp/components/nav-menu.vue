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
        <a class="navbar-item" href="https://bulma.io/">
          Home
        </a>
        <div class="navbar-item has-dropdown is-hoverable">
          <a class="navbar-link" href="https://bulma.io/documentation/overview/start/">
            Docs
          </a>
          <div class="navbar-dropdown is-boxed">
            <a class="navbar-item" href="https://bulma.io/documentation/overview/start/">
              Overview
            </a>
            <a class="navbar-item" href="https://bulma.io/documentation/modifiers/syntax/">
              Modifiers
            </a>
            <a class="navbar-item" href="https://bulma.io/documentation/columns/basics/">
              Columns
            </a>
            <a class="navbar-item" href="https://bulma.io/documentation/layout/container/">
              Layout
            </a>
            <a class="navbar-item" href="https://bulma.io/documentation/form/general/">
              Form
            </a>
            <hr class="navbar-divider">
            <a class="navbar-item" href="https://bulma.io/documentation/elements/box/">
              Elements
            </a>
            <a class="navbar-item is-active" href="https://bulma.io/documentation/components/breadcrumb/">
              Components
            </a>
          </div>
        </div>
      </div>

      <div class="navbar-end">
        <div class="navbar-item">
          <div class="field is-grouped">
            <p class="control">
              <a class="bd-tw-button button" data-social-network="Twitter" data-social-action="tweet" data-social-target="http://localhost:4000" target="_blank" href="https://twitter.com/intent/tweet?text=Bulma: a modern CSS framework based on Flexbox&amp;hashtags=bulmaio&amp;url=http://localhost:4000&amp;via=jgthms">
              <span class="icon">
                <i class="fab fa-twitter"></i>
              </span>
                <span>
                Tweet
              </span>
              </a>
            </p>
            <p class="control">
              <a class="button is-primary" href="https://github.com/jgthms/bulma/releases/download/0.7.5/bulma-0.7.5.zip">
              <span class="icon">
                <i class="fas fa-download"></i>
              </span>
                <span>Download</span>
              </a>
            </p>
          </div>
        </div>
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
      methods: {
        toggleCollapsed: function (event) {
          this.collapsed = !this.collapsed
        }
      }
    }
</script>

<style scoped>
    .slide-enter-active, .slide-leave-active {
    transition: max-height .35s
    }
    .slide-enter, .slide-leave-to {
    max-height: 0px;
    }

    .slide-enter-to, .slide-leave {
    max-height: 20em;
    }
</style>
