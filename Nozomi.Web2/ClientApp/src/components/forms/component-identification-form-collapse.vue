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
                            <div v-if="typeof row[propertyKey] === 'number' || typeof row[propertyKey] === 'string'"
                                 class="container is-paddingless is-marginless"
                            >

                                <!-- ShowButtons: TRUE -->
                                <div v-if="showButtons"
                                     class="columns"
                                >
                                    <div class="column is-four-fifths">

                                        {{ propertyKey }}: {{ row[propertyKey] }}

                                    </div>
                                    <div class="column">

                                        <b-tooltip
                                                label="(Recommended) Track value with this as an identifier to receive accurate value information"
                                                position="is-top"
                                                :delay="250"
                                                multilined
                                                animated
                                        >
                                            <b-button icon-right="key"
                                                      @click="setSelectedIdentifier({
                                                        data: row,
                                                        identifier: getIdentifier(propertyKey + '=>' + row[propertyKey]),
                                                        query: getQuery(propertyKey)
                                                      })"
                                            />
                                        </b-tooltip>

                                        <b-tooltip label="Track value without unique identifier"
                                                   position="is-top"
                                                   :delay="250"
                                                   animated
                                        >
                                            <b-button icon-right="eye"
                                                      @click="setSelectedIdentifier({
                                                        data: null,
                                                        identifier: null,
                                                        query: getQuery(propertyKey)
                                                      })"
                                            />
                                        </b-tooltip>

                                    </div>
                                </div>

                                <!-- ShowButtons: FALSE -->
                                <b-checkbox
                                        v-else
                                        @input="setSelectedIdentifier({
                                            data: row, 
                                            identifier: getIdentifier(propertyKey + '=>' + row[propertyKey]),
                                            query: getQuery(propertyKey)
                                        })"
                                        :disabled="shouldDisableCheckbox(propertyKey, row[propertyKey])"
                                >
                                    {{ propertyKey }}: {{ row[propertyKey] }}
                                </b-checkbox>
                            </div>

                            <!-- Recursive collapse -->
                            <component-identification-collapse
                                    v-else-if="Array.isArray(row[propertyKey])"
                                    :is-nested="true"
                                    :show-buttons="showButtons"
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
                                    :show-buttons="showButtons"
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
                        <div v-if="typeof data[propertyKey] === 'number' || typeof data[propertyKey] === 'string'"
                             class="container is-paddingless is-marginless"
                        >

                            <!-- ShowButtons: TRUE -->
                            <div v-if="showButtons"
                                 class="columns"
                            >
                                <div class="column is-four-fifths">

                                    {{ propertyKey }}: {{ data[propertyKey] }}

                                </div>
                                <div class="column">

                                    <b-tooltip
                                            label="(Recommended) Track value with this as an identifier to receive accurate value information"
                                            position="is-top"
                                            :delay="250"
                                            multilined
                                            animated
                                    >
                                        <b-button icon-right="key"
                                                  @click="setSelectedIdentifier({
                                                    data: data,
                                                    identifier: getIdentifier(propertyKey + '=>' + data[propertyKey]),
                                                    query: getQuery(propertyKey)
                                                  })"
                                        />
                                    </b-tooltip>

                                    <b-tooltip label="Track value without unique identifier"
                                               position="is-top"
                                               :delay="250"
                                               animated
                                    >
                                        <b-button icon-right="eye"
                                                  @click="setSelectedIdentifier({
                                                    data: null,
                                                    identifier: null,
                                                    query: getQuery(propertyKey)
                                                  })"
                                        />
                                    </b-tooltip>

                                </div>
                            </div>

                            <!-- ShowButtons: FALSE -->
                            <b-checkbox v-else
                                        @input="setSelectedIdentifier({
                                            data: data, 
                                            identifier: getIdentifier(propertyKey + '=>' + data[propertyKey]),
                                            query: getQuery(propertyKey)
                                        })"
                                        :disabled="shouldDisableCheckbox(propertyKey, data[propertyKey])"
                            >
                                {{ propertyKey }}: {{ data[propertyKey] }}
                            </b-checkbox>
                        </div>

                        <!-- Recursive collapse -->
                        <component-identification-collapse
                                v-else-if="Array.isArray(data[propertyKey])"
                                :is-nested="true"
                                :show-buttons="showButtons"
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
                                :show-buttons="showButtons"
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
            selectedIdentifier: {
                type: String,
                required: false,
                default: ""
            },
            showButtons: {
                type: Boolean,
                required: false,
                default: true
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
        },
        methods: {
            getIdentifier(identifier) {
                if (this.appendedIdentifier.length > 0) {
                    return this.appendedIdentifier.concat("/" + identifier);
                }

                return identifier;
            },
            getQuery(query) {
                if (this.appendedQuery.length > 0 && this.selectedIdentifier.length === 0) {
                    return this.appendedQuery.concat("/" + query);
                }

                return query;
            },
            shouldDisableCheckbox(propertyKey, dataValue) {
                return this.selectedIdentifier ===
                    this.getIdentifier(propertyKey + "=>" + dataValue);
            },
            setSelectedIdentifier({data = {}, identifier = "", query = ""}) {
                this.$emit("setSelectedIdentifier", {
                    data,
                    identifier,
                    query
                });
            }
        }
    }
</script>`