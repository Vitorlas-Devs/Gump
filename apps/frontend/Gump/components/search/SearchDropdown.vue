<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

const ui = useUIStore()
</script>

<template>
  <div flex="~ col" absolute left-0 top-20 h-max w-full justify-center border-t-2 border-t-orange-500 rounded-b-3 border-t-solid bg-crimson-50 px-4 py-2 text-orange-500 text-shadow-orange shadow-orange>
    <div flex="~ row" w-full items-center justify-between>
      <p my-1 text-xl>
        {{ $t('SearchRecent') }}
      </p>
      <p my-1>
        {{ $t('SearchTip') }}
      </p>
    </div>
    <div v-for="search in ui.getSearchHistory" :key="search" flex="~ row" w-full items-center gap-4>
      <div class="i-fa6-solid-clock-rotate-left orangeIcon" />
      <p my-2 class="text-xl">
        {{ search }}
      </p>
    </div>
  </div>
</template>

<style scoped>

</style>
