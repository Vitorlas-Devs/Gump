import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useTranslationStore } from '@/stores/translationStore'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'
import SvgIcon from '@/components/SvgIcon.vue'

import router from './router'
import 'virtual:windi.css'
import 'virtual:fonts.css'
import './assets/main.css'
import CustomScrollbar from 'custom-vue-scrollbar'
import 'vue-select/dist/vue-select.css'
import vSelect from 'vue-select'
import 'custom-vue-scrollbar/dist/style.css'
import App from './App.vue'

const app = createApp(App)

const pinia = createPinia()
pinia.use(piniaPluginPersistedstate)
app.use(pinia)

useTranslationStore().loadInitialTranslations()

app.use(router)

app.component(CustomScrollbar.name, CustomScrollbar)
app.component('VSelect', vSelect)
app.component('SvgIcon', SvgIcon)

app.mount('#app')
