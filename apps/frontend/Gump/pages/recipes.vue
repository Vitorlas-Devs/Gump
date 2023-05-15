<script setup lang="ts">
const recipeSort = ref('owned' as 'owned' | 'liked' | 'saved')
const recipe = useRecipeStore()
const user = useUserStore()

const userRecipes = ref([] as Recipe[])

watch(
  () => [recipeSort.value, user.current.recipes, user.current.likes],
  async () =>
    userRecipes.value = await recipe.getUserRecipes(recipeSort.value),
  { immediate: true, deep: true },
)
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('RecipesNav')" />
    <div grow overflow-y-auto>
      <div flex="~ row" my-10 items-center justify-around>
        <div :class="recipeSort === 'owned' ? 'bg-orange-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-orange-500' : 'text-orange-500 text-shadow-orange'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'owned'">
          Owned
        </div>
        <div :class="recipeSort === 'liked' ? 'bg-crimson-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-crimson-500' : 'text-crimson-500 text-shadow-crimson'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'liked'">
          Liked
        </div>
        <div :class="recipeSort === 'saved' ? 'bg-blue-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-blue-500' : 'text-blue-500 text-shadow-blue'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'saved'">
          Saved
        </div>
      </div>
      <RecipeBoxMini v-for="r in userRecipes" :key="r.id" :recipe="r" my-3 />
    </div>
    <TheNavbar />
  </ion-page>
</template>
