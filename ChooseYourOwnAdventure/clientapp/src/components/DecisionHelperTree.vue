<template>
    <div>
        <DecisionHelper :decisionNode="decisionHelpersData"
                        :depth="0"
                        :showAllDecisions="showAllDecisions"
                        :selectedDecisionIds="selectedDecisionIds"/>
    </div>
</template>

<script>
    import DecisionHelper from './DecisionHelper.vue'
    import { bus } from '../main'

    export default {
        name: 'DecisionHelperTree',
        components: { DecisionHelper },
        props: {
            decisionHelpersData: Object
        },
        created() {
            // Store the decision selection in an array.
            bus.$on('selectDecision', (decisionId) => {
                this.selectedDecisionIds.push(decisionId);
            });

            // If all decision selection complete, then initiate showing all decisions.
            bus.$on('selectDecisionComplete', () => {
                this.showAllDecisions = true;
            });
        },
        data() {
            return {
                selectedDecisionIds: [],
                showAllDecisions: false
            };
        }
    }
</script>
