import { createApp } from 'vue'
import { createPinia } from 'pinia'
// import { i18n } from './locales'

import App from './App.vue'
import router from './router'
import 'virtual:windi.css'
import './assets/main.css'

import { createI18n } from 'vue-i18n'
import messages from "@intlify/unplugin-vue-i18n/messages";

const i18n = createI18n({
  legacy: false,
  globalInjection: true,
  locale: "en_US",
  fallbackLocale: "en_US",
  availableLocales: ["en_US", "hu_HU", "ko_KR"],
  messages: messages,
});

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.use(i18n)

app.mount('#app')
