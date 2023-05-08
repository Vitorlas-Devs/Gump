<script setup lang="ts">
const ui = useUIStore()
const recipe = useRecipeStore()
const user = useUserStore()
const image = useImageStore()

const id = ui.params.recipe
ui.activeNav = 'Recipes'

const currentRecipe = await recipe.getRecipeById(id)
const authorName = await user.getAuthorNameById(currentRecipe?.author as number)

const isLiked = ref<boolean>(user.likes.includes(currentRecipe?.id as number))
const isSaved = ref<boolean>(user.recipes.includes(currentRecipe?.id as number))
</script>

<template>
  <ion-page v-if="currentRecipe" bg-crimson-50>
    <TheHeader title-color="brown" :subtitle="authorName" :title="currentRecipe.title" />
    <div grow>
      <img :src="image.getImage(currentRecipe.image)" h-40 w-full object-cover>
      <RecipeFooter :id="id" :view-count="currentRecipe.viewCount" :like-count="currentRecipe.likeCount" :save-count="currentRecipe.saveCount" :is-liked="isLiked" :is-saved="isSaved" @like="isLiked = !isLiked" @save="isSaved = !isSaved" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
