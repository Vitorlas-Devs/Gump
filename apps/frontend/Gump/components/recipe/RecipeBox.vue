<script setup lang="ts">
const props = defineProps<{
  recipe: SearchRecipe | Recipe
}>()

defineEmits<{
  (event: 'like'): void
  (event: 'save'): void
}>()

const image = useImageStore()
const localePath = useLocalePath()
const ui = useUIStore()
const user = useUserStore()

const authorName = ref('')

async function viewRecipe(recipeId: number) {
  ui.activeNav = 'Recipes'
  ui.setParams('recipe', recipeId)
  await navigateTo(localePath(`/recipe/${recipeId}`))
}

onMounted(async () => {
  authorName.value = await user.getAuthorById(props.recipe.author) ?? ''
})
</script>

<template>
  <div flex="~ col" shadow-orangeBox m-4 h-262px rounded-2xl bg-orange-50>
    <img :src="image.getImage(recipe.image)" h="367/593" w-full cursor-pointer self-center rounded-t-2xl object-cover @click="viewRecipe(recipe.id)">
    <div flex="~ col" shadow-orangeImage px-1 text-center text-shadow>
      <div cursor-pointer @click="viewRecipe(recipe.id)">
        <p my-1 truncate text-xl>
          {{ recipe.title }}
        </p>
        <p my-1 truncate text-lg>
          {{ authorName }}
        </p>
      </div>
      <RecipeFooter :recipe="recipe" @like="$emit('like')" @save="$emit('save')" />
    </div>
  </div>
</template>

<style scoped></style>
