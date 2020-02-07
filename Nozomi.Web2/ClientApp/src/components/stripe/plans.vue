<template>
    <div>
        <div ref="table" class="pricing-table" v-if="!isLoading && plans && plans.length > 0">
            <div v-for="plan in plans" class="pricing-plan">
                <div class="plan-header">{{ plan.nickname }}</div>
                <div class="plan-price">
                    <span class="plan-price-amount">
                        <span class="plan-price-currency">$</span>
                        {{ plan.amount > 0 ? plan.amount / plan.amount_decimal : plan.amount }}
                    </span>/month
                </div>
                <div class="plan-items" v-if="plan.metadata && plan.metadata.length > 0">
                    <div v-for="metaKey of Object.keys(plan.metadata)" class="plan-item">{{ plan.metadata[metaKey] }} {{ metaKey }}</div>
                </div>
                <div class="plan-footer">
                    <button v-if="currentPlan && plan.id === currentPlan" class="button is-fullwidth" disabled="disabled">Current plan</button>
                    <button v-else class="button is-primary is-fullwidth">Choose</button>
                </div>
            </div>
        </div>
        <b-loading :is-full-page="false" :active.sync="isLoading" :can-cancel="false" />
    </div>
</template>

<script>
    import PaymentService from "@/services/auth/PaymentService";
    import {NotificationProgrammatic as Notification} from 'buefy';

    export default {
        name: 'plans',
        props: {
            currentPlan: {
                type: String,
                default: null,
            },
        },
        data() {
            return {
                isLoading: true,
                plans: []
            }
        },
        mounted() {
            let self = this;
            
            PaymentService.plans()
            .then(function(res) {
                self.plans = res.data;
            })
            .finally(function() {
                self.isLoading = false;
            });
        },
    }
</script>

<style scoped>

</style>
