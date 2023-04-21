<script setup lang="ts">
import { sorts, useUIStore } from '~/stores/ui'

// TheHeader component has 4 variants: Home, RecipeView, Recipes, Profile

// Home:
// titleColor: text-orange-500
// 3 icons: hot, new, top sorting options for recipes
// no subtitle

// RecipeView:
// titleColor: text-brown-500
// subtitle: recipe author
// no icons

// Recipes:
// titleColor: text-orange-500
// no icons, no subtitle

// Profile:
// titleColor: text-orange-500
// moderator label: text-blue and on the right side
// no icons, no subtitle

withDefaults(defineProps<{
  title: string
  subtitle?: string
  titleColor?: string
  showModerator?: boolean
  showIcons?: boolean
}>(), {
  titleColor: 'orange',
  showModerator: false,
  showIcons: false,
})

const ui = useUIStore()

const icons = ['i-fa6-solid-fire', 'i-ion-sparkles', 'i-fa6-solid-trophy']

ui.activeNav = 'Recipes'
</script>

<template>
  <div h-20 w-full flex="~ col" bg-crimson-50 p-2 px-3 shadow-orange>
    <div flex="~ row" :h="subtitle ? 10 : 'max'" w-full items-center justify-between>
      <h1 text-2xl font-bold :class="`text-${titleColor}-500 text-shadow-${titleColor}`">
        {{ title }}
      </h1>
      <h2 v-if="showModerator" text-xl font-bold text-blue text-shadow-blue>
        {{ $t("ProfileModerator") }}
      </h2>
      <div v-if="showIcons" flex="~ row" w="1/2" justify-evenly>
        <div
          v-for="(sort, index) in sorts"
          :key="sort"
          :class="`${icons[index]} iconOrange ${ui.activeSort === sort ? 'active' : ''}`"
          @click="ui.activeSort = sort"
        />
      </div>
    </div>
    <div v-if="subtitle" text-lg font-bold text-brown-500>
      {{ subtitle }}
    </div>
  </div>
</template>

<style scoped>
.active {
  transform: scale(1.3) translateY(3px);
}
</style>
