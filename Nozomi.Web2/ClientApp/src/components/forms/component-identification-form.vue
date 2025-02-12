<template>
    <section>
        <!-- Display mapped data -->
        <ComponentIdentificationFormTable
                :data="selectedIdentifiers"
                class="mb-4"
        />

        <div class="buttons mb-7">
            <b-button @click="previousStep">
                Previous
            </b-button>

            <b-button @click="nextStep">
                Next
            </b-button>
        </div>

        <!-- Identity selection field -->
        <ComponentIdentificationFormCollapse
                :data="dispatchPayload.payload ? dispatchPayload.payload : []"
                :is-collapsed="false"
                @setSelectedIdentifier="setSelectedIdentifier"
        />

        <!-- Query selection Modal -->
        <ComponentIdentificationFormModal
                :is-modal-open="isDataSelectionModalOpen"
                :data="selectedIdentifierData"
                :identifier="selectedIdentifier"
                :appended-query="selectedIdentifierQuery"
                @closeModal="isDataSelectionModalOpen = false"
                @completeSelection="completeDataMapping"
        />

        <div class="buttons mt-7">
            <b-button @click="previousStep">
                Previous
            </b-button>

            <b-button @click="nextStep">
                Next
            </b-button>
        </div>
    </section>
</template>

<script>
    import ComponentIdentificationFormCollapse from "./component-identification-form-collapse";
    import ComponentIdentificationFormModal from "../modals/component-identification-form-modal";
    import ComponentIdentificationFormTable from "../tables/component-identification-form-table";

    export default {
        components: {
            ComponentIdentificationFormTable,
            ComponentIdentificationFormCollapse,
            ComponentIdentificationFormModal
        },
        props: {
            components: {
                type: Array,
                required: false,
                default: null
            },
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
                // Identifiers to display in data mapping table
                selectedIdentifiers: [],

                // Identifiers used in modal to prepare selection
                isDataSelectionModalOpen: false,
                selectedIdentifier: "",
                selectedIdentifierQuery: "",
                selectedIdentifierData: {}
            }
        },
        beforeUpdate() {
            if (this.components && this.components.length > 0) { // If components are loaded
                this.selectedIdentifiers = this.components;
            }
        },
        methods: {
            setSelectedIdentifier({data, identifier, query}) {
                if (data && identifier) {
                    this.isDataSelectionModalOpen = true;
                    this.selectedIdentifier = identifier;
                    this.selectedIdentifierQuery = query;
                    this.selectedIdentifierData = data;
                } else {
                    this.selectedIdentifiers.push({query, identifier});
                    this.displaySuccessDataMapSnackbar();
                }
            },

            completeDataMapping(mappedSelection) {
                this.selectedIdentifiers.push(mappedSelection);
                this.isDataSelectionModalOpen = false;

                this.displaySuccessDataMapSnackbar();
            },

            nextStep() {
                if (this.selectedIdentifiers.length > 0) {
                    this.$emit("setIdentifiedSelections", this.selectedIdentifiers);
                } else {
                    this.$buefy.dialog.alert("You have yet to select any identifiers");
                }
            },
            previousStep() {
                this.$emit("setActiveStep", -1);
            },

            displaySuccessDataMapSnackbar() {
                this.$buefy.snackbar.open({
                    duration: 5000,
                    message: "Data mapped, scroll up to review what's added.",
                    type: "is-success",
                    position: "is-bottom-right",
                    actionText: "Review",
                    queue: false,
                    onAction: () => {
                        window.scrollTo(0, 0);
                    }
                });
            }
        }
    }
</script>