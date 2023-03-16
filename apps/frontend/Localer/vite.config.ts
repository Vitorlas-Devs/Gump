import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import WindiCSS from 'vite-plugin-windicss'
import { VitePluginFonts } from 'vite-plugin-fonts'

export default defineConfig({
  plugins: [
    vue(),
    WindiCSS(),
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
  }
})
