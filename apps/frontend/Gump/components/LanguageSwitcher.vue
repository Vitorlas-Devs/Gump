<script setup lang="ts">
import type { LocaleObject } from '@nuxtjs/i18n/dist/runtime/composables'

const { locales } = useI18n()
const switchLocalePath = useSwitchLocalePath()

const localeObjects = locales.value as LocaleObject[]
</script>

<template>
  <div my-2 flex="~ row" gap-2>
    <NuxtLink v-for="locale in localeObjects" :key="locale.code" :to="switchLocalePath(locale.code)" class="linkOrange">
      {{ locale.name }}
    </NuxtLink>
  </div>
</template>

<style scoped>

</style>
