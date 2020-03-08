<template>
    <div class="container">
        <b-loading 
            :is-full-page="false" 
            :active.sync="isLoading" 
            :can-cancel="false" 
        />
        
        <b-tabs
            v-model="activeTab"
            type="is-toggle" 
            expanded>

            <template v-for="(item, index) in trelloListItems">
                <b-tab-item :key="item.id" :label="item.name">
                    
                    <CardListBox 
                        :isActive="index===activeTab" 
                        :trelloIds="{board: trelloBoardId, list: item.id}" 
                    />
                    
                </b-tab-item>
            </template>

        </b-tabs>
    </div>
</template>

<script>
    import CardListBox from "./cardlist-box";
    import TrelloService from "@/services/TrelloService";
    
    export default {
        name: "trello-list-type-tab",
        
        data() {
            return {
                activeTab: 0,
                isLoading: false,
                errorMessage: null,
                
                trelloBoardId: "5e2c209ad3384a49871082bd",
                trelloListItems: []
            }
        },
        
        components: {
            CardListBox
        },
        
        mounted() {
            this.GetLists();
        },
        
        methods: {
            async GetLists() {
                this.isLoading = true;

                try {
                    this.trelloListItems = await TrelloService.getList(this.trelloBoardId);
                } catch(e) {
                    this.errorMessage = e.message;
                }

                this.isLoading = false;
            }
        }
    }
</script>

<style scoped>
    div.container {
        min-height: 20vh;
    }
</style>