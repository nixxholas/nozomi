<template>
    <section>
        <b-table :data="apiKeys"
                 :loading="isLoading"
                 :mobile-cards="true"
        >

            <template slot-scope="empty">
                <section class="section">
                    <div class="content has-text-grey has-text-centered">
                        <p>
                            <b-icon
                                    icon="frown"
                                    size="is-large">
                            </b-icon>
                        </p>
                        <p>Nothing here.</p>
                    </div>
                </section>
            </template>
            
            <template slot-scope="props">
                <b-table-column field="label" label="Key Name">
                    {{props.row.label}}
                </b-table-column>

                <b-table-column field="apiKeyMasked" label="API Key">
                    {{props.row.apiKeyMasked}}
                </b-table-column>

                <b-table-column>
                    <button class="button is-danger" @click="onRevokePressed(props.row.guid)">
                        Revoke
                    </button>
                </b-table-column>
            </template>
            
            <b-loading :active.sync="isLoading" :is-full-page="false" />

        </b-table>
    </section>
</template>

<script>
    export default {
        props: {
            isLoading: {
                type: Boolean,
                required: true,
                default: false
            },
            apiKeys: {
                type: Array,
                required: true,
                default: []
            }
        },
        methods: {
            onRevokePressed(apiKeyGuid) {
                this.$emit('onRevoked', { apiKeyGuid });
            }
        }
    }
</script>