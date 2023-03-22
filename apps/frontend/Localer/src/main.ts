import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useTranslationStore } from '@/stores/translationStore'
import { library } from '@fortawesome/fontawesome-svg-core'
import { fas } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import router from './router'
import 'virtual:windi.css'
import 'virtual:fonts.css'
import './assets/main.css'
import CustomScrollbar from 'custom-vue-scrollbar'
import 'custom-vue-scrollbar/dist/style.css'
import App from './App.vue'

const app = createApp(App)

app.use(createPinia())

useTranslationStore().loadInitialTranslations()

app.use(router)

app.component(CustomScrollbar.name, CustomScrollbar)

library.add(fas)
app.component('FontAwesomeIcon', FontAwesomeIcon)

app.mount('#app')
