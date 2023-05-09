<script setup lang="ts">
import { sorts } from '~/stores/ui'

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
</script>

<template>
  <div flex="~ col" relative z-10 h-20 w-full justify-center bg-crimson-50 p-2 px-3 shadow-orange>
    <div flex="~ row" h-20 w-full items-center justify-between>
      <h1 my-0 truncate text-2xl font-bold :class="`text-${titleColor}-500 text-shadow-${titleColor}`">
        {{ title }}
      </h1>
      <h2 v-if="showModerator" text-xl font-bold text-blue-500 text-shadow-blue>
        {{ $t("ProfileModerator") }}
      </h2>
      <div v-if="showIcons" flex="~ row" w="1/2" justify-evenly>
        <div
          v-for="(sort, index) in sorts"
          :key="sort"
          cursor-pointer
          class="orangeIcon"
          :class="`${icons[index]} ${ui.activeSort === sort ? 'active' : ''}`"
          @click="ui.activeSort = sort"
        />
      </div>
    </div>
    <div v-if="subtitle" truncate text-lg font-bold text-brown-500>
      {{ subtitle }}
    </div>
  </div>
</template>

<style scoped>
.active {
  transform: scale(1.3) translateY(3px);
}
</style>
