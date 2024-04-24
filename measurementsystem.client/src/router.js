import { createMemoryHistory, createRouter } from 'vue-router'

import Login from './components/Login.vue'
import DeviceData from './components/DeviceData.vue'
import Users from './components/Users.vue'

const routes = [
    {
        path: '/login',
        component: Login
    },
    {
        path: '/device-data',
        component: DeviceData
    },
    {
        path: '/device-registration',
        component: null
    },
    {
        path: '/users',
        component: Users
    }
]

const router = createRouter({
    history: createMemoryHistory(),
    routes
})

export default router