<template>
    <b-modal :active="isModalOpen"
             :on-cancel="closeModal"
             :has-modal-card="true"
    >

        <div class="modal-card">
            <header class="modal-card-head">
                <p class="modal-card-title">
                    Select value to track with identifier
                </p>
            </header>
            <section class="modal-card-body">

                <ComponentIdentificationFormCollapse
                        :is-collapsed="false"
                        :data="data"
                        :appended-query="appendedQuery"
                        :title="identifier"
                        :selected-identifier="identifier"
                        @setSelectedIdentifier="valueSelected"
                />

            </section>
        </div>

    </b-modal>
</template>

<script>
    import ComponentIdentificationFormCollapse from "../forms/component-identification-form-collapse";

    export default {
        components: {
            ComponentIdentificationFormCollapse
        },
        props: {
            isModalOpen: {
                type: Boolean,
                required: true,
                default: false
            },
            data: {
                type: Object,
                required: true,
                default: () => ({})
            },
            identifier: {
                type: String,
                required: true,
                default: ""
            },
            appendedQuery: {
                type: String,
                required: true,
                default: ""
            }
        },
        methods: {
            closeModal() {
                this.$emit('closeModal')
            },

            valueSelected({query}) {
                this.$emit("completeSelection", {
                    query,
                    identifier: this.identifier
                });
            }
        }
    }
</script>