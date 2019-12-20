<template>
    <div>
        <section class="hero">
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">
                        Changelog
                    </h1>
                    <div class="timeline">
                        <header class="timeline-header">
                            <span class="tag is-primary">Public Preview</span>
                        </header>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">8 December 2019 - <b>preview 1</b></p>
                                <ul>
                                    <li>Currency view UI improvements</li>
                                    <li>Currency-specific market pairs listing</li>
                                    <li>Compute engine general fixes and improvements</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">14 December 2019 - <b>preview 2</b></p>
                                <ul>
                                    <li>Currency view UI hotfixes</li>
                                    <li>Compute engine critical bug fixes and increased safetynet checks</li>
                                    <li>Compute engine - Fully compatible with Nozomi's complete rewrite</li>
                                    <li>Role-based Authorisation Control (RBAC) implementation</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">19 December 2019 - <b>preview 3</b></p>
                                <ul>
                                    <li>Dashboard - Requests table UI improvements</li>
                                    <li>Dashboard - Requests modification is now implemented.</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">21 December 2019 - <b>preview 4</b></p>
                                <ul>
                                    <li>General hotfixes and improvements across Nozomi Web</li>
                                    <li>Currency Pairs Creation Interface</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    export default {
        name: "changelog",
        data: function() {
            return {}
        }
    }
</script>

<style scoped>

</style>