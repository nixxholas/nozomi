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
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">23 December 2019 - <b>preview 5</b></p>
                                <ul>
                                    <li>.NET Core 3.1 framework upgrade</li>
                                    <li>Prepare for currency sources</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">29 December 2019 - <b>preview 6</b></p>
                                <ul>
                                    <li>.NET Core 3.1 framework compatibility update and hotfixing</li>
                                    <li>Backend sorting is now working as intended</li>
                                    <li>We literally added a huge supercharger to every part of th e app</li>
                                    <li>Some user interface elements finally get to work</li>
                                </ul>
                            </div>
                        </div>
                        <header class="timeline-header">
                            <span class="tag is-info">Public Beta</span>
                        </header>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">03 January 2019 - <b>beta 1</b></p>
                                <ul>
                                    <li><b>UI</b> - Our pricing plans are out bitches</li>
                                    <li><b>UI</b> - General hotfixes and improvements</li>
                                    <li><b>UI</b> - Well.. we couldn't really create currency pairs but now we can</li>
                                    <li><b>Cabin</b> - Components listing for requests</li>
                                    <li><b>Cabin</b> - Components for a request can now be created</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">11 January 2019 - <b>beta 2</b></p>
                                <ul>
                                    <li><b>Backend</b> - Complete database wipe for MVP preparations</li>
                                    <li><b>Backend</b> - Certain entities now carry a more obscure ID</li>
                                    <li><b>Backend</b> - Package updates for better .NET Core 3.1 compatibility</li>
                                    <li><b>UI</b> - General hotfixes and improvements</li>
                                    <li><b>Cabin</b> - Currency type management</li>
                                    <li><b>Cabin</b> - Source type and Currency type tables now have a self-update functionality</li>
                                    <li><b class="has-text-danger">Known Issue</b> - Some creation forms send a UI console error after the creation of an entity is complete</li>
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