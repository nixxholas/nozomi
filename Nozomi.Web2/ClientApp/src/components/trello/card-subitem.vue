<template>
    <div class="container">

        <nav class="level is-mobile">

            <div class="level-left">
                <!--     Show more       -->
<!--                <div v-if="hasSubItems"-->
<!--                    class="button is-primary" -->
<!--                     @click="SetExpanded(!isExpanded)"-->
<!--                >-->
<!--                    Expand-->
<!--                </div>-->
            </div>

            <div class="level-right">
                <!--     Badges       -->
                <span v-for="label in labels"
                      class="tag"
                      :key="label.id"
                      :class="colorMapping[label.color]"
                >
                    {{ label.name }}
                </span>
            </div>

        </nav>
        
        <!--    Extra cards to load    -->
<!--        <div v-if="isExpanded && hasSubItems" -->
<!--            class="container"-->
<!--        >-->
<!--            -->
<!--        </div>-->
        
    </div>
</template>

<script>
    import TrelloService from "@/services/TrelloService";
    
    export default {
        name: "trello-card-subitem",
        
        props: {
            hasSubItems: {
                type: Boolean,
                required: true,
                default: false
            },
            trelloIds: {
                type: Object,
                required: true,
                default: function() {
                    return {
                        board: null,
                        list: null,
                        card: null
                    };
                }
            },
            labels: {
                type: Array,
                required: true,
                default: function() {
                    return []
                }
            }
        },
        
        data() {
            return {
                isLoading: false,
                isLoaded: false,
                isExpanded: false,
                errorMessage: null,
                colorMapping: {
                    "blue": "is-blue",
                    "red": "is-red",
                    "orange": "is-orange",
                    "purple": "is-purple"
                },
                
                subItems: []
            };
        },
        
        methods: {
            async GetSubItems() {
                this.isLoading = true;
                
                try {
                    const { board, list, card } = this.trelloIds;
                    this.subItems = await TrelloService.getChecklists(board, list, card);
                    this.isLoaded = true;
                } catch(e) {
                    this.errorMessage = e.message;
                }
                
                this.isLoading = false;
            },
            
            SetExpanded(bool) {
                this.isExpanded = bool;
                
                if (bool && !this.isLoaded) {
                    this.GetSubItems();
                }
            }
        }
    }
</script>

<style scoped>
    .tag {
        margin-left: 5px;
    }
    
    .is-red {
        background-color: #eb5a46;
        color: #f9f9ff;
    }
    
    .is-blue {
        background-color: #0079bf;
        color: #f9f9ff;
    }
    
    .is-orange {
        background-color: #ff9f1a;
        color: #f9f9ff;
    }
    
    .is-purple {
        background-color: #c377e0;
        color: #f9f9ff;
    }
</style>