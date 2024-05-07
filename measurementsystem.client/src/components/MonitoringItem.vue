<template>
    <div v-if="message.type === 'Record'" class="base-container record">
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
    <div v-else class="base-container error">
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
        created() {
            this.message = toRaw(this.data);
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

    .base-container {
        display: flex;
        align-items: center;
        justify-content: start;
        gap: 10px;
        border-radius: 10px;
        padding: 5px;
        margin: 3px 0;
    }

    .record {
        flex-wrap: wrap;
        border: 2px solid #339989;
        background: #ecf8f6;
    }

    .error {
        border: 2px solid #c32f27;
        background: #ffb3b3;
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

    .error .field {
        border: none;
    }

    .field-header {
        font-weight: bold;
        text-align: center;
    }

    .field-value {
        text-align: center;
    }
</style>