import { defineConfig, presetAttributify, presetIcons, presetUno, presetWebFonts, transformerVariantGroup } from 'unocss'

export default defineConfig({
  presets: [
    presetUno(),
    presetAttributify(),
    presetIcons(),
    presetWebFonts({
      fonts: {
        sans: 'Ubuntu',
        mono: 'Single Day',
      },
    }),
  ],
  transformers: [
    transformerVariantGroup(),
  ],
  theme: {
    colors: {
      crimson: {
        50: '#FFF3F6',
        100: '#FFDAE5',
        200: '#FCC8D8',
        400: '#F73470',
        500: '#D12C5F',
        600: '#AB244E',
      },
      orange: {
        50: '#FEE5E3',
        100: '#FCCAC0',
        200: '#F89679',
        400: '#F37927',
        500: '#F35827',
        600: '#F33527',
      },
      brown: {
        500: '#912808',
        900: '#2A1700',
      },
      blue: { 500: '#0842A0' },
      grey: {
        700: '#3A3A3A',
        800: '#2A2A2A',
        900: '#1A1A1A',
      },
      red: { 500: '#ce2c2c' },
      green: { 500: '#3FB950' },
      purple: { 500: '#A371F7' },
      white: { 500: '#FFFFFF' },
    },
    textShadow: {
      DEFAULT: '0px 0px 6px rgba(151, 39, 4, 0.4)',
      orange: '0px 0px 6px rgba(243, 88, 39, 0.4)',
      crimson: '0px 0px 6px rgba(209, 44, 95, 0.4)',
      blue: '0px 0px 6px rgba(8, 66, 160, 0.4)',
      white: '0px 0px 6px rgba(255, 255, 255, 0.4)',
    },
    boxShadow: {
      orange: '0px 8px 10px -4px rgba(243, 88, 39, 0.5)',
      orangeLight: '0px 2px 10px -2px rgba(243, 88, 39, 0.5)',
      orangeBox: '0px 3px 17px -5px #F35827',
      orangeImage: 'inset 0px 25px 20px -20px rgba(243, 88, 39, 0.5)',
      crimson: '0px 8px 10px -4px rgba(209, 44, 95, 0.5)',
      crimsonUltra: '0px -10px 35px 15px rgba(209, 44, 95, 0.7)',
      crimsonUp: '0px -6px 15px -5px rgba(209, 44, 95, 0.3)',
      blue: '0px 8px 10px -4px rgba(8, 66, 160, 0.5)',
      inner: 'inset 0px 2px 12px -4px rgb(243, 88, 39)',
      innerCrimson: 'inset 0px 2px 12px -4px rgb(209, 44, 95)',
      grey: '0px 2px 6px rgba(0, 0, 0, 0.5)',
      leftActive: 'inset 10px 10px 12px -10px rgba(243, 88, 39, 0.4), inset -10px 20px 12px -20px rgba(243, 88, 39, 0.4);',
      midActive: 'inset -10px 20px 12px -20px rgba(243, 88, 39, 0.4), inset 10px 20px 12px -20px rgba(243, 88, 39, 0.4);',
      rightActive: 'inset -10px 10px 12px -10px rgba(243, 88, 39, 0.4), inset 10px 20px 12px -20px rgba(243, 88, 39, 0.4);',
      inactive: '0px 8px 12px -8px rgba(243, 88, 39, 0.4)',
    },
  },
  shortcuts: [
    {
      coolOl:
      'my-2 pl-6 list-outside list-disc space-y-2 leading-relaxed',
    },
    [/^([a-z]+)Btn$/, ([, c]) => `w-max rounded-full bg-${c}-500 px-6 py-4 font-bold text-white-500 shadow-${c}`],
    [/^([a-z]+)Link$/, ([, c]) => `text-${c}-500 text-shadow-${c} underline underline-offset-5 underline-${c}-500 font-bold`],
    [/^([a-z]+)Icon$/, ([, c]) => `w-6 h-6 text-${c}-500`],
    [/^([a-z]+)Gradient$/, ([, c]) => `bg-gradient-to-rt from-${c}-600 to-${c}-400`],
  ],
})
