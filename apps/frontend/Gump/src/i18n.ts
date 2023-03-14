import { createI18n } from 'vue-i18n'
import messages from '@intlify/unplugin-vue-i18n/messages'

// List of all locales.
export const allLocales = ['en_US', 'hu_HU', 'ko_KR']

// Create Vue I18n instance.
export const i18n = createI18n({
  legacy: false,
  globalInjection: true,
  locale: 'en_US',
  fallbackLocale: 'en_US',
  messages: messages
})

// Set new locale.
export async function setLocalee(locale: string) {
  // Load locale if not available yet.
  if (!i18n.global.availableLocales.includes(locale)) {
    const messages = await loadLocale(locale)

    // fetch() error occurred.
    if (messages === undefined) {
      return
    }

    // Add locale.
    i18n.global.setLocaleMessage(locale, messages)
  }

  // Set locale.
  i18n.global.locale.value = locale
}

// Fetch locale.
async function loadLocale(locale: string) {
  return fetch(`./../../../../locales/${locale}.json`)
    .then((response) => {
      if (response.ok) {
        return response.json()
      }
      throw new Error('Network response was not ok.')
    })
    .catch((error) => {
      console.error(error)
    })
}
