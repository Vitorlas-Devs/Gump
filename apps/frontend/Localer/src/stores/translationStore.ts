import { getContent } from '@/octokit'
import { Base64 } from 'js-base64'
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
    translationsForLocale: (state) => (locale: string) =>
      state.keys.reduce((acc, key) => {
        acc[key] = state.translations[locale]?.[key] ?? null

        return acc
      }, {} as Record<string, string>)
  },
  actions: {
    async loadInitialTranslations() {
      const translationModules = import.meta.glob('../../../../../locales/*.json')
      const translations = await Promise.all(
        Object.entries(translationModules).map(async ([path, loader]) => {
          const locale = path.match(/\/(\w+)\.json$/)?.[1] ?? 'en_US'
          const module = (await loader()) as { default: Record<string, string> }

          return [locale, module.default] as const
        })
      )
      this.initialTranslations = Object.fromEntries(translations)
      this.translations = JSON.parse(JSON.stringify(this.initialTranslations))
      this.locales = Object.keys(this.translations)
      this.keys = Object.keys(this.translations[this.locales[0]])
    },
    loadTranslations() {
      let username = import.meta.env.VITE_USERNAME
      username = username.replace(/ /g, '-')
      this.locales.forEach(async (locale) => {
        const contentRequest = await getContent(username, locale)
        if (contentRequest.response) {
          this.translations[locale] = JSON.parse(
            Base64.decode(contentRequest.response.data.content)
          )
          this.initialTranslations[locale] = JSON.parse(
            Base64.decode(contentRequest.response.data.content)
          )
        }
      })
    },
    checkKey(key: string) {
      return this.keys.includes(key)
    },
    checkDirty() {
      const dirty = this.locales.some((locale) => {
        const keys = Object.keys(this.translations[locale])
        return keys.some(
          (key) => this.translations[locale][key] !== this.initialTranslations[locale][key]
        )
      })
      this.dirty = dirty
    },
    saveChanges() {
      this.locales.forEach((locale) => {
        const keys = Object.keys(this.translations[locale])
        keys.forEach((key) => {
          this.translations[locale][key] = this.initialTranslations[locale][key]
        })
      })
      this.dirty = false
    }
  }
})
