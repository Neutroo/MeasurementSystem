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
                <div v-if="!isAddingInfo && !isEditInfo" class="title-container">
                    <span class="title">Зарегистрированные приборы</span>
                    <button class="open-info-form" @click="toggleInfoForm">Добавить</button>
                </div>
                <div v-if="isAddingInfo" class="title-container">
                    <span class="title">Новый прибор</span>
                    <button class="back" @click="toggleInfoForm">
                        <BackIcon class="back-icon" />
                    </button>
                </div>
                <div v-if="isEditInfo" class="title-container">
                    <span class="title">Редактирование</span>
                    <button class="back" @click="toggleEditForm">
                        <BackIcon class="back-icon" />
                    </button>
                </div>
            </header>
            <main>
                <div v-if="!isAddingInfo && !isEditInfo">
                    <table v-if="deviceInfos">
                        <thead>
                            <tr>
                                <th class="name-field">Название</th>
                                <th class="serial-field">Серийный номер</th>
                                <th class="key-field">Ключ</th>
                                <th class="coord-field">X</th>
                                <th class="coord-field">Y</th>
                                <th class="location-field">Расположение</th>
                                <th class="isdeleted-field">Удален</th>
                                <th style="width: 36px"></th>
                                <th style="width: 36px"></th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="info in deviceInfos" :key="info">
                                <td class="name-field">{{ info.name }}</td>
                                <td class="serial-field">{{ info.serial }}</td>
                                <td class="key-field">{{ info.authKey }}</td>
                                <td class="coord-field">{{ info.x }}</td>
                                <td class="coord-field">{{ info.y }}</td>
                                <td class="location-field">{{ info.location }}</td>
                                <td class="isdeleted-field">{{ isDeletedConvertToWord(info.isDeleted) }}</td>
                                <td class="mini-button-container">
                                    <button class="mini-button" @click="toggleEditForm(info)">
                                        <EditIcon />
                                    </button>
                                </td>
                                <td class="mini-button-container">
                                    <button class="mini-button" @click="showPopup(info.id, info.name + ' - ' + info.serial)">
                                        <CrossIcon />
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div v-if="isAddingInfo">
                    <form class="info-form" @submit.prevent="addDeviceInfo">
                        <div class="info-container">
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="POSTdeviceinfo.name" placeholder="Название" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="POSTdeviceinfo.serial" placeholder="Номер" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="POSTdeviceinfo.authKey" placeholder="Ключ" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="POSTdeviceinfo.x" placeholder="x" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="POSTdeviceinfo.y" placeholder="y" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="POSTdeviceinfo.location" placeholder="Расположение" required>
                            </div>
                        </div>
                        <div class="button-container">
                            <button type="submit" class="flat-button">Зарегистрировать</button>
                        </div>
                    </form>
                </div>
                <div v-if="isEditInfo">
                    <form class="info-form" @submit.prevent="putDeviceInfo(infoIdForEdit)">
                        <div class="info-container">
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="PUTdeviceinfo.name" placeholder="Название" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="PUTdeviceinfo.serial" placeholder="Номер" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="PUTdeviceinfo.x" placeholder="x" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="PUTdeviceinfo.y" placeholder="y" required>
                            </div>
                            <div class="deviceinfo-field">
                                <input type="text" class="deviceinfo-input" v-model="PUTdeviceinfo.location" placeholder="Расположение" required>
                            </div>
                        </div>
                        <div class="button-container">
                            <button type="submit" class="flat-button">Сохранить</button>
                        </div>
                    </form>
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
                isAddingInfo: false,
                isEditInfo: false,
                POSTdeviceinfo: {
                    name: '',
                    serial: '',
                    authKey: '',
                    x: '',
                    y: '',
                    location: '',
                    isDeleted: false
                },
                PUTdeviceinfo: null,
                infoIdForEdit: null,
                deviceInfos: [],
                isPopupVisible: false,
                infoIdForDelete: null,
                infoNameSerialForDelete: '',
                error: ''
            }
        },
        computed: {
            ...mapGetters(["getToken"])
        },
        watch: {
        },
        beforeCreate() {
        },
        created() {
            this.getDeviceInfos();
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
            toggleInfoForm() {
                this.isAddingInfo = !this.isAddingInfo;
                this.error = '';
            },
            toggleEditForm(info) {
                this.isEditInfo = !this.isEditInfo;
                this.PUTdeviceinfo = JSON.parse(JSON.stringify(info));;
                this.infoIdForEdit = this.PUTdeviceinfo.id;
                this.error = '';
            },
            async getDeviceInfos() {
                try {
                    const response = await fetch('api/deviceinfo', {
                        headers: {
                            'Authorization': 'Bearer ' + this.getToken
                        }
                    });

                    if (response.ok) {
                        this.deviceInfos = await response.json();
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
            async addDeviceInfo() {
                this.error = '';

                if (!this.isValid(this.POSTdeviceinfo)) {
                    return;
                }

                try {
                    const response = await fetch('api/deviceinfo', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + this.getToken
                        },
                        body: JSON.stringify({
                            name: this.POSTdeviceinfo.name,
                            serial: this.POSTdeviceinfo.serial,
                            authKey: this.POSTdeviceinfo.authKey,
                            x: this.POSTdeviceinfo.x,
                            y: this.POSTdeviceinfo.y,
                            location: this.POSTdeviceinfo.location,
                            isDeleted: false
                        }),
                    });

                    if (response.ok) {
                        this.POSTdeviceinfo.name = '';
                        this.POSTdeviceinfo.serial = '';
                        this.POSTdeviceinfo.authKey = '';
                        this.POSTdeviceinfo.x = '';
                        this.POSTdeviceinfo.y = '';
                        this.POSTdeviceinfo.location = '';
                        this.error = '';
                        this.getDeviceInfos();
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
            async putDeviceInfo(id) {
                this.error = '';

                if (!this.isValid(this.PUTdeviceinfo)) {
                    return;
                }

                try {
                    const response = await fetch(`api/deviceinfo/${id}`, {
                        method: 'PUT',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + this.getToken
                        },
                        body: JSON.stringify({
                            items: {
                                name: this.PUTdeviceinfo.name,
                                serial: this.PUTdeviceinfo.serial,
                                x: this.PUTdeviceinfo.x,
                                y: this.PUTdeviceinfo.y,
                                location: this.PUTdeviceinfo.location
                            }                           
                        }),
                    });
            
                    if (response.ok) {
                        this.error = '';
                        this.getDeviceInfos();
                        this.isEditInfo = false;
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
            async deleteDeviceInfo(id) {
                this.error = '';

                try {
                    const response = await fetch(`api/deviceinfo/${id}`, {
                        method: 'DELETE',
                        headers: {
                            'Authorization': 'Bearer ' + this.getToken
                        }
                    });

                    if (response.ok) {
                        this.userIdForDelete = null;
                        this.getDeviceInfos();
                        this.isPopupVisible = false;
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
            isValid(info) {
                if (info.authKey.length < 3) {
                    this.error = 'Слишком короткий ключ';
                    return false;
                }
                else if (!this.isNumber(parseFloat(info.x))) {
                    this.error = 'x должен быть числовым типом';
                    return false;
                }
                else if (!this.isNumber(parseFloat(info.y))) {
                    this.error = 'y должен быть числовым типом';
                    return false;
                }
                return true;
            },
            isNumber(value) {
                return typeof value === 'number' && !isNaN(value);
            },
            showPopup(id, nameSerial) {
                this.infoNameSerialForDelete = nameSerial;
                this.infoIdForDelete = id;
                this.isPopupVisible = true;
            },
            isDeletedConvertToWord(isDeleted) {
                if (isDeleted) {
                    return 'Да';
                }
                else {
                    return 'Нет'
                }
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
        width: 900px;
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

    .open-info-form {
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

    .open-info-form:active,
    .open-info-form:hover {
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

    .info-form {
        display: flex;
        align-items: center;
        flex-direction: column;
        gap: 20px;
    }

    .info-container {
        display: flex;
        align-items: center;
        gap: 20px;
    }

    .deviceinfo-input {
        border: none;
        border-bottom: 2px solid #D1D1D4;
        background: none;
        padding: 10px;
        font-weight: 700;
        width: 100%;
        transition: .2s;
        text-align: center;
    }

    .deviceinfo-input:active,
    .deviceinfo-input:focus,
    .deviceinfo-input:hover {
        outline: none;
        border-bottom-color: #339989;
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

    .button-container {
        width: 30%;
    }

    .flat-button {
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
    }

    tr:nth-child(even) {
        background: #ecf8f6;
    }

    .name-field {
        width: 120px; 
        word-break: break-all;
    }

    .serial-field {
        width: 154px;
        word-break: break-all;
    }

    .key-field {
        width: 110px;
        word-break: break-all;
    }

    .coord-field {
        width: 50px;
        word-break: break-all;
    }

    .location-field {
        width: 132px;
        word-break: break-all;
    }

    .isdeleted-field {
        width: 73px;
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

    .popup {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .popup-content {
        display: flex;
        flex-direction: column;
        gap: 15px;
        background: #fffafb;
        padding: 20px;
        border-radius: 15px;
        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    }

    .popup-text {
        font-weight: bold;
    }

    .popup-buttons {
        display: flex;
        gap: 10px;
    }
</style>