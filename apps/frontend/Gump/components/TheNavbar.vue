<script setup lang="ts">
import { tabData, useUIStore } from '~/stores/ui'

const ui = useUIStore()
const localePath = useLocalePath()
</script>

<template>
  <div relative bottom-0 h-max w-full flex="~ row" items-center bg-crimson-50 class="shadow-crimsonUp">
    <RouterLink v-for="tab in tabData" :key="tab.name" :to="localePath(tab.path)" w-full flex="~ col" items-center gap-2 decoration-none @click="ui.setActiveNav(tab.tab)">
      <div mb-8 flex="~ row" items-center justify-center>
        <img src="~assets/hexagon.svg">
        <img :src="`/_nuxt/assets/${tab.name}.svg`" :mb="tab.tab === 'Home' ? 4 : 0" absolute>
      </div>
      <div
        h="1/2" text-1xl absolute bottom-0 text-center font-bold text-crimson-500 text-shadow-crimson
      >
        <p items-center justify-center>
          {{ $t(tab.name) }}
        </p>
        <div v-if="ui.activeNav === tab.tab" absolute bottom--1 left-0 right-0 m-auto h-2 w-8 rounded-full bg-crimson-500 class="shadow-ultraCrimsonDown" />
      </div>
    </RouterLink>
  </div>
</template>

<style scoped></style>
