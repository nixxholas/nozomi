<template>
    <div>
        <section class="hero is-light is-medium pb-11">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered is-column-reverse-mobile">
                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    Gather. <br/>
                                    Stash. Compute.
                                </h1>
                                <p class="subtitle is-lineheight-one-half">
                                    Nozomi is the optimal API-based workflow for backend teams comprising of
                                    API polling, data storage, and aggregating results.
                                </p>
                                <button class="button is-primary is-large"
                                        @click="authenticateOidc(currentRoute)">
                                    Get started
                                </button>
                            </div>
                        </div>
                        <div class="column">
                            <img src="/assets/svg/undraw/undraw_mind_map_cwng.svg"/>
                        </div>
                    </div>

<!--                    <div class="has-text-centered mt-10">-->
<!--                        <p class="is-7 has-text-grey mb-5">PROUDLY SERVING WITH AMAZING COMPANIES</p>-->
<!--                        <div class="columns is-centered">-->
<!--                            -->
<!--                        </div>-->
<!--                    </div>-->
                </div>
            </div>
        </section>

<!--        <section class="hero is-medium mt-10">-->
<!--            <div class="hero-body is-paddingless">-->
<!--                <div class="container">-->
<!--                    <p class="title has-text-centered is-2">-->
<!--                        Key features-->
<!--                    </p>-->

<!--                    <div class="columns is-vcentered is-centered">-->

<!--                        <div class="column is-4">-->
<!--                            <figure class="image pt-5">-->
<!--                                <img src="/assets/svg/undraw/undraw_mind_map_cwng.svg"/>-->
<!--                            </figure>-->
<!--                            <div class="card-content">-->
<!--                                <div class="content">-->
<!--                                    <div class="container">-->
<!--                                        <h1 class="title is-4">Blazing fast</h1>-->
<!--                                        <p class="subtitle is-5">-->
<!--                                            Scalable, fast, and simple polling deployments.-->
<!--                                        </p>-->
<!--                                    </div>-->
<!--                                </div>-->
<!--                            </div>-->
<!--                        </div>-->

<!--                    </div>-->
<!--                </div>-->
<!--            </div>-->
<!--        </section>-->

        <section class="hero is-medium">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered">
                        <div class="column">
                            <img src="/assets/svg/undraw/undraw_create_f05x.svg"/>
                        </div>
                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    Gather and stash.
                                </h1>
                                <p class="subtitle is-lineheight-one-half is-4">
                                    Simple customized scheduled jobs to gather data automatically for 
                                    <strong class="has-text-primary is-underlined">you</strong>.
                                </p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>

        <section class="hero is-medium">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered">
                        <div class="column">
                            <img src="/assets/svg/undraw/undraw_abstract_x68e.svg"/>
                        </div>
                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    Mix and match.
                                </h1>
                                <p class="subtitle is-lineheight-one-half is-4">
                                    Gathered some social and financial data? <br />
                                    Mix them to compute and discover new 
                                    <strong class="has-text-primary is-underlined">possibilities</strong>!
                                </p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>

        <section class="hero is-medium">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered is-column-reverse-mobile">
                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    Aggregate anywhere.
                                </h1>
                                <p class="subtitle is-lineheight-one-half is-4">
                                    Values can be cross referenced to allow you to create 
                                    custom algorithms and definitions.
                                </p>
                            </div>
                        </div>
                        <div class="column">
                            <img src="/assets/svg/undraw/undraw_circuit_board_4c4d.svg"/>
                        </div>
                    </div>

                </div>
            </div>
        </section>

        <section class="hero is-medium">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered">
                        <div class="column">
                            <img src="/assets/svg/undraw/undraw_mind_map_cwng.svg"/>
                        </div>
                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    Gather. <br/>
                                    Stash. Compute.
                                </h1>
                                <p class="subtitle is-lineheight-one-half">
                                    Nozomi is the optimal API-based workflow for backend teams comprising of
                                    API polling, data storage, and aggregating results.
                                </p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        
    </div>
</template>

<script>
    import {mapActions} from 'vuex';

    export default {
        data() {
            return {
                currentRoute: window.location.href
            };
        },
        methods: {
            ...mapActions('oidcStore', ['authenticateOidc'])
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