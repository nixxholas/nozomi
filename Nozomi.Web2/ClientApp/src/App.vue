<template>
    <div id="app" class="container-fluid">
        <!--    <b-message type="is-warning" has-icon>-->
        <!--      <b>We're currently in the Release Candidate phase.</b> General UI and Major Features will be constantly updated.-->
        <!--      <br>-->
        <!--      <b v-if="!hasWeb3()">Your browser is incompatible with our authentication engine.</b>-->
        <!--    </b-message>-->
        <nav-menu params="route: route"/>

        <div style="flex: 1; width: 100%">
            <router-view/>
        </div>

        <footer class="footer mt-4" style="bottom: 0; width: 100%;">
            <div class="content container">
                <div class="columns is-1 is-centered is-desktop">
                    <div class="column">
                        <img
                                src="@/assets/logo.png"
                                alt="Nozomi: Data, real quick."
                                style="width: 112px; height: 28px;"
                        ><br>
                        © Nozomi One Pte. Ltd.<br>
                    </div>
                    <div class="column">
                        <h6 class="title">Company</h6>
                        <router-link to="/about">About</router-link>
                        <br>
                        <router-link to="/changelog">Changelog</router-link>
                        <br>
                        <router-link to="/bugs">Bugs and Issues</router-link>
                        <br>
                    </div>
                    <div class="column">
                        <h6 class="title">Resources</h6>
                        <a href="https://api.nozomi.one">Documentation</a>
                    </div>
                    <div class="column">
                        <h6 class="title">Legal</h6>
                        <router-link to="/legal/api-terms">API Terms</router-link>
                        <br>
                    </div>
                    <div class="column">
                        <b-tag type="is-warning" class="mb-1">EAP 21 Mar 2020</b-tag>
                        <br>
                    </div>
                </div>
            </div>
        </footer>
    </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex';
    import NavMenu from './nav-menu';
    
    export default {
        components: {
            'nav-menu': NavMenu,
        },
        data() {
            return {
                buildTime: '',
                shouldShowResendEmailNotification: false
            }
        },
        computed: {
            ...mapGetters('oidcStore', ['oidcUser'])
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidcSilent']),
            hasWeb3() {
                try {
                    return window.ethereum || window.web3;
                } catch (e) {
                    // User does not have a Web3-supportive Plugin/Browser.
                    return false;
                }
            }
        },
        mounted() {
            // Attempts to renew id_token to remove notification when user
            // verified their email
            if (this.oidcUser && !this.oidcUser.email_verified) {
                this.authenticateOidcSilent();
            }
        }
    }
</script>

<style>
    #app {
        display: flex;
        min-height: 100vh;
        flex-direction: column;
    }
</style>
