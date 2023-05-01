<script setup lang="ts">
defineProps<{
  isEdting: boolean
}>()

const recipe = useRecipeStore()
const ui = useUIStore()

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
    ...recipe.ingredients?.reduce((acc, ingredient) => {
      acc[ingredient.name] = 'text-orange-500 font-bold'
      acc[ingredient.volume] = 'text-crimson-500'
      acc[ingredient.value] = 'text-crimson-500'
      return acc
    }, {} as Record<string, string>),
  }
})

const trackedKeys = ref<string[]>([])
</script>

<template>
  <div flex="~ col" mb-90 h-full w-full>
    <div v-if="ui.createMode === 'design'" flex="~ col" items-center justify-between gap-2 px-1>
      <div
        v-for="(step, index) in recipe.currentRecipe?.steps" :key="index"
        flex="~ row" h-full w-full items-center gap-1 rounded-2xl bg-orange-100 px-2
      >
        <p my-1 font-bold>
          {{ index + 1 }}.
        </p>
        <XInput
          v-if="recipe.currentRecipe"
          ref="inputs"
          :track="recipe.ingredients?.map(ingredient => ingredient.name)"
          :text="recipe.currentRecipe.steps[index]"
          :special-values="specialValues"
          h-max w-full border-0 rounded-xl p-2
          @input="recipe.currentRecipe.steps[index] = $event"
          @tracked-keys="trackedKeys = $event"
          @keydown.enter="handleEnter($event, index)"
          @keydown.backspace="handleBackspace($event, index)"
        />
      </div>
      {{ trackedKeys }}
    </div>
  </div>
</template>

<style scoped>

</style>
