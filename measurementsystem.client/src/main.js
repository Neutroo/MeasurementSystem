/*import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

createApp(App).mount('#app')*/

import { createApp } from 'vue'
import router from './router'
import App from './App.vue'

createApp(App)
    .use(router)
    .mount('#app')