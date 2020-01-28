<template>
    <div>
        <b-carousel-list v-if="custId" 
                v-model="carouselPage" :data="cards" :items-to-show="2">
            <template slot="item" slot-scope="props">
                <div class="card">
                    <div class="card-image">
                        <figure class="image is-5by4">
                            <a @click="info(props.index)"><img :src="props.list.image"></a>
                        </figure>
                        <b-tag type="is-danger" rounded style="position: absolute; top: 0;"><b>50%</b></b-tag>
                    </div>
                    <div class="card-content">
                        <div class="content">
                            <p class="title is-6">{{ props.list.title }}</p>
                            <p class="subtitle is-7">@johnsmith</p>
                            <div class="field is-grouped" >
                                <p class="control" v-if="props.list.rating">
                                    <b-rate :value="props.list.rating" show-score disabled/>
                                </p>
                                <p class="control" style="margin-left: auto">
                                    <button class="button is-small is-danger is-outlined"><b-icon size="is-small" icon="heart"/></button>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
        </b-carousel-list>
        <section class="hero is-medium" v-else>
            <div class="hero-body">
                <div class="container has-text-centered">
                    <h1 class="title">
                        Well, we gotta setup billing first!
                    </h1>
                    <h2 class="subtitle">
                        <b-button type="is-info" rounded>Get me started!</b-button>
                    </h2>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    export default {
        name: 'cards',
        props: {
            custId: null,
        },
        date: () => {
            return {
                carouselPage: 0,
                cards: [],
            };
        },
    }
</script>