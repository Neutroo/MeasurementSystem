<script setup>
    import CrossIcon from './icons/IconCross.vue'
    import BackIcon from './icons/IconBack.vue'
    import EditIcon from './icons/IconEdit.vue'
    import Error from './Error.vue'
</script>

<template>
    <div class="container">
        <div class="screen">
            <header>
                <div v-if="!isCalibrateByDevice && !isCalibrateBySensor">
                    <div v-if="!isAddingForm" class="title-container">
                        <span class="title">Калибровочные данные</span>
                        <button class="open-adding-form" @click="toggleAddingForm">Добавить</button>
                    </div>
                    <div v-else class="title-container">
                        <span class="title">Добавление</span>
                        <button class="back" @click="toggleAddingForm">
                            <BackIcon />
                        </button>
                    </div>
                </div>
                <div v-if="isCalibrateByDevice" class="title-container">
                    <span class="title">Калибровка по прибору</span>
                    <button class="back" @click="toggleDeviceCalibrateForm">
                        <BackIcon />
                    </button>
                </div>
                <div v-if="isCalibrateBySensor" class="title-container">
                    <span class="title">Калибровка по датчику</span>
                    <button class="back" @click="toggleSensorCalibrateForm">
                        <BackIcon />
                    </button>
                </div>
            </header>
            <main>
                <div v-if="!isCalibrateByDevice && !isCalibrateBySensor">
                    <div v-if="!isAddingForm && calibrationItems" class="table-container">
                        <div class="select-container">
                            <span class="select-label">Выберите прибор</span>
                            <select v-model="tableSelectedDevice" class="custom-select">
                                <option selected v-for="(sensors, device) in calibrationItems" :value="device">
                                    {{ device }}
                                </option>
                            </select>
                        </div>
                        <table>
                            <thead>
                                <tr>
                                    <th class="sensor-field">Датчик</th>
                                    <th class="date-field">Время добавления</th>
                                    <th class="coef-field">Коэффициенты </th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in calibrationItems[tableSelectedDevice]" :key="item">
                                    <td class="sensor-field">{{ item.sensor }}</td>
                                    <td class="date-field">{{ item.creationDate }}</td>
                                    <td class="coef-field">{{ item.coefficients }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div v-if="isAddingForm" class="menu">
                        <button class="flat-button" @click="toggleDeviceCalibrateForm">По прибору</button>
                        <button class="flat-button" @click="toggleSensorCalibrateForm">По датчику</button>
                    </div>
                </div>
                <div v-if="isCalibrateByDevice" class="menu">
                    <div class="calibrate-container">
                        <div class="select-container">
                            <span class="select-label">Выберите прибор</span>
                            <select v-model="selectedDevice" class="custom-select">
                                <option selected v-for="(sensors, device) in deviceSensors" :value="device">
                                    {{ device }}
                                </option>
                            </select>
                        </div>
                        <div v-if="selectedDevice" class="select-container">
                            <span class="select-label">Выберите датчик</span>
                            <select v-model="selectedSensor" class="custom-select">
                                <option selected v-for="sensor in deviceSensors[selectedDevice]" :value="sensor">
                                    {{ sensor }}
                                </option>
                            </select>
                        </div>
                        <div v-if="selectedSensor" class="select-container">
                            <span class="select-label">Укажите степень полинома</span>
                            <select v-model="selectedDegree" class="custom-select">
                                <option v-for="n in 6" :value="n">{{ n }}</option>
                            </select>
                        </div>
                    </div>
                    <form v-if="selectedDegree" class="polynomial-form" @submit.prevent="addCalibrationItem">
                        <div class="polynomial-container">
                            <div class="number-field" v-for="n in selectedDegree + 1" :key="n">
                                <input class="number-input" type="number" step="0.0001" v-model.number="coefficients[n-1]" :placeholder="`a${n - 1}`" required>
                            </div>
                        </div>
                        <div class="button-container">
                            <button type="submit" class="flat-button" @click="download">Добавить</button>
                        </div>
                    </form>
                </div>
                <div v-if="isCalibrateBySensor" class="menu">
                    
                </div>
                <Error :message="error" />
            </main>
        </div>
        <div v-if="isPopupVisible" class="popup">
            <div class="popup-content">
                <p class="popup-text">Вы действительно хотите удалить прибор {{ infoNameSerialForDelete }}?</p>
                <div class="popup-buttons">
                    <button class="flat-button" @click="deleteDeviceInfo(infoIdForDelete)">Да</button>
                    <button class="flat-button" @click="isPopupVisible = false">Отмена</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>  
    import { defineComponent } from 'vue';
    import { mapGetters } from "vuex";

    export default defineComponent({
        components: {
        },
        directives: {
        },
        filters: {
        },
        props: {
        },
        data() {
            return {
                isCalibrateByDevice: false,
                isCalibrateBySensor: false,
                isAddingForm: false,
                deviceSensors: null,
                selectedDevice: null,
                selectedSensor: null,
                selectedDegree: null,
                tableSelectedDevice: null,
                coefficients: [],
                calibrationItems: null,
                error: ''
            }
        },
        computed: {
            ...mapGetters(["getToken"])
        },
        watch: {
            selectedDevice() {
                this.selectedSensor = null;
                this.selectedDegree = null;
            },
            selectedDegree() {
                this.coefficients = new Array(this.selectedDegree + 1);
            }
        },
        beforeCreate() {
        },
        created() {
            this.getCalibrationItems();
            this.getDeviceSensors();
        },
        beforeMount() {
        },
        mounted() {
        },
        updated() {
        },
        activated() {
        },
        deactivated() {
        },
        beforeDestroy() {
        },
        destroyed() {
        },
        methods: {
            toggleAddingForm() {
                this.isAddingForm = !this.isAddingForm;
                this.error = '';
            },
            toggleDeviceCalibrateForm() {
                this.isCalibrateByDevice = !this.isCalibrateByDevice;
                this.error = '';
            },
            toggleSensorCalibrateForm() {
                this.isCalibrateBySensor = !this.isCalibrateBySensor;
                this.error = '';
            },
            async getDeviceSensors() {
                try {
                    const response = await fetch('api/calibration/fields', {
                        headers: {
                            'Authorization': 'Bearer ' + this.getToken
                        }
                    });

                    if (response.ok) {
                        this.deviceSensors = await response.json();
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
            async getCalibrationItems() {
                try {
                    const response = await fetch('api/calibration', {
                        headers: {
                            'Authorization': 'Bearer ' + this.getToken
                        }
                    });

                    if (response.ok) {
                        this.calibrationItems = await response.json();
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
            async addCalibrationItem() {
                this.error = '';

                try {
                    const response = await fetch('api/calibration', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + this.getToken
                        },
                        body: JSON.stringify({
                            nameSerial: this.selectedDevice,
                            sensor: this.selectedSensor,
                            coefficients: this.coefficients
                        }),
                    });

                    if (response.ok) {
                        this.nameSerial = '';
                        this.sensor = '';
                        this.coefficients = new Array(this.selectedDegree + 1);
                        this.error = '';
                        this.getCalibrationItems();
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
        min-width: 650px;
        box-shadow: 0px 0px 24px #246a73;
        border-radius: 30px;
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

    .menu {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        gap: 20px;
        min-width: 250px;
        max-width: 700px;
        margin: 20px;
    }

    .calibrate-container {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 30px;
    }

    .select-container {
        display: flex;
        flex-direction: column;
        gap: 5px;
        width: 220px;
    }

    .select-label {
        color: #339989;
        font-weight: bold;
    }

    .polynomial-container {
        display: flex;
        align-items: center;
        justify-content: center;
        flex-wrap: wrap;
        gap: 10px;
    }

    .custom-select {
        width: 100%;
        padding: 10px;
        font-size: 16px;
        border: 2px solid #ccc;
        border-radius: 10px;
        background-color: transparent;
    }

    .custom-select option {
        padding: 10px;
    }

    .custom-select:active,
    .custom-select:hover,
    .custom-select:focus {
        outline: none;
        cursor: pointer;
    }

    .custom-select:active,
    .custom-select:hover {
        border-color: #339989;
    }

    .number-field {
        width: 120px;
    }

    .number-input {
        border: none;
        border-bottom: 2px solid #D1D1D4;
        background: none;
        padding: 10px;
        padding-left: 24px;
        font-weight: 700;
        width: 100%;
        transition: .2s;
        text-align: center;
    }

    .number-input:active,
    .number-input:focus,
    .number-input:hover {
        outline: none;
        border-bottom-color: #339989;
    }

    .polynomial-form {
        display: flex;
        align-items: center;
        flex-direction: column;
        gap: 20px;
    }

    .button-container {
        width: 250px;
    }

    .open-adding-form {
        background: #339989;
        font-size: 14px;
        padding: 10px;
        border-radius: 15px;
        border: none;
        text-transform: uppercase;
        font-weight: 700;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 30%;
        color: #fff;
        cursor: pointer;
        transition: .2s;
    }

    .open-adding-form:active,
    .open-adding-form:hover {
        background-color: #29bca4;
        transform: scale(1.02);
    }

    main {
        display: flex;
        align-items: center;
        flex-direction: column;
        padding: 20px;
        padding-top: 10px;
    }

    .back {
        border: none;
        background: none;
    }

    .back:active,
    .back:hover {
        fill: #29bca4;
        transform: scale(1.05);
        cursor: pointer;
    }

    .back:active svg,
    .back:hover svg {
        stroke: #29bca4;
    }

    .flat-button {
        background: #339989;
        font-size: 14px;
        padding: 16px 20px;
        border-radius: 15px;
        border: none;
        text-transform: uppercase;
        font-weight: 700;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        color: #fff;
        cursor: pointer;
        transition: .2s;
    }

    .flat-button:active,
    .flat-button:hover {
        background-color: #29bca4;
        transform: scale(1.02);
    }

    .table-container {
        display: flex;
        gap: 10px;
    }

    thead {
        display: block;
    }

    tbody {
        display: block;
        max-height: 70vh;
        overflow: auto;
    }

    th, td {
        padding-left: .5rem;
        padding-right: .5rem;
    }

    th {
        font-weight: bold;
    }

    td {
        color: #495057;
        text-align: center;
        height: 36px;
    }

    tr:nth-child(even) {
        background: #ecf8f6;
    }

    .sensor-field {
        width: 160px; 
        word-break: break-all;
    }

    .date-field {
        width: 180px;
        word-break: break-all;
    }

    .coef-field {
        width: 180px;
        word-break: break-all;
    }

    .mini-button-container {
        padding: 8px;
    }

    .mini-button {
        display: flex;
        border: none;
        background: none;
    }

    .mini-button:active svg,
    .mini-button:hover svg {
        fill: #29bca4;
        transform: scale(1.05);
        cursor: pointer;
    }
</style>