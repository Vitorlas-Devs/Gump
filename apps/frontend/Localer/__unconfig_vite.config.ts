
let __unconfig_data;
let __unconfig_stub = function (data = {}) { __unconfig_data = data };
__unconfig_stub.default = (data = {}) => { __unconfig_data = data };
import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import WindiCSS from 'vite-plugin-windicss'
import { VitePluginFonts } from 'vite-plugin-fonts'
import svgLoader from 'vite-svg-loader'
import { nodePolyfills } from 'vite-plugin-node-polyfills'
import { viteStaticCopy } from 'vite-plugin-static-copy'
import vueJsx from '@vitejs/plugin-vue-jsx'

const __unconfig_default =  () => {
  return defineConfig({
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
      }),
      viteStaticCopy({
        targets: [
          {
            src: '_redirects',
            dest: '.'
          }
        ]
      }),
      vueJsx()
    ],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      }
    },
    define: {
      __VUE_PROD_DEVTOOLS__: true
    }
  })
}

if (typeof __unconfig_default === "function") __unconfig_default(...[{"command":"serve","mode":"development"}]);export default __unconfig_data;