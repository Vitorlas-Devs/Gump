<script setup lang="ts">
import { debounce } from 'lodash-es'
import { useUIStore } from '~/stores/ui'

defineProps<{
  isEdting: boolean
}>()

const ui = useUIStore()

const rawInput = ref<HTMLTextAreaElement>()
const ingredientInput = ref<HTMLInputElement>()
const toggleDropdown = ref(false)
const toggleResults = ref(false)
const dropdownTop = ref(0)

function handleInputFocus(e: FocusEvent) {
  const inputTop = (e.target as HTMLInputElement).getBoundingClientRect().top
  toggleDropdown.value = true
  dropdownTop.value = inputTop + 35
  toggleResults.value = false
}

function handleInputBlur(e?: Event) {
  (e?.target as HTMLInputElement).blur()
  toggleDropdown.value = false
}

const debouncedDropdown = debounce(() => {
  toggleResults.value = true
}, 1000)

function handleInput(e: Event) {
  toggleDropdown.value = true
  debouncedDropdown()
}

watch(() => ui.currentRecipe.ingredients, () => {
  if (ui.currentRecipe.ingredients.every(ingredient => ingredient.name && ingredient.value && ingredient.volume))
    ui.setCreateHeaderIndex(true)
  else
    ui.setCreateHeaderIndex(false)
}, { deep: true })
</script>

<template>
  <div flex="~ col" mb-90 h-full w-full>
    <div v-if="ui.createMode === 'design'" flex="~ col" items-center justify-between>
      <div
        v-for="(ingredient, index) in ui.currentRecipe.ingredients" :key="index"
        flex="~ row" mx-1 h-full w-full items-center justify-between gap-2
      >
        <input
          ref="ingredientInput"
          v-model="ingredient.name"
          :placeholder="`${$t('CreateIngredientsTip')}...`" w-full flex-1 border-0 border-b-1 border-orange-500
          p-2
          :readonly="!isEdting"
          @input="ui.checkEmptyIngredients(); handleInput($event)"
          @focus="handleInputFocus($event)"
          @blur="handleInputBlur"
          @keydown.enter="handleInputBlur($event)"
        >
        <input
          v-model="ingredient.value"
          type="number"
          placeholder="0"
          w-10 border-0 border-b-1 border-orange-500 p-2
          :readonly="!isEdting"
          @input="ui.checkEmptyIngredients"
        >
        <input
          v-model="ingredient.volume"
          placeholder="cup"
          w-18 border-0 border-b-1 border-orange-500 p-2
          :readonly="!isEdting"
          @input="ui.checkEmptyIngredients"
        >
      </div>
      <SearchDropdown v-if="toggleDropdown" :top-position="dropdownTop" :show-results="toggleResults" />
    </div>
    <textarea
      v-else
      ref="rawInput"
      class="h-full w-full p-2"
      placeholder="Enter ingredients here..."
      border-0 bg-crimson-50 shadow-inner
    />
  </div>
</template>

<style scoped>

</style>
