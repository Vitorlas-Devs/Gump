export default defineNuxtConfig({
  modules: [
    '@nuxt/devtools',
    '@unocss/nuxt',
    '@nuxtjs/eslint-module',
    '@nuxtjs/i18n',
    '@nuxtjs/ionic',
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
        name: 'Hungarian',
      },
      {
        code: 'kr',
        file: 'ko_KR.json',
        name: 'Korean',
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
