<script setup lang="ts">
const props = defineProps<{
  recipe: SearchRecipe | Recipe
}>()

const image = useImageStore()
const localePath = useLocalePath()
const ui = useUIStore()
const user = useUserStore()

const isLiked = ref(false)
const isSaved = ref(false)
const authorName = ref('')

async function viewRecipe(recipeId: number) {
  ui.activeNav = 'Recipes'
  await navigateTo(localePath(`/recipe/${recipeId}`))
}

onMounted(async () => {
  authorName.value = await user.getAuthorNameById(props.recipe.author) ?? ''
  isLiked.value = user.current.likes.includes(props.recipe.id)
  isSaved.value = user.current.recipes.includes(props.recipe.id)
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
      <RecipeFooter :recipe="recipe" @like="isLiked = !isLiked" @save="isSaved = !isSaved" />
    </div>
  </div>
</template>

<style scoped></style>
