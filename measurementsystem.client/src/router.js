import { createMemoryHistory, createRouter } from 'vue-router'

import Login from './components/Login.vue'
import DeviceData from './components/DeviceData.vue'
import Monitoring from './components/Monitoring.vue'
import DeviceRegistration from './components/DeviceRegistration.vue'
import Users from './components/Users.vue'
import Calibration from './components/Calibration.vue'

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
        path: '/monitoring',
        component: Monitoring
    },
    {
        path: '/device-registration',
        component: DeviceRegistration
    },
    {
        path: '/users',
        component: Users
    },
    {
        path: '/calibration',
        component: Calibration
    }
]

const router = createRouter({
    history: createMemoryHistory(),
    routes
})

export default router