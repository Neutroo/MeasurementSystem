<script setup>
    import PauseIcon from './icons/IconPause.vue'
    import UnpauseIcon from './icons/IconUnpause.vue'
    import MonitoringItem from './MonitoringItem.vue'
</script>

<template>
    <div class="container">
        <div class="screen">
            <div>
            </div>
            <header>
                <div class="title-container">
                    <span class="title">Сообщения</span>
                    <button class="pause" @click="togglePause" @mouseenter="changeIconColor" @mouseleave="resetIconColor">
                        <PauseIcon v-if="!pause" :fillColor="iconColor" />
                        <UnpauseIcon v-else :fillColor="iconColor" />
                    </button>
                </div>
            </header>
            <main>
                 <table v-if="messages">
                     <tbody>
                         <tr v-for="message in messages" :key="message">
                             <MonitoringItem :data="message" />
                         </tr>
                     </tbody>
                 </table>
            </main>
        </div>
    </div>
</template>

<script>  
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {       
                messages: [{
                    "type": "Record",
                    "content": {
                        "date": "2024-05-06 17:55:54",
                        "deviceName": "Hydra-Lite",
                        "deviceSerial": "04",
                        "data": {
                            "system_Serial": "04",
                            "system_Version": "2024-02-10 (EXP)",
                            "system_RSSI": "-46",
                            "system_MAC": "34:94:54:95:C8:AC",
                            "system_IP": "10.40.53.20",
                            "BME280_temp": "19.63",
                            "BME280_humidity": "43.89",
                            "BME280_pressure": "755.03"
                        }
                    }
                },
                    {
                        "type": "Error",
                        "content": "Ошибка такая-то.."
                    }],
                polling: null,
                pause: false,
                iconColor: '#339989'
            }
        },
        mounted() {
            this.fetchData();
            this.polling = setInterval(() => {
                if (!this.pause) {
                    this.fetchData();
                }
            }, 3000);
        },
        unmounted() {
            clearInterval(this.polling);          
        },
        methods: {
            togglePause() {
                this.pause = !this.pause;
            },
            async fetchData() {
                try {
                    const response = await fetch('api/monitoring');

                    if (response.ok) {
                        const newMessages = await response.json();
                        this.messages = newMessages;
                    }
                    else {
                        if (response.status === 500) {
                            this.error = 'Status: 500. Internal Server Error.';
                        }
                        else {
                            this.error = await response.text();
                        }
                    }
                }
                catch (error) {
                    console.log(error);
                }
            },
            changeIconColor() {
                this.iconColor = '#29bca4';
            },
            resetIconColor() {
                this.iconColor = '#339989';
            }
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

    .container {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 100vh;
    }

    .screen {
        background-color: #fffafb;
        min-width: 70vh;
        box-shadow: 0px 0px 24px #246a73;
        border-radius: 30px;
    }

    main {
        display: flex;
        align-items: center;
        flex-direction: column;
        padding: 20px;
        padding-top: 10px;
    }

    tbody {
        display: block;
        max-height: 85vh;
        overflow: auto;
    }

    .title-container {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 20px;
        padding-bottom: 10px;
    }

    .title {
        font-size: 18px;
        color: #339989;
        text-transform: uppercase;
        font-weight: bold;
    }

    .pause {
        outline: none;
        border: none;
        background: none;
    }

    .pause:active,
    .pause:hover {
        fill: #29bca4;
        transform: scale(1.05);
        cursor: pointer;
    }
</style>