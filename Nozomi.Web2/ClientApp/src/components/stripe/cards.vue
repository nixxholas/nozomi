<template>
    <div>
        <b-field class="pb-2" grouped group-multiline>
            <StripeCardModal @created="refreshCards" v-if="id"/>
        </b-field>
        <b-collapse v-if="id && !isLoading"
                class="card"
                v-for="(collapse, index) of cards"
                :key="index"
                :open="collapseIndex == index"
                @open="collapseIndex = index">
            <div
                    slot="trigger"
                    slot-scope="props"
                    class="card-header"
                    role="button">
                <p class="card-header-title">
                    {{ collapse.card.brand | capitalize }} ending in {{ collapse.card.last4 }}
                    <b-taglist class="pl-2" attached>
                        <b-tag type="is-dark">expiring on</b-tag>
                        <b-tag type="is-warning">{{ collapse.card.exp_month }}
                            /{{collapse.card.exp_year }}</b-tag>
                    </b-taglist>
                </p>
                <a class="card-header-icon">
                    <b-icon
                            :icon="props.open ? 'caret-up' : 'caret-down'">
                    </b-icon>
                </a>
            </div>
            <div class="card-content">
                <div class="content">
                    <article class="media" v-if="collapse.billing_details">
                        <div class="media-content">
                            <div class="content">
                                <ul style="list-style-type: none;">
                                    <li><h4><b>Billing Details</b></h4></li>
                                    <li v-if="collapse.billing_details.name">
                                        <b>Name: </b> {{ collapse.billing_details.name }}
                                    </li>
                                    <li v-if="collapse.billing_details.email">
                                        <b>Email: </b> {{ collapse.billing_details.email }}
                                    </li>
                                    <li v-if="collapse.billing_details.phone">
                                        <b>Phone: </b> {{ collapse.billing_details.phone }}
                                    </li>
                                    <li v-if="collapse.billing_details.address.country">
                                        <b>Country: </b> {{ collapse.billing_details.address.country }}
                                    </li>
                                    <li>
                                        <b>Address: </b> {{ collapse.billing_details.address.line1 }}
                                    </li>
                                    <li v-if="collapse.billing_details.address.line2">
                                        <b>Suite: </b> {{ collapse.billing_details.address.line2 }}
                                    </li>
                                    <li>
                                        <b>City: </b> {{ collapse.billing_details.address.city }}
                                    </li>
                                    <li v-if="collapse.billing_details.address.state
                                    && collapse.billing_details.address.state !== '---------'">
                                        <b>State: </b> {{ collapse.billing_details.address.state }}
                                    </li>
                                    <li>
                                        <b>Postal Code: </b> {{ collapse.billing_details.address.postal_code }}
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </article>
                </div>
            </div>
            <footer class="card-footer">
                <div v-if="cards.length > 1" class="card-footer-item">
                    <b-button @click="removePaymentMethod(collapse.id)"
                            type="is-danger" icon-left="trash" outlined>
                        Delete
                    </b-button>
                </div>
            </footer>
        </b-collapse>
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
                collapseIndex: 0,
                id: this.rawId,
                isBootstripeRunning: false,
                isLoading: true,
                carouselPage: 0,
                cards: [],
            };
        },
        beforeMount: function () {
            let self = this;

            console.dir(self.id);
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
                PaymentService.listPaymentMethods()
                    .then(function (res) {
                        self.cards = res.data;
                    })
                .finally(function() {
                    self.isLoading = false;
                });
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
            refreshCards: function () {
                let self = this;

                if (self.id) {
                    PaymentService.listPaymentMethods()
                        .then(function (res) {
                            self.cards = res.data;
                        })
                        .catch(function (err) {
                            console.dir(err);
                            Notification.open({
                                duration: 2500,
                                message: "Your cards list have been updated but there was an " +
                                    "issue refreshing, please refresh the page!",
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                            });
                        });
                }
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
                                        
                                        self.refreshCards();
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

<style scoped>

</style>
