<template>
    <section>
        <ComponentIdentificationFormCollapse
                :data="dispatchPayload.payload ? dispatchPayload.payload : []"
                :is-collapsed="false"
                @setSelectedIdentifier="setSelectedIdentifier"
        />

        <br/>
        <b-button @click="previousStep">
            Previous
        </b-button>

        <b-button @click="nextStep">
            Next
        </b-button>
    </section>
</template>

<script>
    import ComponentIdentificationFormCollapse from "./component-identification-form-collapse";

    export default {
        components: {
            ComponentIdentificationFormCollapse
        },
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
        data() {
            return {
                selectedIdentifiers: []
            }
        },
        methods: {
            setSelectedIdentifier(checked, identifier) {
                if (!checked) {
                    const identifierIndex = this.findIdentifierIndex(identifier);
                    this.selectedIdentifiers.splice(identifierIndex, 1);
                } else {
                    this.selectedIdentifiers.push(identifier);
                }
            },
            findIdentifierIndex(identifier) {
                const identifiersLength = this.selectedIdentifiers.length;

                for (let counter = 0; counter < identifiersLength; counter++) {
                    const selectedIdentifer = this.selectedIdentifiers[counter];

                    if (selectedIdentifer === identifier)
                        return counter;
                }

                return -1;
            },
            nextStep() {
                if (this.selectedIdentifiers.length > 0) {
                    this.$emit("setIdentifiedSelection", this.selectedIdentifiers);
                } else {
                    this.$buefy.dialog.alert("You have yet to select any identifiers");
                }
            },
            previousStep() {
                this.$emit("setActiveStep", -1);
            }
        }
    }
</script>