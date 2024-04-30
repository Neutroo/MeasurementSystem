<script setup>
    import Error from './Error.vue'
</script>

<template>
    <div class="container">
        <div class="screen">
            <div class="device-data-form">
                <div class="date-field">
                    <label class="date-label" for="start-date">Начало:</label>
                    <input class="date-input" type="datetime-local" id="start-date" v-model="startDate">
                </div>
                <div class="date-field">
                    <label class="date-label" for="end-date">Конец:</label>
                    <input class="date-input" type="datetime-local" id="end-date" v-model="endDate">
                </div>
                <button class="download" @click="download">Скачать</button>
                <Error :message="error" />
            </div>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                startDate: '',
                endDate: '',
                loading: false,
                error: ''
            }
        },
        methods: {
            async download() {
                this.error = '';

                if (this.startDate == '') {
                    this.error = 'Не указано начальное время';
                    return;
                }
                else if (this.endDate == '') {
                    this.error = 'Не указано время окончания';
                    return;
                }

                this.loading = true;

                try {
                    const response = await fetch(`api/device/jsonfile?from=${this.startDate}&to=${this.endDate}`);

                    if (response.ok) {
                        const blob = await response.blob();
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.href = url;
                        a.download = 'content.json';
                        a.click();
                        this.error = '';
                    }
                    else {
                        if (response.status === 500) {
                            this.error = 'Status: 500. Internal Server Error.';
                        }
                        else {
                            this.error = await response.text();
                        }
                    }
                    this.loading = false;
                }
                catch (error) {
                    console.log(error);
                }
            }
        }
    });
</script>

<style scoped>
    .container {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 100vh;
    }

    .screen {
        background-color: #fffafb;
        min-height: 450px;
        width: 550px;
        box-shadow: 0px 0px 24px #246a73;
        border-radius: 30px;
    }

    .device-data-form {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 30px;
        margin-top: 30px;
    }

    .date-field {
        width: 60%;
        padding: 30px 0px;
        position: relative;
    }

    .date-label {
        color: #339989;
        font-weight: bold;
    }

    .date-input {
        background: none;
        border: none;
        border-bottom: 2px solid #D1D1D4;
        padding: 10px 0px;
        font-weight: 700;
        width: 100%;
        transition: .2s;
        text-align: center;
    }

    .date-input:active,
    .date-input:focus,
    .date-input:hover {
        outline: none;
        border-bottom-color: #339989;
        cursor: text;
    }

    .download {
        background: #339989;
        font-size: 14px;
        margin-top: 30px;
        padding: 16px 20px;
        border-radius: 15px;
        border: none;
        text-transform: uppercase;
        font-weight: 700;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 62%;
        color: #fff;
        cursor: pointer;
        transition: .2s;
    }

    .download:active,
    .download:hover {
        background-color: #29bca4;
        transform: scale(1.02);
    }
</style>