<script setup>
    import CrossIcon from './icons/IconCross.vue'
</script>

<template>
    <div class="container">
        <div class="screen">
            <header>
                <span class="title">Список пользователей</span>
                <button class="add-user" @click="add">Добавить</button>
            </header>
            <main>
                <table>
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Имя пользователя</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="user in users" :key="user">
                            <td>{{ user.id }}</td>
                            <td>{{ user.username }}</td>
                            <td class="cross-icon" >
                                <button class="remove-user" @click="remove(user.id)">
                                    <CrossIcon />
                                </button>
                            </td>
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
                users: [],
                error: ''
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
            add() {
                const user = {
                    id: this.users.length,
                    username: "afafba",
                    password: "bdbdbfd"
                }
                this.users.push(user);
            },
            remove(index) {
                this.users.splice(index, 1);
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

    .add-user {
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

    .add-user:active,
    .add-user:hover {
        background-color: #29bca4;
        transform: scale(1.02);
    }

    main {
        display: flex;
        padding: 20px;
        padding-top: 10px;
    }

    th {
        font-weight: bold;
    }

    tr:nth-child(even) {
        background: #ecf8f6;
    }

    .remove-user {
        border: none;
        background: none;
    }

    .remove-user:active svg,
    .remove-user:hover svg {
        fill: #29bca4;
        transform: scale(1.05);
        cursor: pointer;
    }

    .cross-icon {
        margin-top: 4px;
        display: block;
        padding: 0px 4px;
    }

    td {
        color: #495057;
    }

    th, td {
        padding-left: .5rem;
        padding-right: .5rem;
    }

    table {

    }
</style>