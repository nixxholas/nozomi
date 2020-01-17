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
                                <b-input v-model="user.name" disabled/>
                            </b-field>

                            <b-field
                                    :type="(!user.emailVerified && user.email) ? 'is-danger' : ''"
                                    :message="(!user.emailVerified && user.email) ? 'This email is pending verification' : ''"
                                    label="Email">
                                <b-input 
                                        v-model="user.email" disabled/>
                            </b-field>
                            
                            <b-field>
                                <b-button type="is-primary"
                                          native-type="submit"
                                          value="Update" disabled>
                                    Update
                                </b-button>
                            </b-field>
                        </form>
                    </b-tab-item>
                    <b-tab-item label="Billing" icon="money-bill">
                        
                    </b-tab-item>
<!--                    <b-tab-item label="Videos" icon="video"></b-tab-item>-->
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
            
            console.dir(this.user);
        }
    }
</script>