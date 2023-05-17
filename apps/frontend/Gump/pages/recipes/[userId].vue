<script setup lang="ts">
const ui = useUIStore()
const userStore = useUserStore()
const recipe = useRecipeStore()

const userId = ui.params.recipes

const user = await userStore.getUserById(userId)
const userRecipes = await recipe.getUserRecipes(userId)

ui.activeNav = 'Recipes'
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :subtitle="user?.username" :title="$t('RecipesNav')" />
    <div grow>
      <RecipeBoxMini v-for="r in userRecipes" :key="r.id" :recipe="r" my-3 />
    </div>
    <TheNavbar />
  </ion-page>
</template>
