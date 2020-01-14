<template>
    <section class="hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">
                    Settings
                </h1>
                <b-tabs type="is-toggle" expanded>
                    <b-tab-item label="Profile" icon="user">
                        <form method="put">
                            <b-field label="Name">
                                <b-input v-model="user.name" />
                            </b-field>
                        </form>
                    </b-tab-item>
<!--                    <b-tab-item label="Music" icon="library-music"></b-tab-item>-->
<!--                    <b-tab-item label="Videos" icon="video"></b-tab-item>-->
                    
                    <b-button type="is-primary" 
                            native-type="submit" 
                              value="Update" />
                </b-tabs>
            </div>
        </div>
    </section>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    
    export default {
        name: 'settings-index',
        computed: {
            ...mapGetters('oidcStore', [
                'oidcIsAuthenticated',
                'oidcAuthenticationIsChecked',
                'oidcUser',
                'oidcIdToken',
                'oidcIdTokenExp'
            ]),
        },
        data: function () {
            return {
                user: {}
            }
        },
        mounted: function() {
            this.user = this.oidcUser;
            
            // console.dir(this.user);
        }
    }
</script>