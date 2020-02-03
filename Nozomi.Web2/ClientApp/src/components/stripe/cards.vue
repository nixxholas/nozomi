<template>
    <div>
        <b-field class="pb-2" grouped group-multiline>
            <StripeCardModal v-if="id"/>
        </b-field>
        <b-carousel-list v-if="id && !isLoading"
                         v-model="carouselPage"
                         :arrow="false" :data="cards" :items-to-show="3">
            <template slot="item" slot-scope="props">
                <div class="card" v-if="props.list && props.list.card">
                    <div class="card-image">
<!--                        <figure class="image is-2by1">-->
<!--                            <a @click="info(props.index)"><img :src="props.list.image"></a>-->
<!--                        </figure>-->
                    </div>
                    <div class="card-content">
                        <div class="content">
                            <p class="title is-6">{{ props.list.card.brand }} ending with {{ props.list.card.last4 }}</p>
                            <p class="subtitle is-7" v-if="props.list.billingDetails && props.list.billingDetails.name">{{ props.list.billingDetails.name }}</p>
                            <div class="field is-grouped">
                                <p class="control">expiring on {{ props.list.card.expMonth }}/{{ props.list.card.expYear }}</p>
                                <p class="control" style="margin-left: auto">
                                    <button @click="removePaymentMethod(props.list.id)" class="button is-small is-danger is-outlined">
                                        <b-icon size="is-small" icon="trash"/>
                                    </button>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
        </b-carousel-list>
        <section class="hero is-medium" v-else-if="!isLoading">
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
        props: ['custId'],
        data: function () {
            return {
                id: this.custId,
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
                                    console.dir(res);
                                    self.cards = res.data;
                                })
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
            removePaymentMethod: function(id) {
                console.dir(id);
            },
        }
    }
</script>