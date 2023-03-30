import { getContent } from '@/octokit'
import { Base64 } from 'js-base64'
import { defineStore } from 'pinia'
import { useUserStore } from './userStore'

export const useTranslationStore = defineStore({
  id: 'translation',
  state: () => ({
    locales: [] as string[],
    initialLocales: [] as string[],
    keys: [] as string[],
    initialKeys: [] as string[],
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
      this.initialLocales = Object.keys(this.initialTranslations)
      this.locales = JSON.parse(JSON.stringify(this.initialLocales))
      this.initialKeys = Object.keys(this.translations[this.locales[0]])
      this.keys = JSON.parse(JSON.stringify(this.initialKeys))
    },
    async loadTranslations() {
      const user = useUserStore()
      const username = user.username.replace(/ /g, '-')
      this.locales.forEach(async (locale) => {
        const contentRequest = await getContent(username, locale)
        if (contentRequest.response) {
          this.translations[locale] = JSON.parse(Base64.decode(contentRequest.response.content))
          this.initialTranslations[locale] = JSON.parse(
            Base64.decode(contentRequest.response.content)
          )
        }
      })
    },
    checkKey(key: string) {
      return this.keys.includes(key)
    },
    addKey(key: string) {
      this.keys.push(key)
      this.locales.forEach((locale) => {
        this.translations[locale][key] = ''
      })
    },
    addLanguage(locale: string) {
      this.locales.push(locale)
      this.translations[locale] = {}
      this.keys.forEach((key) => {
        this.translations[locale][key] = ''
      })
    },
    checkDirty() {
      const dirty = this.locales.some((locale) => {
        const keys = Object.keys(this.translations[locale])
        if (keys.every((key) => this.translations[locale][key] === '')) {
          return true
        }
        if (!this.initialTranslations[locale]) {
          return true
        }
        return keys.some(
          (key) => this.translations[locale][key] !== this.initialTranslations[locale][key]
        )
      })
      if (this.keys.length !== Object.keys(this.initialTranslations[this.locales[0]]).length) {
        this.dirty = true
        return
      }
      this.dirty = dirty
    },
    checkDirtyKey(key: string): boolean {
      const dirty = this.locales.some((locale) => {
        if (!this.initialTranslations[locale]) {
          return true
        }
        return this.translations[locale][key] !== this.initialTranslations[locale][key]
      })
      this.dirty = dirty
      return dirty
    },
    resetChanges() {
      this.locales.forEach((locale) => {
        const keys = Object.keys(this.translations[locale])
        if (keys.every((key) => this.translations[locale][key] === '')) {
          this.translations[locale] = {}
          return
        }
        keys.forEach((key) => {
          if (!this.initialTranslations[locale]) {
            this.translations[locale][key] = ''
            return
          }
          this.translations[locale][key] = this.initialTranslations[locale][key]
        })
      })
      this.keys = this.keys.filter((key) => {
        return this.locales.some((locale) => {
          return this.initialTranslations[locale][key] !== ''
        })
      })
      this.keys = JSON.parse(JSON.stringify(this.initialKeys))
      const user = useUserStore()
      this.locales = user.languages = JSON.parse(JSON.stringify(this.initialLocales))
      this.dirty = false
    },
    saveChanges() {
      this.initialTranslations = JSON.parse(JSON.stringify(this.translations))
      this.initialKeys = JSON.parse(JSON.stringify(this.keys))
      this.initialLocales = JSON.parse(JSON.stringify(this.locales))
      this.dirty = false
    }
  }
})
