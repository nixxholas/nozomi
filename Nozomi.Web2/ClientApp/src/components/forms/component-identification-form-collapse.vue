<template>
    <b-collapse class="card" animation="slide" :open="!isCollapsed">

        <!-- Trigger -->
        <div slot="trigger"
             slot-scope="props"
             class="card-header"
             role="button"
        >
            <p class="card-header-title">
                {{ title }}
            </p>

            <a class="card-header-icon">
                <b-icon :icon="props.open ? 'caret-up' : 'caret-down'">
                </b-icon>
            </a>
        </div>

        <!-- Body -->
        <div class="card-content">
            <div class="content">

                <!-- CONDITION: Array -->
                <div v-if="Array.isArray(data)">
                    <div v-for="(row, rowIndex) of data"
                         :key="rowIndex"
                         :class="{'box': !isNested}"
                    >
                        <div v-for="(propertyKey, index) in Object.keys(row)"
                             :key="propertyKey"
                        >

                            <!-- User friendly value -->
                            <div v-if="typeof row[propertyKey] === 'number' || typeof row[propertyKey] === 'string'">
                                <!-- TODO: Prepare for update using ":value" -->
                                <b-checkbox
                                        @input="setSelectedIdentifier($event, { 
                                        identifier: getIdentifier(propertyKey + '=>' + row[propertyKey]),
                                        query: getQuery(propertyKey)
                                    })"
                                        expanded
                                >
                                    {{ propertyKey }}: {{ row[propertyKey] }}
                                </b-checkbox>
                            </div>

                            <!-- Recursive collapse -->
                            <component-identification-collapse
                                    v-else-if="Array.isArray(row[propertyKey])"
                                    :is-nested="true"
                                    :title="propertyKey"
                                    :data="row[propertyKey]"
                                    :appended-identifier="getIdentifier(propertyKey)"
                                    :appended-query="getQuery(propertyKey)"
                                    @setSelectedIdentifier="setSelectedIdentifier"
                            ></component-identification-collapse>

                            <!-- Recursive Nested Object -->
                            <component-identification-collapse
                                    v-else-if="typeof row[propertyKey] === 'object'"
                                    :is-nested="true"
                                    :title="propertyKey"
                                    :data="row[propertyKey]"
                                    :appended-identifier="getIdentifier(propertyKey)"
                                    :appended-query="getQuery(propertyKey)"
                                    @setSelectedIdentifier="setSelectedIdentifier"
                            ></component-identification-collapse>

                            <hr class="divider" v-if="index < (Object.keys(data).length - 1)"/>
                        </div>
                    </div>

                </div>


                <!-- NOT CLEAN CODE... Same as  above logic -->
                <!-- CONDITION: Object -->
                <div v-else-if="typeof data === 'object'"
                     :class="{'box': !isNested}"
                >

                    <div v-for="(propertyKey, index) of Object.keys(data)"
                         :key="propertyKey"
                    >
                        <!-- User friendly value -->
                        <div v-if="typeof data[propertyKey] === 'number' || typeof data[propertyKey] === 'string'">
                            <!-- TODO: Prepare for update using ":value" -->
                            <b-checkbox
                                    @input="setSelectedIdentifier($event, { 
                                        identifier: getIdentifier(propertyKey + '=>' + data[propertyKey]),
                                        query: getQuery(propertyKey)
                                    })"
                                    expanded
                            >
                                {{ propertyKey }}: {{ data[propertyKey] }}
                            </b-checkbox>
                        </div>

                        <!-- Recursive collapse -->
                        <component-identification-collapse
                                v-else-if="Array.isArray(data[propertyKey])"
                                :is-nested="true"
                                :title="propertyKey"
                                :data="data[propertyKey]"
                                :appended-identifier="getIdentifier(propertyKey)"
                                :appended-query="getQuery(propertyKey)"
                                @setSelectedIdentifier="setSelectedIdentifier"
                        ></component-identification-collapse>

                        <!-- Recursive Nested Object -->
                        <component-identification-collapse
                                v-else-if="typeof data[propertyKey] === 'object'"
                                :is-nested="true"
                                :title="propertyKey"
                                :data="data[propertyKey]"
                                :appended-identifier="getIdentifier(propertyKey)"
                                :appended-query="getQuery(propertyKey)"
                                @setSelectedIdentifier="setSelectedIdentifier"
                        ></component-identification-collapse>

                        <hr class="divider" v-if="index < (Object.keys(data).length - 1)"/>
                    </div>
                </div>

            </div>

        </div>

    </b-collapse>
</template>

<script>
    export default {
        name: 'component-identification-collapse',
        props: {
            title: {
                type: String,
                required: false,
                default: ""
            },
            data: {
                required: true,
                default: () => ([]),
                validator: function (value) {
                    return Array.isArray(value) || typeof value === 'object';
                }
            },
            appendedIdentifier: {
                type: String,
                required: false,
                default: ""
            },
            appendedQuery: {
                type: String,
                required: false,
                default: ""
            },
            isCollapsed: {
                type: Boolean,
                require: false,
                default: true
            },
            isNested: {
                type: Boolean,
                require: false,
                default: false
            }
            // TODO: Receive selected identifiers from parent for updates
        },
        methods: {
            getIdentifier(identifier) {
                if (this.appendedIdentifier.length > 0) {
                    return this.appendedIdentifier.concat("/" + identifier);
                }

                return identifier;
            },
            getQuery(query) {
                if (this.appendedQuery.length > 0) {
                    return this.appendedQuery.concat("/" + query);
                }

                return query;
            },
            setSelectedIdentifier(checked, identifier) {
                this.$emit("setSelectedIdentifier", checked, identifier);
            }
        }
    }
</script>`