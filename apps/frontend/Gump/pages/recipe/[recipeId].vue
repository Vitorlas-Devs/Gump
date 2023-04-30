<script setup lang="ts">
const route = useRoute()
const ui = useUIStore()

const id = route.params.recipeId

ui.activeNav = 'Recipes'
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader title-color="brown" subtitle="Bartók Béla" title="RecipeView" />
    <div grow>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ id }}
      </h1>
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
