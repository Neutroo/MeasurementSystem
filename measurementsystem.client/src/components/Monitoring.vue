<script setup>
    import CrossIcon from './icons/IconCross.vue'
    import BackIcon from './icons/IconBack.vue'
    import AddUserIcon from './icons/IconAddUser.vue'
    import Error from './Error.vue'
</script>

<template>
    <div class="container">
        <div class="screen">      
            <div>
                <div id="messages">
                    <p v-for="msg in messages">{{ msg }}</p>
                </div>
                <input id="chatbox" v-model="newMessage" @keyup.enter="sendMessage">
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
                messages: [],
                newMessage: '',
                socket: null,
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
        },
        beforeMount() {
        },
        mounted() {
            this.socket = new WebSocket('api/monitoring');
            this.socket.onmessage = (event) => {
                this.messages.push(event.data);
            };
            this.socket.onopen = (event) => {
                console.log('hello');
            };
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
            sendMessage() {
                this.socket.send(this.newMessage);
                this.newMessage = "";
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
</style>