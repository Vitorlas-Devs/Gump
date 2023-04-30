<script setup lang="ts">
defineProps<{
  isEdting: boolean
}>()

const recipe = useRecipeStore()
const ui = useUIStore()

const inputs = ref<HTMLInputElement[]>([])

function handleEnter(e: Event, index: number) {
  (e?.target as HTMLInputElement).blur()
  recipe.addEmptyStep(index)
  nextTick(() => {
    inputs.value[index + 1]?.focus()
  })
}

function handleBackspace(e: Event, index: number) {
  if ((e?.target as HTMLInputElement).value === '') {
    e.preventDefault()
    recipe.removeStep(index)
    inputs.value[index - 1]?.focus()
  }
}
</script>

<template>
  <div flex="~ col" mb-90 h-full w-full>
    <div v-if="ui.createMode === 'design'" flex="~ col" items-center justify-between gap-2 px-1>
      <div
        v-for="(step, index) in recipe.currentRecipe?.steps" :key="index"
        flex="~ row" h-full w-full items-center justify-between rounded-2xl bg-orange-100 px-2
      >
        <p my-1 font-bold>
          {{ index + 1 }}.
        </p>
        <input
          v-if="recipe.currentRecipe"
          ref="inputs"
          v-model="recipe.currentRecipe.steps[index]"
          :readonly="!isEdting"
          h-max w-full border-0 rounded-xl p-2
          @keydown.enter="handleEnter($event, index)"
          @keydown.backspace="handleBackspace($event, index)"
        >
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
