<template>
    <section>
        <b-table v-if="payloadIsValid">
            
        </b-table>
        <b-message type="is-danger" has-icon>
            There was error with the request.
        </b-message>
    </section>
</template>

<script>
    export default {
        props: {
            dispatchPayload: {
                type: Object,
                required: true,
                default: function () {
                    return {
                        response: {
                            version: "1.1",
                            content: {}, // HTTP request object?
                            statusCode: 400,
                            reasonPhrase: "BADREQUEST",
                            headers: [],
                            trailingHeaders: [],
                            requestMessage: {
                                version: "1.1",
                                content: null,
                                method: {},
                                requestUri: "",
                                headers: [],
                                properties: {}
                            },
                            isSuccessStatusCode: false
                        },
                        payload: null
                    }
                }
            }
        },
        computed: {
            payloadIsValid: function() {
                console.dir(this.dispatchPayload);
                return this.dispatchPayload !== null && this.dispatchPayload.response !== null && this.dispatchPayload.payload !== null;
            }
        }
    }
</script>