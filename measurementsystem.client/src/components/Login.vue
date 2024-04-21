<script setup>
    import UserIcon from './icons/IconUser.vue'
    import LockIcon from './icons/IconLock.vue'
</script>

<template>
    <div class="container">
        <div class="screen">
            <form class="login" @submit.prevent="login">
                <div class="login-field">
                    <UserIcon class="login-icon" />
                    <input type="text" class="login-input" v-model="username" placeholder="Username" required>
                </div>
                <div class="login-field">
                    <LockIcon class="login-icon" />
                    <input type="password" class="login-input" v-model="password" placeholder="Password" required>
                </div>
                <button type="submit" class="login-submit">
                    <span class="button-text">Войти</span>
                </button>
            </form>
            <a class="skip" @click="goToHome">Пропустить</a>
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
            goToHome() {
                this.$router.push('/')
            },
            login() {
                console.log(this.username);
                console.log(this.password);
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

    .container {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 100vh;
    }

    .screen {
        background-color: #fffafb;
        position: relative;
        height: 500px;
        width: 360px;
        box-shadow: 0px 0px 24px #246a73;
        border-radius: 30px;
    }

    .login {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 30px;
        margin-top: 50px;
    }

    .login-field {
        padding: 20px 0px;
        position: relative;
    }

    .login-icon {
        position: absolute;
        top: 26px;
    }

    .login-input {
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

    .login-input:active,
    .login-input:focus,
    .login-input:hover {
        outline: none;
        border-bottom-color: #339989;
    }

    .login-submit {
        background: #339989;
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

    .login-submit:active,
    .login-submit:focus,
    .login-submit:hover {
        background-color: #29bca4;
        transform: scale(1.02);
    }

    .skip {
        position: absolute;
        bottom: 15px;
        right: 25px;
        color: #339989;
        cursor: pointer;
    }

    .skip:hover {
        color: #29bca4;
        transform: scale(1.01);
    }
</style>