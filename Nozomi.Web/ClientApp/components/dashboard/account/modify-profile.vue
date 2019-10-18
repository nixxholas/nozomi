<template>
{{ profile }}
</template>

<script>
    import { mapGetters, mapActions } from 'vuex';

    export default {
        name: "Modify Profile",
        computed: {
            ...mapGetters([
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
                profile: function() {
                    return this.oidcUser();
                }
            }
        }
    }
</script>

<style scoped>

</style>
