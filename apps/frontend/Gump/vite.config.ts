import { fileURLToPath, URL } from 'node:url'
import { resolve, dirname } from 'node:path'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import WindiCSS from 'vite-plugin-windicss'
import VueI18nPlugin from '@intlify/unplugin-vue-i18n/vite'
import { VitePluginFonts } from 'vite-plugin-fonts'

export default defineConfig({
  plugins: [
    vue(),
    WindiCSS(),
    VueI18nPlugin({
      include: resolve(dirname(fileURLToPath(import.meta.url)), './../../../locales/**'),
      runtimeOnly: false
    }),
    VitePluginFonts({
      google: {
        families: ['Ubuntu']
      }
    })
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  define: {
    __VUE_I18N_FULL_INSTALL__: true,
    __VUE_I18N_LEGACY_API__: false,
    __INTLIFY_PROD_DEVTOOLS__: false
  }
})
