<script setup lang="ts">
import { debounce } from 'lodash-es'

const props = defineProps<{
  isEdting?: boolean
  currentRecipe?: Recipe
}>()

const ui = useUIStore()
const recipe = useRecipeStore()

const currentRecipe = computed(() => {
  return props.currentRecipe ? props.currentRecipe : recipe.currentRecipe
})

const rawInput = ref<HTMLTextAreaElement>()
const ingredientInputs = ref<HTMLInputElement[]>([])
const inputField = ref<HTMLDivElement>()
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

onClickOutside(inputField, () => {
  toggleDropdown.value = false
})

const debouncedDropdown = debounce(() => {
  toggleResults.value = true
}, 1000)

function handleInput(e: Event) {
  // e is to be used in the future (to send the data)
  if ((e.target as HTMLInputElement).value.length > 2) {
    toggleDropdown.value = true
    toggleResults.value = false
    ui.searchValue = (e.target as HTMLInputElement).value
    debouncedDropdown()
  }
}

function handleHistoryClick(search: string) {
  recipe.addEmptyIngredient()
  if (currentRecipe.value)
    currentRecipe.value.ingredients[currentRecipe.value.ingredients.length - 1].name = search

  toggleDropdown.value = false
  toggleResults.value = false
  nextTick(() => {
    ingredientInputs.value[ingredientInputs.value.length - 1]?.focus()
  })
}

watch(() => recipe.ingredients, () => {
  if (recipe.ingredients.every(ingredient => ingredient.name && ingredient.value && ingredient.volume))
    ui.setCreateHeaderIndex(true)
  else
    ui.setCreateHeaderIndex(false)
}, { deep: true })

const dropdowns = ref<boolean[]>([])

watch(() => recipe.ingredients, () => {
  dropdowns.value = recipe.ingredients.map(() => false)
}, { deep: true })

function toggleIngredient(index: number) {
  dropdowns.value[index] = !dropdowns.value[index]
}

function handleBackspace(e: Event, index: number) {
  if ((e?.target as HTMLInputElement).value === '') {
    e.preventDefault()
    recipe.removeIngredient(index)
    ingredientInputs.value[index - 1]?.focus()
  }
}

const recipesById = computed(() => {
  const result: Record<number, Recipe> = {}
  recipe.recipes.forEach((recipe) => {
    result[recipe.id] = recipe
  })
  return result
})
</script>

<template>
  <div flex="~ col" mb-90 h-full w-full>
    <div v-if="ui.createMode === 'design'" ref="inputField" flex="~ col" items-center justify-between>
      <div
        v-for="(ingredient, index) in currentRecipe?.ingredients" :key="index"
        flex="~ col" mx-1 h-full w-full items-center justify-between gap-2
        :class="ingredient.linkedRecipe === null ? '' : 'recipeInput border-b-2 border-orange-200 border-b-solid'"
      >
        <div flex="~ row" h-full w-full items-center justify-between gap-2>
          <input
            ref="ingredientInputs"
            v-model="ingredient.name"
            :placeholder="`${$t('CreateIngredientsTip')}...`"
            w-full flex-1 border-0 p-2
            :class="ingredient.linkedRecipe === null ? 'border-b-1 border-orange-500' : ''"
            :readonly="!isEdting"
            @input="recipe.checkEmptyIngredients(); handleInput($event)"
            @focus="handleInputFocus($event)"
            @keydown.enter="handleInputBlur($event)"
            @keydown.backspace="handleBackspace($event, index)"
          >
          <div v-if="ingredient.linkedRecipe === null" flex="~ row" gap-2>
            <input
              v-model="ingredient.value"
              type="number"
              placeholder="0"
              w-10 border-0 border-b-1 border-orange-500 p-2
              :readonly="!isEdting"
              @input="recipe.checkEmptyIngredients"
            >
            <input
              v-model="ingredient.volume"
              placeholder="cup"
              w-18 border-0 border-b-1 border-orange-500 p-2
              :readonly="!isEdting"
              @input="recipe.checkEmptyIngredients"
            >
          </div>
          <div
            v-else
            :class="dropdowns[index] ? 'i-fa6-solid-chevron-up' : 'i-fa6-solid-chevron-down'"
            mr-2 cursor-pointer
            @click="toggleIngredient(index)"
          />
        </div>
        <div
          v-if="dropdowns[index] && ingredient.linkedRecipe !== null && recipesById[ingredient.linkedRecipe]"
          w-full
        >
          <div
            v-for="(subIngredient, subIndex) in recipesById[ingredient.linkedRecipe].ingredients"
            :key="subIndex"
            flex="~ row" ml-6 items-center justify-between gap-2
          >
            <p my-0 text-3xl>
              -
            </p>
            <input
              :value="subIngredient.name"
              placeholder="name"
              w-full flex-1 border-0 border-t-1 border-orange-500 p-2
              :readonly="true"
            >
            <input
              :value="subIngredient.value"
              type="number"
              placeholder="0"
              w-10 border-0 border-t-1 border-orange-500 p-2
              :readonly="true"
            >
            <input
              :value="subIngredient.volume"
              placeholder="cup"
              w-18 border-0 border-t-1 border-orange-500 p-2
              :readonly="true"
            >
          </div>
        </div>
      </div>
      <SearchDropdown
        v-if="toggleDropdown && !readonly"
        :top-position="dropdownTop" :show-results="toggleResults"
        @handle-history-click="handleHistoryClick"
      />
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
.recipeInput {
  background-color: #FEE5E3 !important;
}
</style>
