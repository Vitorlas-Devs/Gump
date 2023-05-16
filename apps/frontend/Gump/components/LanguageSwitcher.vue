<script setup lang="ts">
import type { LocaleObject } from '@nuxtjs/i18n/dist/runtime/composables'

const { locales } = useI18n()
const localePath = useLocalePath()
const user = useUserStore()

const localeObjects = locales.value as LocaleObject[]

const localeNames = computed(() => {
  return localeObjects.map(locale => locale.name) as string[]
})

const langs = computed({
  get: () => {
    const language = localeObjects.find(locale => locale.code === user.current.language)
    return language?.name ?? ''
  },
  set: async (value: string) => {
    const language = localeObjects.find(lang => lang.name === value)
    if (language) {
      user.current.language = language.code
      await navigateTo(localePath('/profile', language.code))
    } else {
      user.current.language = ''
    }
  },
})
</script>

<template>
  <div my-2 flex="~ row" gap-2>
    <SearchSelect v-model:model="langs" w-60 :options="localeNames" mode="single" />
  </div>
</template>
