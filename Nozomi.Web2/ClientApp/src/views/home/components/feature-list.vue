<template>
    <div>
        <section class="hero is-medium" v-for="feature in features" :key="feature.title">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered"
                         :class="{'is-column-reverse-mobile': feature.reversed}">

                        <div class="column is-5" v-if="!feature.reversed">
                            <img :src="feature.image"/>
                        </div>

                        <!-- Gap space between 2 columns -->
                        <div class="column is-1" v-if="!feature.reversed">
                        </div>

                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    {{ feature.title }}
                                </h1>
                                <p class="subtitle is-lineheight-one-half is-4"
                                   v-html="feature.subtitle">
                                </p>
                                <div v-if="feature.callToAction">
                                    <button class="button is-primary is-large">
                                        Explore
                                    </button>
                                </div>
                            </div>
                        </div>

                        <!-- Gap space between 2 columns -->
                        <div class="column is-1" v-if="feature.reversed">
                        </div>

                        <div class="column is-5" v-if="feature.reversed">
                            <img :src="feature.image"/>
                        </div>

                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                features: [
                    {
                        reversed: false,
                        image: "/assets/svg/undraw/undraw_create_f05x.svg",
                        title: "Gather and stash.",
                        subtitle: `
                             Simple customized scheduled jobs to gather data automatically for
                             <strong class="has-text-primary is-underlined">you</strong>.
                        `,
                        callToAction: null,
                    },
                    {
                        reversed: true,
                        image: "/assets/svg/undraw/undraw_abstract_x68e.svg",
                        title: "Mix and match.",
                        subtitle: `
                            Gathered some social and financial data? <br/>
                            Mix them to compute and discover new
                            <strong class="has-text-primary is-underlined">possibilities</strong>.
                        `,
                        callToAction: null,
                    },
                    {
                        reversed: false,
                        image: "/assets/svg/undraw/undraw_circuit_board_4c4d.svg",
                        title: "Aggregate anywhere.",
                        subtitle: `
                            Values can be cross referenced to allow you to create
                            <strong class="has-text-primary is-underlined">custom</strong>
                            algorithms and definitions.
                        `,
                        callToAction: null,
                    },
                    {
                        reversed: true,
                        image: "/assets/svg/undraw/undraw_server_cluster_jwwq.svg",
                        title: "Plug it anywhere.",
                        subtitle: `
                            We provide intuitive and optimized RESTful APIs
                            to allow you to 
                            <strong class="has-text-primary is-underlined">seamlessly integrate</strong> 
                            our services.
                        `,
                        callToAction: null,
                    }
                ]
            };
        }
    }
</script>

<style scoped>
    .is-lineheight-one-half {
        line-height: 1.5em;
    }

    .is-underlined {
        text-decoration: underline;
    }

    /* Mobile responsiveness */
    @media (max-width: 768px) {
        .is-column-reverse-mobile {
            display: flex;
            flex-direction: column-reverse;
        }
    }
</style>