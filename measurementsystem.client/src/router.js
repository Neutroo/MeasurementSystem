import { createMemoryHistory, createRouter } from 'vue-router'

import Login from './components/Login.vue'
import DeviceData from './components/DeviceData.vue'

const routes = [
    {
        path: '/login',
        component: Login
    },
    {
        path: '/device-data',
        component: DeviceData
    }
]

const router = createRouter({
    history: createMemoryHistory(),
    routes
})

export default router