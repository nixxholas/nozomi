<template>
    <div>
        <section class="hero">
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">
                        Bugs & Issues
                    </h1>
                    
                    <TrelloListTabs />
                    
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    import TrelloListTabs from "@/components/trello/list-type-tab";
    
    export default {
        name: "bugs-and-issues",
        data: function() {
            return {}
        },
        components: {
            TrelloListTabs
        }
    }
</script>

<style scoped>

</style>