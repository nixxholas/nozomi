<template>
    <b-table :data="data"
             :hoverable="true"
    >

        <template slot-scope="props">
            <b-table-column label="Identifier">
                {{ props.row.identifier ? props.row.identifier : "-" }}
            </b-table-column>

            <b-table-column label="Value">
                {{ props.row.query }}
            </b-table-column>

            <b-table-column>
                <b-button type="is-danger" @click="removeRow(props.index)">
                    Remove
                </b-button>
            </b-table-column>
        </template>

        <template slot="empty">
            <section class="section">
                <div class="content has-text-grey has-text-centered">
                    <p>
                        <b-icon
                                icon="key"
                                size="is-large">
                        </b-icon>
                    </p>
                    <p>
                        Get started by choosing a unique identifier below
                    </p>
                </div>
            </section>
        </template>

    </b-table>
</template>

<script>
    export default {
        props: {
            data: {
                type: Array,
                required: true,
                default: () => ([])
            }
        },
        methods: {
            removeRow(rowIndex) {
                this.data.splice(rowIndex, 1);
            }
        }
    }
</script>