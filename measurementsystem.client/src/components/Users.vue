<script setup>
    import CrossIcon from './icons/IconCross.vue'
    import BackIcon from './icons/IconBack.vue'
    import AddUserIcon from './icons/IconAddUser.vue'
    import Error from './Error.vue'
</script>

<template>
    <div class="container">
        <div class="screen">
            <header>
                <span v-if="!isAddingUser" class="title">Список пользователей</span>
                <span v-else class="title">Новый пользователь</span>
                <button v-if="!isAddingUser" class="open-user-form" @click="toggleUserForm">Добавить</button>
                <button v-else class="back" @click="toggleUserForm">
                    <BackIcon class="back-icon" />
                </button>
            </header>
            <main>
                <div v-if="isAddingUser">
                    <form class="user-form" @submit.prevent="addUser">
                        <div class="user-field">
                            <input type="text" class="user-input" v-model="username" placeholder="Имя пользователя" required>
                        </div>
                        <div class="user-field">
                            <input type="password" class="user-input" v-model="password" placeholder="Пароль" required>
                        </div>
                        <div class="user-field">
                            <input type="password" class="user-input" v-model="confirmedPassword" placeholder="Повторный пароль" required>
                        </div>
                        <button type="submit" class="flat-button">
                            <AddUserIcon />
                        </button>
                    </form>
                </div>
                <div v-else>
                    <table v-if="users">
                        <thead>
                            <tr>
                                <th class="username-field">Имя пользователя</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="user in users" :key="user">
                                <td class="username-field">{{ user.username }}</td>
                                <td class="cross-icon">
                                    <button class="delete-user" @click="showPopup(user.id, user.username)">
                                        <CrossIcon />
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <Error :message="error" />
            </main>
        </div>
        <div v-if="isPopupVisible" class="popup">
            <div class="popup-content">
                <p class="popup-text">Вы действительно хотите удалить пользователя {{ usernameForDelete }}?</p>
                <div class="popup-buttons">
                    <button class="flat-button" @click="deleteUser(userIdForDelete)">Да</button>
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
                isAddingUser: false,
                username: '',
                password: '',
                confirmedPassword: '',
                users: null,
                isPopupVisible: false,
                userIdForDelete: null,
                usernameForDelete: '',
                error: ''
            }
        },
        computed: {
            ...mapGetters(["getToken", "getUsername"])
        },
        watch: {
        },
        beforeCreate() {
        },
        created() {
            this.getUsers();
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
            toggleUserForm() {
                this.isAddingUser = !this.isAddingUser;
                this.error = '';
            },
            async getUsers() {
                try {
                    const response = await fetch('api/user', {
                        headers: {
                            'Authorization': 'Bearer ' + this.getToken
                        }
                    });

                    if (response.ok) {
                        this.users = await response.json();
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
            async addUser() {
                this.error = '';

                if (!this.isValid()) {
                    return;
                }

                try {
                    const response = await fetch('api/user', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + this.getToken
                        },
                        body: JSON.stringify({
                            username: this.username,
                            password: this.password
                        }),
                    });

                    if (response.ok) {
                        this.username = '';
                        this.password = '';
                        this.confirmedPassword = '';
                        this.error = '';
                        this.getUsers();
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
            async deleteUser(id) {
                this.error = '';

                if (this.getUsername === this.usernameForDelete) {
                    this.error = 'Вы не можете удалить пользователя, через которого сейчас авторизованы';
                    this.isPopupVisible = false;
                    return;
                }

                try {
                    const response = await fetch(`api/user/${id}`, {
                        method: 'DELETE',
                        headers: {
                            'Authorization': 'Bearer ' + this.getToken
                        }
                    });

                    if (response.ok) {
                        this.userIdForDelete = null;
                        this.getUsers();
                        this.error = '';
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
            isValid() {
                if (this.username.length < 3) {
                    this.error = 'Слишком короткое имя пользователя';
                    return false;
                }
                else if (this.password.length < 6) {
                    this.error = 'Слишком короткий пароль';
                    return false;
                }
                else if (this.password !== this.confirmedPassword) {
                    this.error = 'Пароли не совпадают';
                    return false;
                }
                return true;
            },
            showPopup(id, username) {
                this.usernameForDelete = username;
                this.userIdForDelete = id;
                this.isPopupVisible = true;
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
        width: 450px;
        box-shadow: 0px 0px 24px #246a73;
        border-radius: 30px;
    }

    header {
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

    .open-user-form {
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

    .open-user-form:active,
    .open-user-form:hover {
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

    .user-form {
        display: flex;
        align-items: center;
        flex-direction: column;
        gap: 20px;
    }

    .user-input {
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

    .user-input:active,
    .user-input:focus,
    .user-input:hover {
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
        max-height: 200px;
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

    .username-field {
        width: 165px; 
        word-break: break-all;
    }

    .cross-icon {
        padding: 8px;
    }

    .delete-user {
        display: flex;
        border: none;
        background: none;
    }

    .delete-user:active svg,
    .delete-user:hover svg {
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