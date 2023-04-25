<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

const ui = useUIStore()

ui.searchToggled = false

const searchInput = ref<HTMLElement>()
const searchBox = ref<HTMLElement>()

function handleSearch() {
  ui.searchToggled = true
  ui.dropdownToggled = true
  nextTick(() => searchInput.value?.focus())
}

onClickOutside(searchBox, () => {
  ui.searchToggled = false
  ui.dropdownToggled = false
})
</script>

<template>
  <div ref="searchBox" flex="~ col" h-20 w-full justify-center bg-crimson-50 p-2 px-3 shadow-orange>
    <div
      v-if="!ui.searchToggled"
      flex="~ row" shadow-orangeLight h-10 w-full items-center rounded-full px-4
      @click="handleSearch"
    >
      <h1 h-10 cursor-pointer text-2xl font-bold text-orange-500>
        {{ $t('SearchNav') }}
      </h1>
    </div>
    <input
      v-else
      ref="searchInput"
      v-model="ui.searchValue"
      h-10 w-full border-0 text-lg text-orange-500
      @keydown.enter="ui.addSearchHistory(ui.searchValue)"
    >
    <SearchDropdown v-if="ui.searchToggled" />
  </div>
</template>

<style scoped>

</style>
