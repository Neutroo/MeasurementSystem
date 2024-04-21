<script setup>
    import LoginIcon from './icons/IconLogin.vue'
    import DataIcon from './icons/IconData.vue'
    import RegisteredDevicesIcon from './icons/IconRegisteredDevices.vue'
    import UsersIcon from './icons/IconUsers.vue'
</script>

<template>
    <div class="container">
        <div class="sidebar">
            <nav>
                <router-link class="router-field" to="/login">
                    <LoginIcon class="router-icon" />
                    <p class="router-text">Авторизоваться</p>
                </router-link>
                <router-link class="router-field" to="/device-data">
                    <DataIcon class="router-icon" />
                    <p class="router-text">Данные по приборам</p>
                </router-link>
                <router-link class="router-field" to="/">
                    <RegisteredDevicesIcon class="router-icon" />
                    <p class="router-text">Регистрация устройств</p>
                </router-link>
                <router-link class="router-field" to="/">
                    <UsersIcon class="router-icon" />
                    <p class="router-text">Пользователи</p>
                </router-link>
            </nav>
        </div>

        <div class="content">
            <main>
                <router-view />
            </main>
            <h1>Main Content</h1>
            <div><p>content</p></div>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';

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
                username: '',
                password: ''
            }
        },
        computed: {
            currentRoute() {
                return this.$route.path;
            }
        },
        watch: {
        },
        beforeCreate() {
        },
        created() {
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
            goToMenu() {
                this.$router.push('/menu')
            },
            login() {
                // Implement your login logic here
            }
        },
    });
</script>

<style scoped>
    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
        font-family: Raleway, sans-serif;
    }

    body {
        background: linear-gradient(90deg, #C7C5F4, #776BCC);
    }

    .container {
        display: flex;
        min-height: 100vh;
    }

    .sidebar {
        width: 150px;
        background-color: #fff;
        box-shadow: 0px 0px 24px #f25c54;
        padding: 20px;
        position: fixed;
        top: 0;
        left: 0;
        height: 100vh;
    }

    .content {
        flex: 1;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 20px;
        margin-left: 200px;
    }

    .screen {
        background-color: #fff;
        position: relative;
        height: 500px;
        width: 360px;
        box-shadow: 0px 0px 24px #f25c54;
        border-radius: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    nav {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }

    .router-field {
        /*background: #f25c54;
        font-size: 14px;
        margin: 15px 61px;
        padding: 16px 20px;
        border-radius: 30px;
        border: none;
        text-transform: uppercase;
        font-weight: 700;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 75%;
        color: #fff;
        cursor: pointer;
        transition: .2s;
        text-decoration: none;*/
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 20px 0px;
        text-decoration: none;
        color: #f25c54;
    }

    .router-field:active,
    .router-field:focus,
    .router-field:hover {
        transform: scale(1.02);
    }

    .router-icon {
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

    .login {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 30px;
        margin-top: 50px;
    }

    .login__field {
        padding: 20px 0px;
        position: relative;
    }

    .login__icon {
        position: absolute;
        top: 30px;
        color: #7875B5;
    }

    .login__input {
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

    .login__input:active,
    .login__input:focus,
    .login__input:hover {
        outline: none;
        border-bottom-color: #f25c54;
    }

    .login__submit {
        background: #f25c54;
        font-size: 14px;
        margin-top: 30px;
        padding: 16px 20px;
        border-radius: 30px;
        border: none;
        text-transform: uppercase;
        font-weight: 700;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 75%;
        color: #fff;
        cursor: pointer;
        transition: .2s;
    }

    .login__submit:active,
    .login__submit:focus,
    .login__submit:hover {
        background-color: #f4845f;
        transform: scale(1.02);
    }

    .social-login {
        position: absolute;
        height: 40px;
        width: 160px;
        text-align: center;
        bottom: 0px;
        right: 0px;
        color: #f25c54;
    }
</style>