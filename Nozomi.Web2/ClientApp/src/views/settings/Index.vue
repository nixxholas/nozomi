<template>
    <section class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">
                    Settings
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
    import {NotificationProgrammatic as Notification} from "buefy/types/components";

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
                    userClaims: []
                }
            }
        },
        mounted: function () {
            this.user = this.oidcUser;
        },
        methods: {
            push: function () {
                let self = this;

                // Collate them first
                // for (let key in self.user) {
                //     if (self.user.hasOwnProperty(key)) {
                //         self.model.userClaims.push({ key : key, value: self.user[key] });
                //     }
                // }
                
                // Then compress them
                // self.model.userClaims = JSON.stringify(self.model.userClaims);

                // for (let key in self.user) {
                //     if (self.user.hasOwnProperty(key)) {
                //         console.dir(key);
                //         self.model.userClaims.push({ 
                //             key: key,
                //             value: self.user[key]
                //         });
                //     }
                // }

                NozomiAuthService.update(self.model)
                    .then(function (res) {
                        if (res && res.status === 200) {
                            self.isModalActive = false; // Close the modal
                            Notification.open({
                                duration: 2500,
                                message: self.form.name + ` successfully updated!`,
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });
                        } else {
                            Notification.open({
                                duration: 2500,
                                message: `There might've been a communication error, please try again!`,
                                position: 'is-bottom-right',
                                type: 'is-warning',
                                hasIcon: true
                            });
                        }
                    }).catch(function (err) {
                    Notification.open({
                        duration: 2500,
                        message: `Please make sure your entry is correctly filled!`,
                        position: 'is-bottom-right',
                        type: 'is-danger',
                        hasIcon: true
                    });
                });
            }
        }
    }
</script>