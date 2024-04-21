<script setup>
    import Sidebar from './components/Sidebar.vue'
</script>

<template>
    <div class="container">
        <Sidebar />
        <div class="content">
            <main>
                <router-view />
            </main>
        </div>
    </div>
</template>

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

    .container {
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
    
    @media (min-width: 1024px) {
        header {
            display: flex;
            place-items: center;
            padding-right: calc(var(--section-gap) / 2);
        }
    
        .logo {
          margin: 0 2rem 0 0;
        }
    
        header .wrapper {
          display: flex;
          place-items: flex-start;
          flex-wrap: wrap;
        }
    }
</style>
