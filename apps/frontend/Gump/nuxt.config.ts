export default defineNuxtConfig({
  modules: [
    '@nuxt/devtools',
    '@unocss/nuxt',
    '@nuxtjs/eslint-module',
    '@nuxtjs/i18n',
  ],
  eslint: {
    lintOnStart: false,
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
        name: 'Magyar',
      },
      {
        code: 'kr',
        file: 'ko_KR.json',
        name: '한국어',
      },
    ],
    lazy: true,
    langDir: '../../../locales',
    defaultLocale: 'en',
    vueI18n: {
      fallbackLocale: 'en',
    },
  },
})
