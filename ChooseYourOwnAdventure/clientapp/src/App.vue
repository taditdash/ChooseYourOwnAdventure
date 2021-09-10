<template>
    <div id="app">
        <DecisionHelperTree :decisionHelpersData="decisionHelpersData" />
    </div>
</template>

<script>
    import DecisionHelperTree from './components/DecisionHelperTree.vue'

    export default {
        name: 'app',
        components: {
            DecisionHelperTree
        },
        created() {
            window.axios.get("api/DecisionHelper")
                .then((response) => {
                    this.decisionHelpersData = response.data;
                })
                .catch((error) => {
                    alert(error);
                });
        },
        data() {
            return {
                decisionHelpersData: {}
            };
        },
    }
</script>

<style>
    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
