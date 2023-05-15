<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()
const user = useUserStore()

ui.activeNav = 'Home'

watch(
  () => ui.activeSort,
  async () => await recipe.getRecipesBySort(ui.activeSort),
  { immediate: true },
)

onMounted(async () => {
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
    <div flex="~ row" absolute z-100 h-full w-full items-center justify-center bg-crimson-50>
      <div
        class="i-fa6-solid-circle-check"
        h-50 w-50 from-crimson-500 to-orange-500 bg-gradient-to-rt
        style="filter: drop-shadow(0px 8px 10px -4px rgba(243, 88, 39, 0.5));"
      />
    </div>
    <div v-if="recipe.recipes" grow overflow-y-auto pb-30>
      <RecipeBox v-for="r of recipe.recipes" :key="r.id" :recipe="r" @like="handleLiked(r)" @save="handleSaved(r)" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped></style>
