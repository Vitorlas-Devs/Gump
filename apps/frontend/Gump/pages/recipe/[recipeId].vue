<script setup lang="ts">
import type { DefineComponent } from 'nuxt/dist/app/compat/vue-demi'

const ui = useUIStore()
const recipe = useRecipeStore()
const user = useUserStore()
const image = useImageStore()

const id = ui.params.recipe
ui.activeNav = 'Recipes'

const currentRecipe = await recipe.getRecipeById(id)

const authorName = await user.getAuthorNameById(currentRecipe?.author as number)

type RecipeTabType = Record<RecipeTab, { component: DefineComponent<{}, {}, any>; translation: string }>

const recipeTabData: RecipeTabType = {
  Info: {
    component: defineAsyncComponent(() => import('~/components/recipeview/InfoView.vue')),
    translation: 'RecipeViewInfo',
  },
  Ingredients: {
    component: defineAsyncComponent(() => import('~/components/recipeview/IngredientsView.vue')),
    translation: 'CreateIngredients',
  },
  Steps: {
    component: defineAsyncComponent(() => import('~/components/recipeview/StepsView.vue')),
    translation: 'CreateSteps',
  },
}

function handleLiked() {
  if (currentRecipe) {
    currentRecipe.isLiked = !currentRecipe.isLiked
    currentRecipe.likeCount += currentRecipe.isLiked ? 1 : -1
    // recipe.$patch({
    //   recipes: recipe.recipes.map(r => r.id === id ? currentRecipe : r),
    // })
  }
}

function handleSaved() {
  if (currentRecipe) {
    currentRecipe.isSaved = !currentRecipe.isSaved
    currentRecipe.saveCount += currentRecipe.isSaved ? 1 : -1
    // recipe.$patch({
    //   recipes: recipe.recipes.map(r => r.id === id ? currentRecipe : r),
    // })
  }
}
</script>

<template>
  <ion-page v-if="currentRecipe" bg-crimson-50>
    <TheHeader title-color="brown" :subtitle="authorName" :title="currentRecipe.title" />
    <img :src="image.getImage(currentRecipe.image)" h-40 w-full object-cover>
    <RecipeFooter :recipe="currentRecipe" @like="handleLiked" @save="handleSaved" />
    <div
      flex="~ row" items-center justify-center space-x-4
    >
      <div
        v-for="(tab, key, index) in recipeTabData"
        :key="index"
        cursor-pointer text-center text-2xl font-bold
        :class="ui.activeRecipeTab === key ? 'text-crimson-500' : 'text-brown-500'"
        @click="ui.activeRecipeTab = key"
      >
        {{ $t(tab.translation) }}
      </div>
    </div>
    <div grow overflow-y-auto>
      <component :is="recipeTabData[ui.activeRecipeTab].component" :is-editing="false" :current-recipe="currentRecipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
