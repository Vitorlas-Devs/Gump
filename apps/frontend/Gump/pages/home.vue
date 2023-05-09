<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()
const user = useUserStore()

watch(
  () => ui.activeSort,
  async () => await recipe.getSearchRecipes(ui.activeSort),
  { immediate: true },
)

ui.activeNav = 'Home'

onMounted(async () => {
  await user.getUserData()
})
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader show-icons :title="$t('HomeNav')" />
    <div v-if="recipe.searchRecipes" grow overflow-y-auto pb-30>
      <RecipeBox v-for="r of recipe.searchRecipes" :key="r.id" :recipe="r" />
      <MainButton fixed color="orange" icon-type="create" title="Create recipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped></style>
