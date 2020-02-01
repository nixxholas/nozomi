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
                                    <b-input v-model="model.userClaims.given_name" />
                                </b-field>

                                <b-field label="Last name" expanded>
                                    <b-input v-model="model.userClaims.family_name" />
                                </b-field>
                            </b-field>

                            <b-field label="Username">
                                <b-input v-model="model.userClaims.preferred_username" disabled/>
                            </b-field>

                            <b-field
                                    :type="{ 'is-danger': !model.userClaims.email_verified && model.userClaims.email }"
                                    :message="[
                                        { 'This email is pending verification': (!model.userClaims.email_verified && model.userClaims.email) }
                                        ]"
                                    label="Email">
                                <b-input
                                        v-model="model.userClaims.email" disabled/>
                            </b-field>

                            <b-field label="Website">
                                <b-input type="url" v-model="model.userClaims.website" />
                            </b-field>

                            <b-field label="Default Wallet Address">
                                <b-input type="url" v-model="model.userClaims.default_wallet_hash" disabled/>
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
                        <CardsComponent :id="model.userClaims.stripe_cust_id" @created="updateUser" />
                    </b-tab-item>
                    <b-tab-item label="API Keys" icon="key">
                        
                    </b-tab-item>
                </b-tabs>
            </div>
        </div>
    </section>
</template>

<script>
    import {mapGetters} from 'vuex';
    import NozomiAuthService from "@/services/NozomiAuthService";
    import CardsComponent from '@/components/stripe/cards';
    import {NotificationProgrammatic as Notification} from 'buefy';

    export default {
        name: 'settings-index',
        components: {
            CardsComponent
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcAuthenticationIsChecked',
                'oidcUser',
                'oidcIdToken',
                'oidcIdTokenExp',
            ]),
        },
        data: function () {
            return {
                previousPassword: null,
                password: null,
                model: {
                    password: '',
                    previousPassword: '',
                    userClaims: {
                        given_name: '',
                        family_name: '',
                        preferred_username: '',
                        email: '',
                        email_verified: '',
                        website: '',
                        default_wallet_hash: '',
                    }
                }
            }
        },
        created: function () {
            let self = this;
            self.model.userClaims = this.oidcUser;

            // If stripe's cust id ain't found,
            if (!self.model.userClaims.stripe_cust_id) {
                // Just attempt to look for it just in case
                NozomiAuthService.getStripeCustId()
                    .then(function (res) {
                        if (res && res.status === 200 && res.data && res.data !== "") {
                            self.model.userClaims.stripe_cust_id = res.data;
                        } else {
                            self.model.userClaims.stripe_cust_id = null;
                        }
                    })
                    .catch(function (err) {
                        self.model.userClaims.stripe_cust_id = null;
                    });
            }
        },
        methods: {
            push: function () {
                let self = this;

                NozomiAuthService.update(JSON.parse(JSON.stringify(self.model)))
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
            },
            updateUser: function() {
                let self = this;
                
                if (!self.model.userClaims.stripe_cust_id) {
                    // Update the user
                    NozomiAuthService.getStripeCustId()
                        .then(function (res) {
                            if (res && res.status === 200 && res.data) {
                                self.model.userClaims.stripe_cust_id = res.data;
                                
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
                        })
                        .catch(function (err) {
                            Notification.open({
                                duration: 2500,
                                message: `There might've been a communication error, please try again!`,
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                            });
                        });
                }
            }
        }
    }
</script>