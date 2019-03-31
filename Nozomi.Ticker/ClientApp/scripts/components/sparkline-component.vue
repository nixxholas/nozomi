// https://medium.freecodecamp.org/getting-started-with-vue-single-file-components-using-webpack-2ae078058688
<template>
    <svg :id="id" class="sparkline" width="50" height="15" stroke-width="2"></svg>
</template>
<script>
    export default {
        name: "sparkline-div",
        data() {
            return {
                id: "spark1",
                dataset: []
            }
        },
        beforeMount: function() {
            sparkline(document.querySelector(".sparkline"), dataset);
        }
    }
</script>