import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useTranslationStore } from '@/stores/translationStore'
import { icon, library } from '@fortawesome/fontawesome-svg-core'
// import { fas } from '@fortawesome/free-solid-svg-icons'
// import { far } from '@fortawesome/free-regular-svg-icons'
// import { fab } from '@fortawesome/free-brands-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

const x = 'faRotate'
let definition

// see: https://stackoverflow.com/questions/75281809/lazy-loading-fontawesome-icons-in-vue3-vite-not-working-in-dev
if (import.meta.env.PROD) {
  const iconModule = await import(`../node_modules/@fortawesome/free-solid-svg-icons/${x}.js`)
  definition = iconModule.definition
} else {
  const iconModule = await import(`@fortawesome/free-solid-svg-icons`)
  definition = iconModule[x as keyof typeof iconModule]
}
library.add(definition)
// Property 'value' does not exist on type '(icon: IconLookup | IconName, params?: IconParams | undefined) => Icon'.ts(2339)
// icon.value = 'fa-solid fa-rotate'

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

// library.add(fas, far, fab)
app.component('FontAwesomeIcon', FontAwesomeIcon)

app.mount('#app')
