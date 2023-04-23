export default defineNuxtConfig({
  modules: [
    '@nuxt/devtools',
    '@unocss/nuxt',
    '@pinia/nuxt',
    '@pinia-plugin-persistedstate/nuxt',
    '@nuxtjs/eslint-module',
    '@nuxtjs/i18n',
    '@nuxtjs/ionic',
    'nuxt-vitest',
  ],
  css: [
    '@/assets/main.css',
  ],
  pwa: {
    icon: false,
  },
  eslint: {
    lintOnStart: false,
  },
  pinia: {
    autoImports: [
      'defineStore',
      'storeToRefs',
      ['defineStore', 'definePiniaStore'],
    ],
  },
  i18n: {
    locales: [
      {
        code: 'en',
        file: 'en_US.json',
        name: 'English',
      },
      {
        code: 'hu',
        file: 'hu_HU.json',
        name: 'Hungarian',
      },
      {
        code: 'kr',
        file: 'ko_KR.json',
        name: 'Korean',
      },
      {
        code: 'fr',
        file: 'fr_FR.json',
        name: 'French',
      },
      {
        code: 'ro',
        file: 'ro_RO.json',
        name: 'Romanian',
      },
      {
        code: 'de',
        file: 'de_DE.json',
        name: 'German',
      },
    ],
    lazy: true,
    langDir: '../../../locales/',
    defaultLocale: 'en',
  },
})
