import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'

const app = createApp(App)

app.use(router)
app.use(vuetify)

// Inicializar autenticação após o mount da aplicação
app.mount('#app')
