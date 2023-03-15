import { defineStore } from 'pinia'

const translationModules = import.meta.glob('../../../../../locales/*.json')

export type TranslationData = typeof import('../../../../../locales/en_US.json') & {
  [key: string]: string
}

// {
//	"locale": "en_US",
// 	"hoarderBadgeName": "Recipe Hoarder",
// 	"hoarderBadgeDescription": "Save at least 100 recipes.",
// 	"HomeButton": "Home",
// 	"Welcome": "Welcome, {username}!"
// }

type Translations = Record<string, TranslationData>

export const useTranslationStore = defineStore({
  id: 'translation',
  state: (): {
    translations: Translations
    currentLocale: string
  } => ({
    translations: {} as Translations,
    currentLocale: 'en_US'
  }),
  getters: {
    getTranslations: (state): Translations => state.translations || {},
    getCurrentLocale: (state): string => state.currentLocale || 'en_US',
    keys: (getters): string[] => {
      return Object.keys(getters.translations[getters.currentLocale])
    },
    translationsForKey: (getters): ((key: string) => Record<string, string>) => {
      return (key: string): Record<string, string> => {
        const translations = getters.translations
        return Object.keys(translations).reduce((result, locale) => {
          result[locale] = translations[locale][key] || ''
          return result
        }, {} as Record<string, string>)
      }
    },
    locales: (getters): string[] => {
      return Object.keys(getters.translations)
    }
  },
  actions: {
    // Load the translation data
    async loadTranslations(): Promise<void> {
      const files = await Promise.all(Object.values(translationModules).map((module) => module()))
      const translations = files.reduce((result: Translations, data, index) => {
        const path = Object.keys(translationModules)[index]
        const locale = path.match(/locales\/(.+)\.json/)![1]
        result[locale] = data as TranslationData
        return result
      }, {} as Translations)
      this.translations = translations
    },

    // Set the current locale
    setCurrentLocale(locale: string): void {
      this.currentLocale = locale
    }
  }
})
