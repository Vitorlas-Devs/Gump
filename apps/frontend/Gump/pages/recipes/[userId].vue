<script setup lang="ts">
const ui = useUIStore()
const userStore = useUserStore()
const recipe = useRecipeStore()
const localePath = useLocalePath()

const userId = ui.params.recipes

const user = await userStore.getUserById(userId)
const userRecipes = await recipe.getUserRecipes(userId)

ui.activeNav = 'Recipes'
</script>

<template>
  <ion-page v-if="user" bg-crimson-50>
    <TheHeader cursor-pointer :subtitle="user.username" :title="$t('RecipesNav')" @click="ui.setParams('user', user.id);navigateTo(localePath(`/user/${user.id}`))" />
    <div grow>
      <RecipeBoxMini v-for="r in userRecipes" :key="r.id" :recipe="r" my-3 />
    </div>
    <TheNavbar />
  </ion-page>
</template>
