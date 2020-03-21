<template>
    <section>
        <b-button class="is-medium is-primary mt-5 mb-10"
                  @click="isModalActive = true"
        >
            Generate API key
        </b-button>

        <b-modal :active.sync="isModalActive"
                 has-modal-card
                 trap-focus
        >

            <form @submit.prevent="create">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">API Key generator</p>
                    </header>
                    <div class="modal-card-body">
                        <b-field label="Name your key">
                            <b-input v-model="label"
                                     type="text"
                                     placeholder="e.g. MyFirstProject"
                                     required
                            >
                            </b-input>
                        </b-field>
                    </div>
                    <div class="modal-card-foot">
                        <button class="button is-primary"
                                type="submit"
                        >
                            Generate
                        </button>
                    </div>
                </div>
            </form>

        </b-modal>
    </section>
</template>

<script>
    import {NotificationProgrammatic as Notification} from 'buefy';

    export default {
        props: {
            onCreate: {
                type: Function,
                required: true,
                default: () => {
                }
            }
        },
        data() {
            return {
                isModalActive: false,
                label: ""
            };
        },
        methods: {
            create() {
                this.onCreate(this.label)
                    .then(() => {
                        this.label = "";
                        this.isModalActive = false;
                        this.$emit('keyCreated', true);
                    })
                    .catch((error) => {
                        Notification.open({
                            duration: 2500,
                            type: 'is-danger',
                            position: 'is-bottom-right',
                            message: e.response.data.message ? e.response.data.message : e.message
                        });
                    });
            }
        }
    }
</script>