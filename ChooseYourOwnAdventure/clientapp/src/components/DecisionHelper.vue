<template>
    <div class="decision" v-show="decisionNode.depth == 0 || showAllDecisions">
        <div :style="indent" class="description">
            <a href="#"
               @click="onDecisionSelect(decisionNode)"
               :class="selectedDecision">
                {{ decisionNode.description }}
            </a>
        </div>

        <div :style="halfWidth"
             v-for="answer in decisionNode.childDecisions"
             :key="answer.decisionId">
            <DecisionHelper :ref="'decision' + answer.decisionId"
                            :decisionNode="answer"
                            :depth="depth + 1"
                            :showAllDecisions="showAllDecisions"
                            :selectedDecisionIds="selectedDecisionIds" />
        </div>
    </div>
</template>

<script>
    import { bus } from '../main'

    export default {
        name: 'DecisionHelper',
        props: {
            decisionNode: Object,
            depth: Number,
            showAllDecisions: {
                type: Boolean,
                default: false
            },
            selectedDecisionIds: Array
        },
        data() {
            return {
                selectedDecision: ''
            };
        },
        methods: {
            onDecisionSelect(decision) {
                this.showChildDecisions(decision);

                // Emit a selection event on the bus if there are children.
                // Else emit selection complete event.
                if (decision.childDecisions.length > 0)
                    bus.$emit('selectDecision', decision.decisionId);
                else
                    bus.$emit('selectDecisionComplete');
            },
            showChildDecisions(decision) {
                let vm = this;

                decision.childDecisions.forEach((child) => {
                    vm.$refs['decision' + child.decisionId][0].$el.style.display = "block";
                });
            }
        },
        computed: {
            indent() {
                return { transform: `translate(${this.depth * 10}px)` }
            },
            halfWidth() {
                return {
                    width: this.depth == 0
                        ? "50%"
                        : "100%",
                    display: "inline-block"
                };
            },
            decisionStyle() {
                return {
                    display: this.depth == 0
                        ? 'none' : 'inline-block'
                }
            }
        },
        watch: {
            showAllDecisions(val) {
                // If all decisions are visible, then highlight the ones user selected.
                if (val && this.selectedDecisionIds.indexOf(this.decisionNode.decisionId) > -1) {
                    this.selectedDecision = 'selected';
                }
            }
        }
    }
</script>

<style scoped>
    .selected {
        color: green;
        font-weight: bold;
    }
    .decision {
        padding: 10px;
    }
        .decision .description {
            border: 1px solid;
            padding: 10px;
        }
</style>
