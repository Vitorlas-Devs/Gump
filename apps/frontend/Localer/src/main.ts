import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useTranslationStore } from '@/stores/translationStore'
import SvgIcon from '@/components/SvgIcon.vue'

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

app.component('SvgIcon', SvgIcon)

app.mount('#app')
