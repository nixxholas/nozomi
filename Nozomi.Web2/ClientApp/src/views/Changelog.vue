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
                                <p class="heading">8 December 2019 - <b>preview release candidate 1</b></p>
                                <ul>
                                    <li>Currency view UI improvements</li>
                                    <li>Currency-specific market pairs listing</li>
                                    <li>Compute engine general fixes and improvements</li>
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