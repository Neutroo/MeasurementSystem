<script setup>
    import LoginIcon from './icons/IconLogin.vue'
    import DataIcon from './icons/IconData.vue'
    import MonitoringIcon from './icons/IconMonitoring.vue'
    import RegisteredDevicesIcon from './icons/IconRegisteredDevices.vue'
    import UsersIcon from './icons/IconUsers.vue'
    import CalibrationIcon from './icons/IconCalibration.vue'
    import UserIcon from './icons/IconUser.vue'
</script>

<template>
    <div v-if="currentRoute != '/login'" class="sidebar">
        <nav>
            <router-link v-if="!isLoggedIn" class="router-field" to="/login">
                <LoginIcon />
                <p class="router-text">Авторизоваться</p>
            </router-link>
            <router-link class="router-field" to="/device-data">
                <DataIcon class="data-icon" />
                <p class="router-text">Данные по приборам</p>
            </router-link>
            <router-link class="router-field" to="/monitoring">
                <MonitoringIcon />
                <p class="router-text">Мониторинг</p>
            </router-link>
            <router-link v-if="isLoggedIn" class="router-field" to="/device-registration">
                <RegisteredDevicesIcon />
                <p class="router-text">Регистрация устройств</p>
            </router-link>
            <router-link v-if="isLoggedIn" class="router-field" to="/users">
                <UsersIcon />
                <p class="router-text">Пользователи</p>
            </router-link>
            <router-link v-if="isLoggedIn" class="router-field" to="/calibration">
                <CalibrationIcon />
                <p class="router-text">Калибровка</p>
            </router-link>
        </nav>
        <div v-if="isLoggedIn" class="user-field">
            <UserIcon class="user-icon" />
            <span class="username">{{ getUsername }}</span>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';
    import { mapGetters } from "vuex";

    export default defineComponent({
        computed: {
            ...mapGetters(["isLoggedIn", "getUsername"]),
            currentRoute() {
                return this.$route.path;
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

    .sidebar {
        width: 150px;
        background-color: #fffafb;
        box-shadow: 0px 0px 24px #246a73;
        padding: 20px;
        position: sticky;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    nav {
        flex: 1;
        display: flex;
        flex-direction: column;
        gap: 15px;
    }

    .user-field {
        width: 100%;
        display: flex;
        flex-direction: column;
    }

    .user-icon {
        align-self: center;
    }

    .username {
        font-size: 12px;
        font-weight: bold;
        overflow-wrap: break-word;
        color: #339989;
        text-align: center;
    }

    .router-field {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 10px 0px;
        text-decoration: none;
        color: #339989;
    }

    .router-field:active,
    .router-field:hover {
        transform: scale(1.02);
    }

    .router-text {
        border: none;
        border-bottom: 2px solid #D1D1D4;
        background: none;
        padding: 8px;
        font-weight: 700;
        font-size: 12px;
        width: 100%;
        transition: .2s;
        text-align: center;
        width: 130px;
    }
</style>