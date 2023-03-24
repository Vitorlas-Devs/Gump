import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import WindiCSS from 'vite-plugin-windicss'
import { VitePluginFonts } from 'vite-plugin-fonts'
import svgLoader from 'vite-svg-loader'
import { nodePolyfills } from 'vite-plugin-node-polyfills'

interface ViteEnv {
  [key: string]: string
}

const viteEnv: ViteEnv = {}

Object.keys(process.env).forEach((key) => {
  if (key.startsWith('VITE_')) {
    viteEnv[`import.meta.env.${key}`] = process.env[key]!
  }
})

export default defineConfig({
  plugins: [
    vue(),
    WindiCSS(),
    VitePluginFonts({
      google: {
        families: ['Ubuntu']
      }
    }),
    svgLoader(),
    nodePolyfills({
      protocolImports: true
    })
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  define: viteEnv,
})
