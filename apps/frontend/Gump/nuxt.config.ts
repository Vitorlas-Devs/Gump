export default defineNuxtConfig({
  modules: [
    '@nuxt/devtools',
    '@unocss/nuxt',
    '@pinia/nuxt',
    '@vueuse/nuxt',
    '@pinia-plugin-persistedstate/nuxt',
    '@nuxtjs/eslint-module',
    '@nuxtjs/i18n',
    '@nuxtjs/ionic',
    'nuxt-vitest',
  ],
  imports: {
    // Auto-import these:
    dirs: ['stores', 'stores/shared'],
  },
  components: [
    {
      path: '~/components',
      pathPrefix: false,
    },
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
  piniaPersistedstate: {
    storage: 'localStorage',
  },
  i18n: {
    locales: [
      {
        code: 'en_US',
        file: 'en_US.json',
        name: 'English',
      },
      {
        code: 'hu_HU',
        file: 'hu_HU.json',
        name: 'Hungarian',
      },
      {
        code: 'ko_KR',
        file: 'ko_KR.json',
        name: 'Korean',
      },
      {
        code: 'fr_FR',
        file: 'fr_FR.json',
        name: 'French',
      },
      {
        code: 'ro_RO',
        file: 'ro_RO.json',
        name: 'Romanian',
      },
      {
        code: 'de_DE',
        file: 'de_DE.json',
        name: 'German',
      },
    ],
    lazy: true,
    langDir: '../../../locales/',
    defaultLocale: 'en_US',
  },
  vite: {
    vue: {
      template: {
        compilerOptions: {
          isCustomElement: tag => ['i18n'].includes(tag),
        },
      },
    },
  },
})
