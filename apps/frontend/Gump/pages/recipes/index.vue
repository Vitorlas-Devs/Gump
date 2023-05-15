<script setup lang="ts">
const recipesSort = ref('Owned' as RecipesSort)
const recipe = useRecipeStore()
const user = useUserStore()

const userRecipes = ref([] as Recipe[])

watch(
  () => [recipesSort.value, user.current.recipes, user.current.likes],
  async () =>
    userRecipes.value = await recipe.getUserRecipes(recipesSort.value),
  { immediate: true, deep: true },
)
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('RecipesNav')" />
    <div grow overflow-y-auto>
      <div flex="~ row" my-10 items-center justify-around>
        <div :class="recipesSort === 'Owned' ? 'bg-orange-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-orange-500' : 'text-orange-500 text-shadow-orange'" cursor-pointer px-4 py-1 text-6 @click="recipesSort = 'Owned'">
          {{ $t('RecipesSortOwned') }}
        </div>
        <div :class="recipesSort === 'Liked' ? 'bg-crimson-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-crimson-500' : 'text-crimson-500 text-shadow-crimson'" cursor-pointer px-4 py-1 text-6 @click="recipesSort = 'Liked'">
          {{ $t('RecipesSortLiked') }}
        </div>
        <div :class="recipesSort === 'Saved' ? 'bg-blue-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-blue-500' : 'text-blue-500 text-shadow-blue'" cursor-pointer px-4 py-1 text-6 @click="recipesSort = 'Saved'">
          {{ $t('RecipesSortSaved') }}
        </div>
      </div>
      <RecipeBoxMini v-for="r in userRecipes" :key="r.id" :recipe="r" my-3 />
    </div>
    <TheNavbar />
  </ion-page>
</template>
