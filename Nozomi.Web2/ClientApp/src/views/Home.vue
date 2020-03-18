<template>
    <div>
        <section class="hero is-light is-medium">
            <div class="hero-body">
                <div class="container">
                    <div class="columns is-vcentered is-column-reverse-mobile">
                        <div class="column is-5 is-offset-1">
                            <div>
                                <h1 class="title is-1">
                                    Instruct. <br/>
                                    Preview. Consume.
                                </h1>
                                <p class="subtitle is-lineheight-one-half">
                                    Nozomi is the optimal workflow for backend teams. <br/>
                                    All-in-one: Job scheduler, restful polling, and
                                    aggregating results.
                                </p>
                                <button class="button is-primary is-large"
                                        @click="authenticateOidc(currentRoute)">
                                    Get started
                                </button>
                            </div>
                        </div>
                        <div class="column">
                            <img src="/assets/svg/undraw/undraw_Data_points_ubvs.svg"/>
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

    /* Mobile responsiveness */
    @media (max-width: 768px) {
        .is-column-reverse-mobile {
            display: flex;
            flex-direction: column-reverse;
        }
    }
</style>