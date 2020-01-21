<template>
    <section class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">
                    Settings
                    {{ user }}
                </h1>
                <b-tabs type="is-toggle" expanded>
                    <b-tab-item label="Profile" icon="user">
                        <form v-on:submit.prevent="push()">
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

                            <b-field grouped>
                                <b-field label="Current Password" expanded>
                                    <b-input type="password" v-model="model.previousPassword" password-reveal/>
                                </b-field>

                                <b-field label="New Password" expanded>
                                    <b-input type="password" v-model="model.password" password-reveal/>
                                </b-field>
                            </b-field>

                            <b-field>
                                <b-button type="is-primary"
                                          native-type="submit">
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
    import {mapGetters} from 'vuex';
    import NozomiAuthService from "@/services/NozomiAuthService";

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
                previousPassword: null,
                password: null,
                user: {
                    given_name: '',
                    family_name: '',
                    preferred_username: '',
                    email: '',
                    email_verified: '',
                    website: '',
                    default_wallet_hash: ''
                },
                model: {
                    password: '',
                    previousPassword: '',
                    userClaims: {}
                }
            }
        },
        mounted: function () {
            this.user = this.oidcUser;
        },
        methods: {
            push: function () {
                let self = this;

                for (let key in self.user) {
                    if (self.user.hasOwnProperty(key)) {
                        self.model.userClaims[key] = self.user[key];
                    }
                }

                // for (let key in self.user) {
                //     if (self.user.hasOwnProperty(key)) {
                //         console.dir(key);
                //         self.model.userClaims.push({ 
                //             key: key,
                //             value: self.user[key]
                //         });
                //     }
                // }

                // TODO: Ensure that userClaims does not cause the entire payload to end up null in the API.
                console.dir(self.model);

                NozomiAuthService.update(self.model)
                    .then(function (res) {
                        console.dir(res);
                    }).catch(function (err) {
                    console.dir(err)
                });
            }
        }
    }
</script>