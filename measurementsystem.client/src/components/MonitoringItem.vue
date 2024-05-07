<script setup>

</script>

<template>
    <div v-if="message.type === 'Record'" class="record-container">
        <div class="field">
            <div class="field-header">Имя</div>
            <div class="field-value">{{ message.content.deviceName }}</div>
        </div>
        <div class="field">
            <div class="field-header">Время</div>
            <div class="field-value">{{ message.content.date }}</div>
        </div>
        <div v-for="(value, key) in message.content.data" :key="key" class="field">
            <div class="field-header">{{ key }}</div>
            <div class="field-value">{{ value }}</div>
        </div>
    </div>
    <div v-else class="error-container">
        <div class="field">
            <div class="field-header">Ошибка</div>
            <div class="field-value">{{ message.content }}</div>
        </div>
    </div>
</template>

<script>
    import { defineComponent, toRaw } from 'vue';

    export default defineComponent({
        props: {
            data: {
                type: Object,
                required: true
            }
        },
        data() {
            return {
                message: null
            }
        },
        computed: {
            checkType() {
                return this.message.type === 'Record';
            }
        },
        beforeCreate() {
        },
        created() {
            this.message = toRaw(this.data);
        },
        methods: {

        }
    });
</script>

<style scoped>
    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
        font-family: Raleway, sans-serif;
    }

    .record-container {
        display: flex;
        align-items: center;
        justify-content: start;
        flex-wrap: wrap;
        border: 2px solid #339989;
        border-radius: 10px;
        gap: 10px;
        background: #ecf8f6;
        margin: 2px 0;
        padding: 5px;
    }

    .error-container {
        display: flex;
        align-items: center;
        justify-content: start;
        border: 2px solid #c32f27;
        border-radius: 10px;
        gap: 10px;
        background: #ffb3b3;
        margin: 2px 0;
    }

    .field {
        display: flex;
        flex-direction: column;
        gap: 3px;
        padding: 5px;
        font-size: 14px;
        border-radius: 10px;
        border: 2px solid rgba(51, 153, 137, 0.5);
    }

    .error-container .field {
        border: none;
    }

    .field-header {
        font-weight: bold;
        text-align: center;
    }

    .field-value {
        color: #495057;
        text-align: center;
    }

    .field-value {
        color: #495057;
        text-align: center;
    }

    .error-container .field-value {
        color: black;
        text-align: center;
    }
</style>