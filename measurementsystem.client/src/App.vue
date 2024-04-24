<script setup>
    import Sidebar from './components/Sidebar.vue'
</script>

<template>
    <div class="main-container">
        <Sidebar />
        <div class="content">
            <main>
                <router-view />
            </main>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';

    export default defineComponent({
        created() {
            this.$router.push('/login')
        }
    });
</script>

<style>
    header {
        line-height: 1.5;
    }

    body {       
        background: linear-gradient(90deg, #7de2d1, #339989);
        padding: 0;
        margin: 0;
        font-family: Raleway, sans-serif;
    }

    .main-container {
        display: flex;
        height: 100vh;
        max-height: 100vh;
        width: 100%;
    }

    .content {
        flex: 1;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 20px;
    }
</style>
