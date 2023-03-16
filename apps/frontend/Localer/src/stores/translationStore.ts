import { defineStore } from 'pinia'

// en_US.json
// {
// 	"hoarderBadgeName": "Recipe Hoarder",
// 	"hoarderBadgeDescription": "Save at least 100 recipes.",
// 	"HomeButton": "Home",
// 	"Welcome": "Welcome, {username}!"
// }

export const useTranslationStore = defineStore({
  id: 'translation',
  state: () => ({
    locales: [] as string[],
    keys: [] as string[],
    translations: {} as Record<string, Record<string, string>>
  }),
  getters: {
    translationsForKey: (state) => (key: string) =>
      state.locales.reduce((acc, locale) => {
        acc[locale] = state.translations[locale]?.[key] ?? null

        return acc
      }, {} as Record<string, string>)
  },
  actions: {
    async loadTranslations() {
      const translationModules = import.meta.glob('../../../../../locales/*.json')
      const translations = await Promise.all(
        Object.entries(translationModules).map(async ([path, loader]) => {
          const locale = path.match(/\/(\w+)\.json$/)?.[1] ?? 'en_US'
          const module = (await loader()) as { default: Record<string, string> }

          return [locale, module.default] as const
        })
      )
      this.translations = Object.fromEntries(translations)
      this.locales = Object.keys(this.translations)
      this.keys = Object.keys(this.translations[this.locales[0]] ?? {})
    },
    checkKey(key: string) {
      return this.translationsForKey(key)[this.locales[0]] !== null
    }
  }
})
