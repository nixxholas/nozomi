<template>
  <div class="is-primary">
    <div class="level-left">
      <b-button type="is-text" tag="router-link" to="/dashboard">Back to dashboard</b-button>
    </div>
    <section class="hero is-medium is-bold">
      <div class="hero-body">
        <div class="container">
          <h1 class="title">
            User profile
          </h1>
          <h2 class="subtitle">
            <form>
              <b-field>
                <b-input placeholder="Email"
                         type="email"
                         icon="email">
                </b-input>
              </b-field>
            </form>
            {{ profile }}
          </h2>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex';

    export default {
        name: "modify-profile",
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcUser',
                'oidcAccessToken',
                'oidcAccessTokenExp',
                'oidcIdToken',
                'oidcIdTokenExp',
                'oidcAuthenticationIsChecked',
                'oidcError'
            ]),
            hasAccess: function() {
                return this.oidcIsAuthenticated || this.$route.meta.isPublic
            }
        },
        data: function () {
            return {
                profile: this.oidcUser,
                accessToken: this.oidcAccessToken
            }
        }
    }
</script>

<style scoped>

</style>
