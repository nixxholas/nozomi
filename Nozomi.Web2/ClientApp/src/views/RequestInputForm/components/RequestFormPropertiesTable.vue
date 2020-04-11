<template>
    <b-table :data="formData">

        <template slot-scope="props">
            <b-table-column label="Key">

                <b-autocomplete
                        v-model="props.row.key"
                        :data="filterPropertyTypes(props.row.key)"
                        field="key"
                        @blur="setPropertyType(props.index)"
                        @focus="addExtraTableRow(props.index)"
                        expanded
                ></b-autocomplete>

            </b-table-column>

            <b-table-column label="Value">
                <b-input v-model="props.row.value"
                         @blur="setPropertyType(props.index)"
                         @focus="addExtraTableRow(props.index)"
                         expanded
                />
            </b-table-column>

            <b-table-column>
                <b-button type="is-danger" @click="removeRow(props.index)">
                    Remove
                </b-button>
            </b-table-column>
        </template>

    </b-table>
</template>

<script>
    export default {
        props: {
            headerTypes: {
                type: Array,
                required: true,
                default: () => ([])
            },
            formData: {
                type: Array,
                required: true,
                default: () => ([])
            },
            allowCustomValue: {
                type: Boolean,
                default: false
            }
        },

        created() {
            this.addExtraTableRow();
        },

        methods: {
            addExtraTableRow(rowIndex = -1) {
                if ((this.formData.length - 1) === rowIndex) {
                    this.formData.push({key: "", value: ""});
                }
                else if (rowIndex === -1) {
                    // Adds a new row when rowIndex is not specified
                    this.formData.push({key: "", value: ""});
                }
            },

            filterPropertyTypes(input) {
                return this.headerTypes.filter(headerType => headerType.key.toLowerCase().includes(input.toLowerCase()));
            },
            
            getPropertyType(keyInput) {
                let defaultPropertyType = -1; // Represents invalid property type
                
                // Defaults to use the only property type
                if (this.headerTypes.length === 1) {
                    return this.headerTypes[0].value;
                }
                
                for (const headerType of this.headerTypes) {
                    if (headerType.key === keyInput)
                        return headerType.value;

                    // Retrieve custom type if valids
                    if (defaultPropertyType === -1 && headerType.key.toLowerCase().includes("custom"))
                        defaultPropertyType = headerType.value;
                }
                
                return defaultPropertyType;
            },

            setPropertyType(rowIndex) {
                const keyInput = this.formData[rowIndex].key;
                this.formData[rowIndex].type = this.getPropertyType(keyInput);
            },
            
            removeRow(rowIndex) {
                this.formData.splice(rowIndex, 1);
            }
        }
    }
</script>