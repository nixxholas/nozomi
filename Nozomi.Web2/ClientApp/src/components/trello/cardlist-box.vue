    <template>
        <section class="container">
            <b-loading
                :is-full-page="false"
                :active.sync="isLoading"
                :can-cancel="false"
            />
            
            <div class="box" v-for="card in trelloCards" :key="card.id">
                <article class="media">
                    <div class="media-content">
                        <div class="content">
                            <p>
                                
                                <strong>{{ card.name }}</strong> 
                                <small> @ {{ FormatDate(card.dateLastActivity) }}</small>
                                <br />
                                <div v-html="ReplaceBreakWithBR(card.desc)"></div>
                                
                            </p>
                        </div>
                    </div>
                </article>
            </div>
        </section>
    </template>
    
    <script>
        import TrelloService from "@/services/TrelloService";
        
        export default {
            name: "trello-card-box",
            
            props: {
                isActive: {
                    type: Boolean,
                    required: true
                },
                trelloIds: {
                    type: Object,
                    required: true,
                    default: function() {
                        return {
                            board: null,
                            list: null
                        }
                    }
                }
            },
            
            data() {
                return {
                    isLoading: false,
                    isLoaded: false,
                    errorMessage: null,
                    
                    trelloCards: []
                }
            },
            
            mounted() {
                this.GetCards();
            },
            
            watch: {
                isActive: function() {
                    this.GetCards();
                }
            },

            methods: {
                async GetCards() {
                    if (this.isActive && !this.isLoaded) {
                        this.isLoading = true;

                        try {
                            const { board, list } = this.trelloIds;

                            this.trelloCards = await TrelloService.getCards(board, list);
                            this.isLoaded = true;
                        } catch(e) {
                            this.errorMessage = e.message;
                        }

                        this.isLoading = false;
                    }
                },
                
                ReplaceBreakWithBR(str) {
                    return str.replace("\n", "<br/>");
                },
                
                FormatDate(dateString) {
                    return Date(dateString).toLocaleDateString();
                }
            }
        }
    </script>
    
    <style scoped>
        section.container {
            min-height: 20vh;
        }
    </style>