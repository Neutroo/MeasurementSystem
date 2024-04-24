<script setup>
    import ErrorIcon from './icons/IconError.vue'
</script>

<template>
    <div v-if="message" class="error-container">
        <div class="error-field">
            <ErrorIcon class="error-icon"></ErrorIcon>
            <span class="error-text">{{ message }}</span>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';

    export default defineComponent({
        props: {
            message: {
                type: String,
                required: true
            }
        }
    });
</script>

<style scoped>
    .error-container {
        width: 80%;
        display: flex;
        justify-content: center;
    }

    .error-field {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 20px 0px;
        gap: 5px;
    }

    .error-icon {
        min-width: 20px;
        min-height: 20px;
    }

    .error-text {
        font-weight: 700;
        color: #c32f27;
    }
</style>