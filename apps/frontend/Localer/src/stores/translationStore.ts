import { defineStore } from 'pinia'

export const useTranslationStore = defineStore({
  id: 'translation',
  state: () => ({
    locales: [] as string[],
    keys: [] as string[],
    translations: {} as Record<string, Record<string, string>>,
    initialTranslations: {} as Record<string, Record<string, string>>,
    dirty: false
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
      this.keys = Object.keys(this.translations[this.locales[0]])
    },
    async loadInitialTranslations() {
      const translationModules = import.meta.glob('../../../../../locales/*.json')
      const translations = await Promise.all(
        Object.entries(translationModules).map(async ([path, loader]) => {
          const locale = path.match(/\/(\w+)\.json$/)?.[1] ?? 'en_US'
          const module = (await loader()) as { default: Record<string, string> }

          return [locale, module.default] as const
        })
      )
      this.translations = Object.fromEntries(translations)
      this.initialTranslations = JSON.parse(JSON.stringify(this.translations))
    },
    checkKey(key: string) {
      return this.translationsForKey(key)[this.locales[0]] !== null
    },
    checkDirty() {
      const dirty = this.locales.some((locale) => {
        const keys = Object.keys(this.translations[locale])
        return keys.some(
          (key) => this.translations[locale][key] !== this.initialTranslations[locale][key]
        )
      })
      this.dirty = dirty
    }
  }
})
