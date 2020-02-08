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
                    <button v-if="oidcIsAuthenticated && plan.id === currentPlan" class="button is-fullwidth"
                            disabled="disabled">Current plan
                    </button>
                    <button v-else-if="oidcIsAuthenticated" @click="subscribe(plan.id)"
                            class="button is-primary is-fullwidth">Choose
                    </button>
                    <button v-else class="button is-success" @click="authenticateOidc(currentRoute)">Sign up now!
                    </button>
                </div>
            </div>
        </div>
        <div v-else-if="currentPlan && viewMode" class="container">
            <h2 class="title">You are currently on </h2>
        </div>
        <div v-else-if="viewMode" class="container p-4">
            <h2 class="title">You currently <span class="has-text-danger">do not</span> have a plan.</h2>
        </div>
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
            currentPlan: {
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
                isLoading: true,
                plans: [],
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
        mounted() {
            let self = this;

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
            subscribe: function (planId) {
                let self = this;
                self.isLoading = true;

                PaymentService.subscribe(planId)
                    .then(function (res) {
                        console.dir(res);
                        Notification.open({
                            duration: 2000,
                            message: `Plan successfully subscribed!`,
                            position: 'is-bottom-right',
                            type: 'is-success',
                            hasIcon: true
                        });
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
        },
    }
</script>

<style scoped>

</style>
