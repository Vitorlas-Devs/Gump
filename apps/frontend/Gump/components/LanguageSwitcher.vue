<script setup lang="ts">
import type { LocaleObject } from '@nuxtjs/i18n/dist/runtime/composables'
import { type Tab, tabData, useUIStore } from '~/stores/ui'

const { locales } = useI18n()
const switchLocalePath = useSwitchLocalePath()
const ui = useUIStore()

const localeObjects = locales.value as LocaleObject[]

const route = useRoute()

const tabToRoute = tabData.find(
  element => element.path === route.path.replace(/^\/\w+\//, '/'),
)?.tab as Tab

function setActive() {
  ui.setActiveNav(tabToRoute)
}
</script>

<template>
  <div my-2 flex="~ row" gap-2>
    <NuxtLink v-for="locale in localeObjects" :key="locale.code" :to="switchLocalePath(locale.code)" class="orangeLink" @click="setActive">
      {{ locale.name }}
    </NuxtLink>
  </div>
</template>

<style scoped>

</style>
