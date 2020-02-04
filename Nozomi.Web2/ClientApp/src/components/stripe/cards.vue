<template>
    <div>
        <b-field class="pb-2" grouped group-multiline>
            <StripeCardModal v-if="id"/>
        </b-field>
        <b-carousel-list v-if="id && !isLoading"
                         v-model="carouselPage"
                         :arrow="false" :data="cards" :items-to-show="2">
            <template slot="item" slot-scope="props">
                <div class="card" v-if="props.list && props.list.card">
<!--                    <div class="card-image">-->
<!--                        <figure class="image is-2by1">-->
<!--                            <a @click="info(props.index)"><img :src="props.list.image"></a>-->
<!--                        </figure>-->
<!--                    </div>-->
                    <div class="card-content">
                        <div class="content">
                            <p class="title is-6">{{ props.list.card.brand }} ending with {{ props.list.card.last4 }}</p>
<!--                            <p class="subtitle is-7" v-if="props.list.billing_details && props.list.billing_details.name">-->
<!--                                {{ props.list.billing_details.name }}</p>-->
                            <div class="field is-grouped">
                                <p class="control">expiring on {{ props.list.card.exp_month }}/{{ props.list.card.exp_year }}</p>
                                <p class="control" v-if="cards.length > 1" style="margin-left: auto">
                                    <button @click="removePaymentMethod(props.list.id)"
                                            class="button is-small is-danger is-outlined">
                                        <b-icon size="is-small" icon="trash"/>
                                    </button>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
        </b-carousel-list>
        <section class="hero is-medium" v-else-if="!isLoading && !rawId">
            <div class="hero-body">
                <div class="container has-text-centered" v-if="!id">
                    <h1 class="title">
                        Well, we gotta setup billing first!
                    </h1>
                    <h2 class="subtitle">
                        <b-button @click="bootstripe" type="is-info" rounded>Get me started!</b-button>
                    </h2>
                </div>
                <b-loading :is-full-page="false" :active.sync="isBootstripeRunning" :can-cancel="true"/>
            </div>
        </section>
        <b-loading :is-full-page="false" :active.sync="isLoading" :can-cancel="true"/>
    </div>
</template>

<script>
    import StripeCardModal from "@/components/stripe/card-modal";
    import NozomiAuthService from "@/services/NozomiAuthService";
    import PaymentService from "@/services/auth/PaymentService";
    import {NotificationProgrammatic as Notification} from 'buefy';

    export default {
        name: 'cards',
        components: {StripeCardModal},
        props: {
            rawId: String,
        },
        data: function () {
            return {
                id: this.rawId,
                isBootstripeRunning: false,
                isLoading: true,
                carouselPage: 0,
                cards: [],
            };
        },
        beforeMount: function () {
            let self = this;

            if (!self.id) {
                PaymentService.getStripeCustId()
                    .then(function (res) {
                        if (res && res.status === 200 && res.data) {
                            self.id = res.data;

                            PaymentService.listPaymentMethods()
                                .then(function (res) {
                                    self.cards = res.data;
                                });
                        }
                    })
                    .catch(function (err) {
                    })
                    .finally(function () {
                        self.isLoading = false;
                    });
            } else {
                self.isLoading = false;
            }
        },
        methods: {
            bootstripe: function () {
                let self = this;
                self.isBootstripeRunning = true;

                NozomiAuthService.bootstripe()
                    .then(function (res) {
                        if (res.status === 200) {
                            Notification.open({
                                duration: 2500,
                                message: `Stripe successfully set up!`,
                                position: 'is-bottom-right',
                                type: 'is-success',
                                hasIcon: true
                            });

                            // Inform the parent that a new request has been created
                            // https://forum.vuejs.org/t/passing-data-back-to-parent/1201
                            self.$emit('created', true);

                            // Update the user
                            // TODO: Remove this, make the watch function work gracefully
                            self.$nextTick(function () {
                                if (!self.id) {
                                    PaymentService.getStripeCustId()
                                        .then(function (res) {
                                            if (res && res.status === 200 && res.data) {
                                                self.id = res.data;
                                            }
                                        })
                                        .catch(function (err) {
                                            console.dir(err);
                                            Notification.open({
                                                duration: 2500,
                                                message: `There might've been a communication error, please try again!`,
                                                position: 'is-bottom-right',
                                                type: 'is-danger',
                                                hasIcon: true
                                            });
                                        });
                                }
                            });
                        } else {
                            Notification.open({
                                duration: 2500,
                                message: `There might be a connection issue with Stripe. Please try again in a moment!`,
                                position: 'is-bottom-right',
                                type: 'is-warning',
                                hasIcon: true
                            });
                        }
                    })
                    .catch(function (err) {
                        Notification.open({
                            duration: 2500,
                            message: `There was an issue setting up stripe, please try again!`,
                            position: 'is-bottom-right',
                            type: 'is-danger',
                            hasIcon: true
                        });
                    }).finally(function () {
                    self.isBootstripeRunning = false;
                });
            },
            removePaymentMethod: function (id) {
                let self = this;

                // Client-side validation
                if (id && self.cards && self.cards.length > 1) {
                    self.$buefy.dialog.confirm({
                        message: 'Are you sure you want to delete this card?',
                        onConfirm: () => {
                            self.$buefy.toast.open('Deleting the card!');

                            self.isLoading = true;
                            PaymentService.removePaymentMethod(id)
                                .then(function (res) {
                                    if (res.status === 200) {
                                        Notification.open({
                                            duration: 2500,
                                            message: res.data ? res.data : "Card successfully removed!",
                                            position: 'is-bottom-right',
                                            type: 'is-success',
                                            hasIcon: true
                                        });
                                    }
                                })
                                .catch(function (err) {
                                    Notification.open({
                                        duration: 2500,
                                        message: "Please ensure you have more than 1 payment method before deleting!",
                                        position: 'is-bottom-right',
                                        type: 'is-danger',
                                        hasIcon: true
                                    });
                                })
                                .finally(function () {
                                    self.isLoading = false;
                                });
                        }
                    });
                } else {
                    Notification.open({
                        duration: 2500,
                        message: `You only have one card!`,
                        position: 'is-bottom-right',
                        type: 'is-danger',
                        hasIcon: true
                    });
                }
            },
        },
        watch: {
            // TODO: Since the parent will update, this should handle the receiving of new data
            rawId: function (newVal, oldVal) {
                if (newVal && newVal !== oldVal) {
                    this.id = newVal;
                }
            },
        },
    }
</script>