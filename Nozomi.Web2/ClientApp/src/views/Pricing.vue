<template>
    <section class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">
                    Pricing
                </h1>
                <h2 class="subtitle">
                    
                </h2>
                
                <Plans />
                
                <ul>
                    <li><i>Data retention of up to 7 days</i></li>
                </ul>
            </div>
        </div>
    </section>
</template>
<script>
    import {mapGetters} from "vuex";
    import Plans from "@/components/stripe/plans";
    
    export default {
        name: "Pricing",
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcAuthenticationIsChecked',
                'oidcUser',
                'oidcIdToken',
                'oidcIdTokenExp',
            ]),
        },
        components: {
            Plans
        },
    }
</script>