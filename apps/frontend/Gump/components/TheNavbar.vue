<script setup lang="ts">
import { type navState, useUIStore } from '~/stores/ui'

const localePath = useLocalePath()
const ui = useUIStore()

const tabs = [
  {
    name: 'HomeNav',
    path: '/',
    icon: 'homeicon',
  },
  {
    name: 'SearchNav',
    path: '/search',
    icon: 'searchicon',
  },
  {
    name: 'CreateNav',
    path: '/create',
    icon: 'createicon',
  },
  {
    name: 'RecipesNav',
    path: '/recipes',
    icon: 'recipesicon',
  },
  {
    name: 'ProfileNav',
    path: '/profile',
    icon: 'profileicon',
  },
]
</script>

<template>
  <div v-middle relative bottom-0 h-30 w-full flex flex-row items-center gap-4 bg-crimson-50 px-3>
    <RouterLink v-for="tab in tabs" :key="tab.name" v-middle :to="localePath(tab.path)" vertical-center w-full flex flex-col content-center items-center gap-2 decoration-none @click="ui.setActiveNav(tab.path.slice(1) as navState)">
      <img src="~assets/hexagon.svg">

      <img :src="`/_nuxt/assets/${tab.icon}.svg`" absolute>
      <div
        text-1xl text-center font-bold text-crimson-500 text-shadow-crimson
      >
        {{ $t(tab.name) }}
      </div>
    </RouterLink>
  </div>
</template>

<style scoped></style>
