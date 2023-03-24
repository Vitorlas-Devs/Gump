import { fileURLToPath, URL } from 'node:url'
import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import WindiCSS from 'vite-plugin-windicss'
import { VitePluginFonts } from 'vite-plugin-fonts'
import svgLoader from 'vite-svg-loader'
import { nodePolyfills } from 'vite-plugin-node-polyfills'

export default ({ mode, command }: { mode: string; command: string }) => {
  const env = loadEnv(mode, process.cwd(), '')
  if (command === 'serve') {
    return defineConfig({
      define: {
        'process.env.VITE_REPO': `"${env.VITE_REPO}"`,
        'process.env.VITE_DEV': 'true'
      },
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
      }
    })
  }
  return defineConfig({
    define: {
      'process.env.VITE_REPO': 'process.env.VITE_REPO',
      'process.env.VITE_DEV': 'false'
    },
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
    }
  })
}
