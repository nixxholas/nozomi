<template>
    <section class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">
                    Settings
                </h1>
                <b-tabs type="is-toggle" expanded>
                    <b-tab-item label="Profile" icon="user">
                        <form method="put">
                            <b-field grouped>
                                <b-field label="First name" expanded>
                                    <b-input v-model="user.given_name" disabled/>
                                </b-field>

                                <b-field label="Last name" expanded>
                                    <b-input v-model="user.family_name" disabled/>
                                </b-field>
                            </b-field>

                            <b-field label="Username">
                                <b-input v-model="user.preferred_username" disabled/>
                            </b-field>

                            <b-field
                                    :type="{ 'is-danger': !user.email_verified && user.email }"
                                    :message="[
                                        { 'This email is pending verification': (!user.email_verified && user.email) }
                                        ]"
                                    label="Email">
                                <b-input
                                        v-model="user.email" disabled/>
                            </b-field>
                            
                            <b-field label="Website">
                                <b-input type="url" v-model="user.website" disabled/>
                            </b-field>

                            <b-field label="Default Wallet Address">
                                <b-input type="url" v-model="user.default_wallet_hash" disabled/>
                            </b-field>

                            <b-field>
                                <b-button type="is-primary"
                                          native-type="submit"
                                          value="Update" disabled>
                                    Update
                                </b-button>
                            </b-field>
                        </form>
                    </b-tab-item>
                    <b-tab-item label="Billing" icon="money-bill">

                    </b-tab-item>
                    <!--                    <b-tab-item label="Videos" icon="video"></b-tab-item>-->
                </b-tabs>
            </div>
        </div>
    </section>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';

    export default {
        name: 'settings-index',
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcAuthenticationIsChecked',
                'oidcUser',
                'oidcIdToken',
                'oidcIdTokenExp'
            ]),
        },
        data: function () {
            return {
                user: {}
            }
        },
        mounted: function () {
            this.user = this.oidcUser;

            console.dir(this.user);
        }
    }
</script>