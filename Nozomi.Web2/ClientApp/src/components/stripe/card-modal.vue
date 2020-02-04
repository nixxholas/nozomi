<template>
    <!--  TODO: Ensure cardelement is filled along with the rest of the form  -->
    <div>
        <b-button class="button is-info is-rounded"
                  icon-left="credit-card"
                  :loading="isLoading"
                  @click="openModal" v-if="!cardId">
            Add a card
        </b-button>
        <button class="button is-warning"
                :loading="isLoading"
                @click="openModal" v-else>
            Edit
        </button>

        <b-modal has-modal-card trap-focus :active.sync="isModalActive">
            <b-loading :active.sync="isModalLoading" :can-cancel="false"/>
            <!--https://stackoverflow.com/questions/48028718/using-event-modifier-prevent-in-vue-to-submit-form-without-redirection-->
            <form v-on:submit.prevent="create()" class="has-text-justified">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title" v-if="!cardId">Add a card</p>
                        <p class="modal-card-title" v-else>Edit a card</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field>
                            <b-input v-model="cardDetails.name" type="text" placeholder="Cardholder's Name"/>
                        </b-field>

                        <b-field>
                            <b-input v-model="cardDetails.email" type="email" placeholder="Billing Email"/>
                        </b-field>

                        <b-field>
                            <b-input v-model="cardDetails.address" type="text" placeholder="Billing Address"/>
                        </b-field>

                        <b-field grouped>
                            <b-field expanded>
                                <b-input v-model="cardDetails.city" placeholder="City"/>
                            </b-field>
                            <b-field expanded>
                                <b-input v-model="cardDetails.state" placeholder="State"/>
                            </b-field>
                            <b-field expanded>
                                <b-input v-model="cardDetails.postalCode" placeholder="Postal Code"/>
                            </b-field>
                        </b-field>

                        <div ref="cardelement"></div>
                        <p v-show="elementsError" id="card-errors" v-text="elementsError"/>
                    </section>

                    <footer class="modal-card-foot">
                        <button class="button" type="button" @click="closeModal">Close</button>
                        <button class="button is-primary" type="submit">Add</button>
                    </footer>
                </div>
            </form>
        </b-modal>
    </div>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    import {NotificationProgrammatic as Notification} from 'buefy';
    import PaymentService from "@/services/auth/PaymentService";

    export default {
        name: "stripe-card-modal",
        props: {
            currentRoute: window.location.href, // https://forum.vuejs.org/t/how-to-get-path-from-route-instance/26934/2
            cardId: {
                type: String,
                default: null
            }
        },
        computed: {
            ...mapGetters('oidcStore', [
                'oidcUser'
            ])
        },
        data: function () {
            return {
                isLoading: true,
                isModalActive: false,
                isModalLoading: false,
                complete: false,
                // Stripe variables
                cardDetails: {
                    name: '',
                    email: '',
                    address: '',
                    country: '',
                    city: '',
                    state: '',
                    postalCode: '',
                    phone: '',
                },
                card: null,
                elementsError: null,
                paymentMethod: 'card',
                stripe: null,
                stripePubKey: '',
                stripeSetupIntent: null,
            }
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc', 'signOutOidc']),
            createCardElement: function () {
                let self = this;

                // Check if stripe is up, else set it up
                if (!self.stripe && self.stripePubKey) {
                    self.stripe = Stripe(self.stripePubKey);
                } else {
                    console.dir("Can't seem to get stripe and it's pub key up");
                    return;
                }

                // SetupIntent configuration
                PaymentService.stripeSetupIntent()
                    .then(function (res) {
                        if (res.data) {
                            self.stripeSetupIntent = res.data;
                        }
                    });

                // Get stripe elements up
                const elements = self.stripe.elements({
                    // Use Roboto from Google Fonts
                    fonts: [
                        {
                            cssSrc: 'https://fonts.googleapis.com/css?family=Roboto',
                        },
                    ],
                    // Detect the locale automatically
                    locale: 'auto',
                });
                // Define CSS styles for Elements
                const style = {
                    base: {
                        color: "#32325D",
                        fontWeight: 500,
                        fontFamily: "Inter UI, Open Sans, Segoe UI, sans-serif",
                        fontSize: "16px",
                        fontSmoothing: "antialiased",

                        "::placeholder": {
                            color: "#CFD7DF"
                        }
                    },
                    invalid: {
                        color: "#E25950"
                    }
                };
                // Create the card element, then attach it to the DOM
                self.card = elements.create('card', {
                    iconStyle: "solid",
                    style: style
                });
                self.card.mount(self.$refs.cardelement);
                // Element focus ring
                self.card.on("focus", function () {
                    self.$refs.cardelement.classList.add("focused");
                });
                self.card.on("blur", function () {
                    self.$refs.cardelement.classList.remove("focused");
                });
                // Add an event listener: check for error messages as we type
                self.card.addEventListener('change', ({error}) => {
                    if (error) {
                        self.elementsError = error.message;
                    } else {
                        self.elementsError = '';
                    }
                });
            },
            create: function () {
                this.isModalLoading = true;

                let self = this;

                // Check if stripe is up, else set it up
                if (!self.stripe && self.stripePubKey) {
                    self.stripe = Stripe(self.stripePubKey);
                }
                
                if (self.stripeSetupIntent && self.stripeSetupIntent.client_secret
                    // Ensure that its not setup yet
                    && !self.stripeSetupIntent.payment_method) {
                    // Setup the card first through Stripe for PCI compliance
                    self.stripe.confirmCardSetup(
                        self.stripeSetupIntent.client_secret,
                        {
                            // https://stripe.com/docs/api/payment_methods/object
                            payment_method: {
                                type: 'card',
                                card: self.card,
                                billing_details: {
                                    address: {
                                        city: self.cardDetails.city,
                                        // TODO: Country Support
                                        // country: self.cardDetails.country,
                                        line1: self.cardDetails.address,
                                        "postal_code": self.cardDetails.postalCode,
                                        state: self.cardDetails.state,
                                    },
                                    email: self.cardDetails.email,
                                    name: self.cardDetails.name,
                                    // TODO: Phone number support
                                    // phone: self.cardDetails.phone,
                                },
                            },
                        }
                    ).then(function (result) {
                        if (result.error || !result.setupIntent) {
                            // Display error.message in your UI.
                            Notification.open({
                                duration: 2500,
                                message: result.error.message ? result.error.message 
                                    : "The current session may have been up for too long, please refresh!",
                                position: 'is-bottom-right',
                                type: 'is-danger',
                                hasIcon: true
                            });
                        } else if (result.setupIntent && result.setupIntent.payment_method) {
                            // Update the intent first
                            self.stripeSetupIntent = result.setupIntent;
                            
                            // Perform some conversion support for .NET Core because
                            // Stripe.NET doesn't seem to work well for SetupIntent
                            if (self.stripeSetupIntent.created && self.stripeSetupIntent.created > 0 
                                && self.stripeSetupIntent.payment_method) {
                                // Perform the relevant conversions gracefully
                                // let createdDate = new Date(0); // The 0 there is the key, which sets the date to the epoch
                                // createdDate.setUTCSeconds(self.stripeSetupIntent.created);
                                // result.setupIntent.created = createdDate;
                                
                                result.setupIntent.paymentMethodId = self.stripeSetupIntent.payment_method;
                                result.setupIntent.paymentMethodTypes = self.stripeSetupIntent.payment_method_types;

                                // The setup from Stripe has succeeded. Bind the token in our db with our user's data.
                                PaymentService.addPaymentMethod(result.setupIntent)
                                    .then(function(res) {
                                        if (res.status === 200) {
                                            Notification.open({
                                                duration: 2500,
                                                message: res.data ? res.data : 'New payment method successfully added!',
                                                position: 'is-bottom-right',
                                                type: 'is-success',
                                                hasIcon: true
                                            });
                                        }
                                    })
                                    .finally(function() {
                                        self.isModalLoading = false;
                                    });
                            }
                        }
                    })
                    .finally(function() {
                        self.isModalLoading = false;
                    });
                } else {
                    Notification.open({
                        duration: 2500,
                        message: "There was an issue with our payment gateway, please refresh and try again.",
                        position: 'is-bottom-right',
                        type: 'is-danger',
                        hasIcon: true
                    });
                    self.isModalLoading = false;
                }
            },
            openModal() {
                this.isModalActive = true;
                // https://stackoverflow.com/questions/60019294/vue-spa-single-file-component-element-binding-with-stripe-elements
                this.$nextTick(function () {
                    this.createCardElement()
                });
            },
            closeModal() {
                this.isModalActive = false;
            },
        },
        mounted: function () {
            let self = this;

            PaymentService.getStripePubKey()
                .then(function (res) {
                    self.stripePubKey = res.data;
                })
                .finally(function () {
                    self.isLoading = false;
                });
        },
    }
</script>

<style scoped>

</style>
