<template>
    <div>
        <div ref="table" class="pricing-table" v-if="!isLoading && plans && plans.length > 0 && !viewMode">
            <div v-for="plan in plans" class="pricing-plan">
                <div class="plan-header">{{ plan.nickname }}</div>
                <div class="plan-price">
                    <span class="plan-price-amount">
                        <span class="plan-price-currency">$</span>
                        {{ plan.amount > 0 ? plan.amount / plan.amount_decimal : plan.amount }}
                    </span>/month
                </div>
                <div class="plan-items" v-if="plan.metadata">
                    <div v-for="metaKey of Object.keys(plan.metadata)" class="plan-item">{{ plan.metadata[metaKey] }} {{
                        metaKey }}
                    </div>
                </div>
                <div class="plan-footer">
                    <div v-if="oidcIsAuthenticated && plan.id === currentPlan" class="buttons">
                        <b-button expanded
                                  disabled="disabled">Current plan
                        </b-button>
                        <b-button expanded
                                  v-if="plan.amount !== 0"
                                  type="is-danger"
                                  @click="unsubscribe(plan.id)">
                            Unsubscribe
                        </b-button>
                    </div>
                    <b-button v-else-if="oidcIsAuthenticated && !currentPlan && stripeUserId" @click="subscribe(plan.id)"
                              type="is-primary" expanded>Choose
                    </b-button>
                    <b-button v-else-if="oidcIsAuthenticated && !currentPlan" tag="router-link"
                              to="/settings" type="is-primary" expanded>Choose
                    </b-button>
                    <b-button v-else-if="oidcIsAuthenticated && currentPlan" @click="changeSubscription(plan.id)"
                              type="is-primary" expanded>Switch
                    </b-button>
                    <button v-else class="button is-success" @click="authenticateOidc(currentRoute)">Sign up now!
                    </button>
                </div>
            </div>
        </div>
        <div v-else-if="currentPlan && viewMode" class="container">
            <h2 class="title">You are currently on </h2>
        </div>
        <!--        <div v-else-if="viewMode" class="container p-4">-->
        <!--            <h2 class="title">You currently <span class="has-text-danger">do not</span> have a plan.</h2>-->
        <!--        </div>-->
        <b-loading :is-full-page="false" :active.sync="isLoading" :can-cancel="false"/>
    </div>
</template>

<script>
    import PaymentService from "@/services/auth/PaymentService";
    import {mapActions, mapGetters} from "vuex";
    import {NotificationProgrammatic as Notification} from 'buefy';

    export default {
        name: 'plans',
        props: {
            existingPlan: {
                type: String,
                default: null,
            },
            viewMode: {
                type: Boolean,
                default: false,
            },
        },
        data() {
            return {
                currentPlan: this.existingPlan,
                isLoading: true,
                plans: [],
                stripeUserId: null,
                currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            }
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
        beforeCreate() {
            let self = this;
            
            if (!self.stripeUserId) {
                PaymentService.getStripeCustId()
                .then(function(res) {
                    if (res && res.status === 200) {
                        self.stripeUserId = res.data;
                    }
                });
            }
        },
        mounted() {
            let self = this;

            if (!self.currentPlan) {
                PaymentService.currentPlan()
                    .then(function (res) {
                        if (res && res.status === 200) {
                            self.currentPlan = res.data;
                        }
                    });
            }

            PaymentService.plans()
                .then(function (res) {
                    self.plans = res.data;
                })
                .finally(function () {
                    self.isLoading = false;
                });
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            changeSubscription: function(planId) {
                let self = this;
                self.isLoading = true;
                
                PaymentService.changeSubscription(planId)
                .then(function(res) {
                    if (res && res.status === 200) {
                        Notification.open({
                            duration: 2000,
                            message: res.data ? res.data : 'Plan successfully changed!',
                            position: 'is-bottom-right',
                            type: 'is-success',
                            hasIcon: true
                        });

                        PaymentService.currentPlan()
                            .then(function (res) {
                                if (res && res.status === 200) {
                                    self.currentPlan = res.data;
                                }
                            });
                    }
                })
                .finally(function() {
                    self.isLoading = false;
                });
            },
            subscribe: function (planId) {
                let self = this;
                self.isLoading = true;

                PaymentService.subscribe(planId)
                    .then(function (res) {
                        if (res && res.status === 200) {
                            Notification.open({
                                duration: 2000,
                                message: res.data ? res.data : 'Plan successfully subscribed!',
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });

                            PaymentService.currentPlan()
                                .then(function (res) {
                                    if (res && res.status === 200) {
                                        self.currentPlan = res.data;
                                    }
                                });
                        }
                    })
                    .catch(function (err) {
                        console.dir(err);
                        Notification.open({
                            duration: 2500,
                            message: `There was an issue with subscribing! Please try again.`,
                            position: 'is-bottom-right',
                            type: 'is-danger',
                            hasIcon: true
                        });
                    })
                    .finally(function () {
                        self.isLoading = false;
                    });
            },
            unsubscribe: function () {
                let self = this;
                self.isLoading = true;

                PaymentService.unsubscribe()
                    .then(function (res) {
                        if (res && res.status === 200) {
                            Notification.open({
                                duration: 2000,
                                message: res.data ? res.data : 'Plan successfully unsubscribed!',
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });

                            PaymentService.currentPlan()
                                .then(function (res) {
                                    if (res && res.status === 200) {
                                        self.currentPlan = res.data;
                                    }
                                });
                        }
                    })
                    .catch(function (err) {
                        console.dir(err);
                        Notification.open({
                            duration: 2500,
                            message: `There was an issue with unsubscribing! Please try again.`,
                            position: 'is-bottom-right',
                            type: 'is-danger',
                            hasIcon: true
                        });
                    })
                    .finally(function () {
                        self.isLoading = false;
                    });
            },
        },
    }
</script>

<style scoped>

</style>
