<script setup lang="ts">
const props = defineProps<{
  topPosition?: number
  showResults?: boolean
}>()

const emit = defineEmits<{
  (event: 'handleHistoryClick', search: string): void
}>()

const ui = useUIStore()
const recipe = useRecipeStore()

const topPosition = computed(() => {
  if (props.topPosition && props.topPosition > 310)
    return 310

  return props.topPosition
})

const foundRecipes = ref<Recipe[]>([])

watch(() => props.showResults, (showResults) => {
  if (showResults) {
    const searchValue = ui.searchValue.trim()
    if (searchValue.length > 2)
      foundRecipes.value = recipe.search(searchValue)
  }
})

function handleHistoryClick(search: string) {
  ui.searchValue = search
  ui.addSearchHistory(search)
  emit('handleHistoryClick', search)
  if (props.showResults) {
    ui.dropdownToggled = false
  } else {
    const searchValue = ui.searchValue.trim()
    if (searchValue.length > 2)
      foundRecipes.value = recipe.search(searchValue)
  }
}
</script>

<template>
  <div
    flex="~ col" absolute left-0 top-20 z-30 h-max w-full justify-center border-t-2 border-t-orange-500 rounded-b-3 border-t-solid bg-crimson-50 text-orange-500 text-shadow-orange shadow-orange
    :style="{ top: `${topPosition}px` }"
  >
    <div v-if="!showResults" px-4 py-2>
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
        <p
          my-2 cursor-pointer text-xl
          @click="handleHistoryClick(search)"
        >
          {{ search }}
        </p>
      </div>
    </div>
    <div v-else>
      <RecipeBoxMini v-for="foundRecipe in foundRecipes" :key="foundRecipe.id" :recipe="foundRecipe" />
    </div>
  </div>
</template>

<style scoped>

</style>
