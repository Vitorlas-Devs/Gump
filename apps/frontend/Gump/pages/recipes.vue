<script setup lang="ts">
const recipeSort = ref('owned')
const recipe = useRecipeStore()
const user = useUserStore()

const currentSortRecipes = computed(() => {
  switch (recipeSort.value) {
    case 'owned':
      return recipe.recipes.filter(r => r.author === user.current.id)
    case 'likes':
      return recipe.recipes.filter(r => r.isLiked)
    case 'saved':
      return recipe.recipes.filter(r => r.isSaved)
    default:
      return recipe.recipes
  }
})
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('RecipesNav')" />
    <div grow overflow-y-auto>
      <div flex="~ row" my-10 items-center justify-around>
        <div :class="recipeSort === 'owned' ? 'bg-orange-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-orange-500' : 'text-orange-500 text-shadow-orange'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'owned'">
          Owned
        </div>
        <div :class="recipeSort === 'likes' ? 'bg-crimson-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-crimson-500' : 'text-crimson-500 text-shadow-crimson'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'likes'">
          Likes
        </div>
        <div :class="recipeSort === 'saved' ? 'bg-blue-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-blue-500' : 'text-blue-500 text-shadow-blue'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'saved'">
          Saved
        </div>
      </div>
      <RecipeBoxMini v-for="r of currentSortRecipes" :key="r.id" :recipe="r" my-3 />
    </div>
    <TheNavbar />
  </ion-page>
</template>
