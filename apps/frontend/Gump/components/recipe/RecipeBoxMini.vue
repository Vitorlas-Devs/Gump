<script setup lang="ts">
defineProps<{
  recipe: Recipe
}>()

const image = useImageStore()
const ui = useUIStore()
const recipeStore = useRecipeStore()
const localePath = useLocalePath()

function addRecipe(recipe: Recipe) {
  recipeStore.addRecipe(recipe)
}

function viewRecipe(recipeId: number) {
  navigateTo(localePath(`/recipe/${recipeId}`))
}
</script>

<template>
  <div flex="~ justify-between" shadow-orangebox m-4 max-h-35 w-auto cursor-pointer rounded-2xl bg-orange-50>
    <div flex="~" @click="viewRecipe(recipe.id)">
      <img :src="image.getImage(recipe.image)" w="1/3" rounded-s-2xl object-cover>
      <div grow self-center text-shadow>
        <p m-3 text-xl>
          {{ recipe.title }}
        </p>
        <p m-3 text-lg>
          {{ recipe.author }}
        </p>
      </div>
    </div>
    <img v-if="ui.activeNav === 'Create'" src="~assets/PlusSign.svg" m-3 @click="addRecipe(recipe)">
  </div>
</template>
