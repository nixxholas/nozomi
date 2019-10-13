// https://github.com/perarnborg/vuex-oidc-example/blob/master/src/views/OidcCallback.vue
<template>
  <div>
  </div>
</template>

<script>
    import { mapActions } from 'vuex'

    export default {
        name: 'OidcCallback',
        methods: {
            ...mapActions('oidcStore', [
                'oidcSignInCallback'
            ])
        },
        mounted () {
            this.oidcSignInCallback()
                .then((redirectPath) => {
                    // https://stackoverflow.com/questions/2647867/how-to-determine-if-variable-is-undefined-or-null
                    this.$router.push(redirectPath == null ? redirectPath : "/");
                })
                .catch((err) => {
                    console.error(err);
                    this.$router.push('/oidc-callback-error'); // Handle errors any way you want
                })
        }
    }
</script>
