<template>
    <div>
        <section class="hero is-light is-medium">
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
                                    The API suite for data polling, storage, and aggregation - built for teams and enthusiasts
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

<!--                    <CompaniesComponent/>-->
                </div>
            </div>
        </section>

<!--        <BenefitsComponent />-->
        
        <p class="title is-1 has-text-centered has-text-grey mt-10">
            HOW WE WORK?
        </p>
        <FeatureListComponent />

    </div>
</template>

<script>
    import {mapActions} from 'vuex';
    // import CompaniesComponent from './components/companies';
    // import BenefitsComponent from './components/benefits';
    import FeatureListComponent from './components/feature-list';

    export default {
        data() {
            return {
                currentRoute: window.location.href
            };
        },
        components: {
            // CompaniesComponent,
            // BenefitsComponent,
            FeatureListComponent
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