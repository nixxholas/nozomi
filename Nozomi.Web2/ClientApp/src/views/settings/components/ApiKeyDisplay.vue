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
                    Notification.open({
                        duration: 2500,
                        type: 'is-danger',
                        position: 'is-bottom-right',
                        message: e.response.data.message ? e.response.data.message : e.message
                    });
                } finally {
                    this.isLoading = false;
                }
            },
            onKeyGenerated() {
                this.getGeneratedKeys({forceRefetch: true});
            },
            async onKeyRevoked({ apiKeyGuid }) {
                try {
                    await ApiKeyService.remove(apiKeyGuid);
                    this.getGeneratedKeys({forceRefetch: true});   
                }
                catch(e) {
                    Notification.open({
                        duration: 2500,
                        type: 'is-danger',
                        position: 'is-bottom-right',
                        message: e.response.data.message ? e.response.data.message : e.message
                    });
                }
            }
        },
        mounted() {
            this.getGeneratedKeys();
        }
    }
</script>