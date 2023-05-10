<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()
const user = useUserStore()

ui.activeNav = 'Home'

watch(
  () => ui.activeSort,
  async () => await recipe.getRecipes(ui.activeSort),
)

onMounted(async () => {
  recipe.cachedRecipes = {
    hot: [],
    new: [],
    top: [],
  }
  await recipe.getRecipes(ui.activeSort)
  await user.getUserData()
})

function handleLiked(r: SearchRecipe) {
  if (r) {
    r.isLiked = !r.isLiked
    r.likeCount += r.isLiked ? 1 : -1
  }
}

function handleSaved(r: SearchRecipe) {
  if (r) {
    r.isSaved = !r.isSaved
    r.saveCount += r.isSaved ? 1 : -1
  }
}
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader show-icons :title="$t('HomeNav')" />
    <div v-if="recipe.searchRecipes" grow overflow-y-auto pb-30>
      <RecipeBox v-for="r of recipe.searchRecipes" :key="r.id" :recipe="r" @like="handleLiked(r)" @save="handleSaved(r)" />
      <MainButton fixed color="orange" icon-type="create" title="Create recipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped></style>
