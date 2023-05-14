<script setup lang="ts">
const ui = useUIStore()
const recipe = useRecipeStore()

const isSearched = ref(false)

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
    <SearchHeader z-2 @search="isSearched = true" />
    <div v-if="isSearched" h-76vh grow overflow-y-auto>
      <RecipeBox v-for="r of recipe.recipes" :key="r.id" :recipe="r" @like="handleLiked(r)" @save="handleSaved(r)" />
    </div>
    <div grow />
    <TheNavbar z-2 />
    <div
      v-if="ui.dropdownToggled"
      absolute left-0 top-0 h-full w-full bg-brown-900 opacity-20
    />
  </ion-page>
</template>
