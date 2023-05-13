<script setup lang="ts">
const props = defineProps<{
  isEditing?: boolean
  currentRecipe?: Recipe
}>()

const recipe = useRecipeStore()
const ui = useUIStore()

const currentRecipe = computed(() => {
  return props.currentRecipe ? props.currentRecipe : recipe.currentRecipe
})

const inputs = ref<HTMLElement[]>([])

function handleEnter(e: Event, index: number) {
  (e?.target as HTMLElement).blur()
  e.preventDefault()
  recipe.addEmptyStep(index)
  nextTick(() => {
    inputs.value[index + 1]?.focus()
  })
}

function handleBackspace(e: Event, index: number) {
  if ((e?.target as HTMLElement).textContent === '') {
    e.preventDefault()
    recipe.removeStep(index)
    inputs.value[index - 1]?.focus()
  }
}

const specialValues = computed(() => {
  return {
    ...currentRecipe.value?.ingredients?.reduce((acc, ingredient) => {
      acc[ingredient.name] = 'text-orange-500 font-bold'
      acc[ingredient.volume] = 'text-crimson-500 font-bold'
      acc[ingredient.value] = 'text-crimson-500 font-bold'
      return acc
    }, {} as Record<string, string>),
  }
})

const trackedKeys = ref<string[][]>([])

const foundRecipesList: { steps: string[]; trackedKey: string; index: number; displayed: boolean }[] = reactive([])

const toggledCarets = ref<boolean[]>([])

const computedfoundRecipes = computed(() => {
  if (!currentRecipe.value?.ingredients)
    return

  foundRecipesList.splice(0, foundRecipesList.length)
  toggledCarets.value = []

  trackedKeys.value.forEach((trackedKeys, index) => {
    const foundIngredients = currentRecipe.value?.ingredients?.filter(ingredient => trackedKeys.includes(ingredient.name))
    if (foundIngredients?.length) {
      const foundRecipesIds = foundIngredients.map(ingredient => ingredient.linkedRecipe)
      const foundRecipes = recipe.recipes.filter(recipe => foundRecipesIds.includes(recipe.id))
      const foundSteps = foundRecipes.map(recipe => recipe.steps)
      foundSteps.forEach((steps, stepIndex) => {
        foundRecipesList.push({ steps, index, trackedKey: trackedKeys[stepIndex], displayed: false })
      })
    }
  })

  return foundRecipesList
})

function showLinkedRecipe(index: number) {
  computedfoundRecipes.value?.forEach((foundRecipe) => {
    if (foundRecipe.index === index)
      foundRecipe.displayed = !foundRecipe.displayed
  })
}

function toggleCaret(index: number) {
  toggledCarets.value[index] = !toggledCarets.value[index]
}

function checkDone() {
  if (recipe.currentRecipe) {
    if (recipe.currentRecipe.steps.every(step => step.length > 0))
      ui.createHeaderStates[2] = true
    else
      ui.createHeaderStates[2] = false
  }
}

watch(() => recipe.currentRecipe?.steps, () => {
  checkDone()
}, { immediate: true, deep: true })
</script>

<template>
  <div flex="~ col" mb-90 h-full w-full>
    <div v-if="ui.createMode === 'design'" flex="~ col" items-center justify-between gap-2 px-1>
      <div
        v-for="(step, index) in currentRecipe?.steps" :key="index"
        flex="~ col" h-full w-full items-center gap-0 px-1
      >
        <div
          flex="~ row" h-full w-full items-center gap-1 rounded-2xl bg-orange-50 px-2
        >
          <p my-1 font-bold>
            {{ index + 1 }}.
          </p>
          <XInput
            v-if="currentRecipe"
            ref="inputs"
            :track="currentRecipe.ingredients?.map(ingredient => ingredient.name)"
            :text="step"
            :special-values="specialValues"
            :readonly="!isEditing"
            h-full w-full self-center border-0 rounded-xl p-2
            @input="currentRecipe.steps[index] = $event"
            @tracked-keys="trackedKeys[index] = $event"
            @keydown.enter="handleEnter($event, index)"
            @keydown.backspace="handleBackspace($event, index)"
          />
          <div
            :class="trackedKeys[index] && trackedKeys[index].length > 0 ? toggledCarets[index] ? 'visible i-fa6-solid-caret-up' : 'visible i-fa6-solid-caret-down' : 'invisible'"
            orangeIcon h-5 w-5 cursor-pointer px-4
            @click="showLinkedRecipe(index); toggleCaret(index)"
          />
        </div>
        <div
          v-if="computedfoundRecipes"
          w-full
        >
          <div
            v-for="foundRecipe in computedfoundRecipes"
            :key="foundRecipe.index"
          >
            <div
              v-if="foundRecipe.index === index && foundRecipe.displayed"
              transform="rotate--90"
              mx-a my-3 w-5 border-2 border-orange-200 rounded-full border-dashed
            />
            <div
              v-if="foundRecipe.index === index && foundRecipe.displayed"
              flex="~ col" gap-1 rounded-2xl bg-orange-50 px-2 py-1
            >
              <p my-0 font-bold text-orange-500>
                {{ foundRecipe.trackedKey }}
              </p>
              <p
                v-for="(subSstep, subIndex) in foundRecipe.steps"
                :key="subIndex" my-0 ml-4
              >
                {{ subIndex + 1 }}. {{ subSstep }}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
