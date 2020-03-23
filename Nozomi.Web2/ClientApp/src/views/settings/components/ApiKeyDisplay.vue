<template>
    <div>
        <ApiKeyModal :on-create="generateApiKey"
                     @keyCreated="onKeyGenerated"
        />

        <ApiKeyTable :api-keys="apiKeys"
                     :is-loading="isLoading"
                     @onRevoked="onKeyRevoked"
        />
    </div>
</template>

<script>
    import {NotificationProgrammatic as Notification} from 'buefy';
    import ApiKeyService from '@/services/auth/ApikeyService';
    import ApiKeyModal from './ApiKeyModal';
    import ApiKeyTable from './ApiKeyTable';

    export default {
        components: {
            ApiKeyTable,
            ApiKeyModal
        },
        data() {
            return {
                isLoading: false,
                apiKeys: []
            };
        },
        methods: {
            generateApiKey(label) {
                // Error events will be handled at component level,
                // returned a promise back to component who initiate 
                // this method
                return ApiKeyService.insert(label);
            },
            async getGeneratedKeys(params = {forceRefetch: false}) {
                const {forceRefetch} = params;
                if (this.apiKeys.length > 0 && !forceRefetch)
                    return;

                try {
                    this.isLoading = true;
                    this.apiKeys = await ApiKeyService.get();
                } catch (e) {
                    this.showErrorNotification(e.response.data.message ? e.response.data.message : e.message);
                } finally {
                    this.isLoading = false;
                }
            },
            onKeyGenerated() {
                this.getGeneratedKeys({forceRefetch: true});

                ApiKeyService.revealApiKey()
                    .then(data => {
                        this.$buefy.dialog.alert({
                            title: 'API Key created',
                            message: `
                                <div class="container has-text-centered">
                                    Please copy this key and save it somewhere safe. <br />
                                    <span class="has-text-danger">
                                        For security reasons, we cannot show it to you again.
                                    </span> <br /><br />
                                    <code class="has-text-grey is-size-8 p-3">
                                        ${data}
                                    </code>
                                </div>
                            `,
                            confirmText: 'Done',
                            canCancel: false
                        });
                    })
                    .catch(e => {
                        this.showErrorNotification(e.response.data.message ? e.response.data.message : e.message);
                    });
            },
            async onKeyRevoked({apiKeyGuid}) {
                try {
                    await ApiKeyService.remove(apiKeyGuid);
                    this.getGeneratedKeys({forceRefetch: true});
                } catch (e) {
                    this.showErrorNotification(e.response.data.message ? e.response.data.message : e.message);
                }
            },
            showErrorNotification(message) {
                Notification.open({
                    message,
                    duration: 2500,
                    type: 'is-danger',
                    position: 'is-bottom-right'
                });
            }
        },
        mounted() {
            this.getGeneratedKeys();
        }
    }
</script>