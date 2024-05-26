<template>
    <section class="dropdown-wrapper">
        <div class="selected-item" @click="isVisible = !isVisible">
            <span v-if="modelValue">{{ modelValue }}</span>
            <span v-else></span>
            <svg ref="icon" class="drop-down-icon" viewBox="0 0 24 24" width="24" height="24" fill="currentColor">
                <path d="M11.9999 10.8284L7.0502 15.7782L5.63599 14.364L11.9999 8L18.3639 14.364L16.9497 15.7782L11.9999 10.8284Z"></path>
            </svg>
        </div>
        <div ref="popover" class="dropdown-popover">
            <input v-model="searchQuery" type="text" placeholder="Поиск" />
            <div class="options">
                <ul>
                    <li @click="selectItem(item)" v-for="(item, index) in filteredOptions" :key="`item-${index}`">
                        {{ item }}
                    </li>
                </ul>
            </div>
        </div>
    </section>
</template>

<script>
    export default {
        props: {
            modelValue: {
                type: String,
                required: true
            },
            options: {
                type: Object,
                required: true
            }
        },
        data() {
            return {
                searchQuery: '',
                isVisible: false
            }
        },
        computed: {
            filteredOptions() {{
                const query = this.searchQuery.toLowerCase();

                if (this.searchQuery == '') {
                    return this.options;
                }

                return this.options.filter((word) =>             
                    String(word).toLowerCase().includes(query)                
                );
            }}
        },
        watch: {
            isVisible() {
                var icon = this.$refs.icon;
                icon.classList.toggle("dropdown");

                var popover = this.$refs.popover;
                popover.classList.toggle("visible");
            }
        },
        emits: ['update:modelValue'],
        methods: {
            selectItem(item) {
                this.$emit('update:modelValue', item);
                this.searchQuery = '';
                this.isVisible = false;      
            }
        }
    };
</script>

<style scoped>
    .dropdown-wrapper {
        max-width: 220px;
        position: relative;
        margin: 0 auto;
    }

    .selected-item {
        height: 30px;
        border: 2px solid lightgray;
        border-radius: 10px;
        padding: 5px 10px;
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 16px;
    }

    .dropdown-popover {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        position: absolute;
        border: 2px solid lightgray;
        top: 37px;
        left: 0;
        right: 0;
        background-color: #fff;
        max-width: 100%;
        padding: 5px;
        visibility: hidden;
        transition: all 0.3s linear;
        max-height: 0;
        overflow: hidden;
    }

    .dropdown-popover input {
        width: 90%;
        height: 30px;
        border: 2px solid lightgray;
        border-radius: 5px;
        font-size: 16px;
        padding-left: 8px;
        outline: none;
    }

    .dropdown-popover input:hover {
        border-color: #339989;
    }

    .visible {
        max-height: 450px;
        visibility: visible;
    }

    .options {
        width: 100%;
    }

    .options ul {
        list-style: none;
        text-align: left;
        padding-left: 8px;
        max-height: 28vh;
        overflow-y: scroll;
        overflow-x: hidden;
    }

    .options ul li {
        width: 100%;
        border-bottom: 1px solid lightgray;
        padding: 10px;
        background-color: #f1f1f1;
        cursor: pointer;
        font-size: 16px;
    }

    .options ul li:hover {
        background: #339989;
        color: #fff;
    }

    .drop-down-icon {
        transform: rotate(180deg);
        transition: all 0.4s ease;      
    }

    .dropdown {
        transform: rotate(0deg);
        transition: all 0.4s ease;
    }
</style>