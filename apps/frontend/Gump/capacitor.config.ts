import type { CapacitorConfig } from '@capacitor/cli'

const config: CapacitorConfig = {
  appId: 'gump.live',
  appName: 'Gump',
  webDir: 'dist',
  bundledWebRuntime: false,
  server: {
    hostname: 'gump.live',
  },
}

export default config
