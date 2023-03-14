import { createI18n } from 'vue-i18n'

const i18n = createI18n({
  locale: 'en_US', // set locale
  fallbackLocale: 'en_US', // set fallback locale
  messages: {} // set empty messages
})

// load locale messages with dynamic import
function loadLocaleMessages() {
  const locales: Record<string, () => Promise<unknown>> = import.meta.glob(
    '../../../../locales/*.json'
  )

  const messages: Record<string, () => Promise<unknown>> = {}
  for (const path in locales) {
    const locale = path.match(/[a-z]{2}_[A-Z]{2}/)?.[0]
    if (locale) {
      messages[locale] = locales[path]
    }
  }

  return messages
}

// set locale and locale message
async function setI18nLanguage(locale: string) {
  if (i18n.mode === 'legacy') {
    i18n.global.locale = locale
  } else {
    i18n.global.setLocaleMessage(locale, loadLocaleMessages()[locale])
    i18n.global.locale = locale
  }
}

// switch locale
async function switchLocale(locale: string) {
  const currentLocale = i18n.global.locale
  if (currentLocale === locale) {
    return currentLocale
  }

  // load locale messages if not already loaded
  const locales = Object.keys(loadLocaleMessages())
  if (!locales.includes(locale)) {
    await import(`./../../../locales/${locale}.json`).then((msgs) => {
      i18n.global.setLocaleMessage(locale, msgs.default)
    })
  } else {
    await setI18nLanguage(locale)
  }

  return setI18nLanguage(locale)
}

export { i18n, switchLocale }