<template>
    <div>
        <b-field grouped group-multiline>
        </b-field>
        <b-carousel-list v-if="id && !isLoading"
                         v-model="carouselPage" :data="cards" :items-to-show="2">
            <template slot="item" slot-scope="props">
                <div class="card">
                    <div class="card-image">
                        <figure class="image is-5by4">
                            <a @click="info(props.index)"><img :src="props.list.image"></a>
                        </figure>
                        <b-tag type="is-danger" rounded style="position: absolute; top: 0;"><b>50%</b></b-tag>
                    </div>
                    <div class="card-content">
                        <div class="content">
                            <p class="title is-6">{{ props.list.title }}</p>
                            <p class="subtitle is-7">@johnsmith</p>
                            <div class="field is-grouped">
                                <p class="control" v-if="props.list.rating">
                                    <b-rate :value="props.list.rating" show-score disabled/>
                                </p>
                                <p class="control" style="margin-left: auto">
                                    <button class="button is-small is-danger is-outlined">
                                        <b-icon size="is-small" icon="heart"/>
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
    import NozomiAuthService from "@/services/NozomiAuthService";
    import PaymentService from "@/services/auth/PaymentService";
    import {NotificationProgrammatic as Notification} from 'buefy';

    export default {
        name: 'cards',
        props: {
            custId: {
                type: String,
                default: null
            },
        },
        data: () => {
            return {
                id: null,
                isBootstripeRunning: false,
                isLoading: true,
                carouselPage: 0,
                cards: [],
            };
        },
        created: function () {
            let self = this;

            if (self.custId)
                self.id = self.custId;

            if (!self.id) {
                PaymentService.getStripeCustId()
                    .then(function (res) {
                        if (res && res.status === 200 && res.data)
                            self.id = res.data;
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
                
                if (!self.id) {
                    self.isLoading = true;
                    // Then set the id again
                    PaymentService.getStripeCustId()
                        .then(function (res) {
                            if (res && res.status === 200 && res.data)
                                self.id = res.data;
                        })
                        .catch(function (err) {
                        })
                        .finally(function() {
                            self.isLoading = false;
                        });
                }
            }
        },
        watch: {
            custId: function (newVal, oldVal) { // watch it
                this.id = newVal;
            }
        }
    }
</script>