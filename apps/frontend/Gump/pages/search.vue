<script setup lang="ts">
import type { LocaleObject } from '@nuxtjs/i18n/dist/runtime/composables'

const route = useRoute()
const { locales } = useI18n()
const switchLocalePath = useSwitchLocalePath()
const localePath = useLocalePath()

const localeObjects = locales.value as LocaleObject[]
</script>

<template>
  <div>
    <h1 text-crimson-500 text-shadow-crimson>
      {{ $t('SearchNav') }}
    </h1>
    <p>
      Current route: {{ route.path }}
    </p>
    <NuxtLink class="linkOrange" :to="localePath('/')">
      {{ $t('HomeNav') }}
    </NuxtLink>
    <br><br>
    Select Language:
    <NuxtLink v-for="locale in localeObjects" :key="locale.code" :to="switchLocalePath(locale.code)" m="2" class="linkOrange">
      {{ locale.name }}
    </NuxtLink>
  </div>
</template>

<style scoped>

</style>
