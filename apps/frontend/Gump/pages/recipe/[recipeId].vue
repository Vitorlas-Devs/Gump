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

const isLiked = ref<boolean>(user.likes.includes(currentRecipe?.id as number))
const isSaved = ref<boolean>(user.recipes.includes(currentRecipe?.id as number))

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
</script>

<template>
  <ion-page v-if="currentRecipe" bg-crimson-50>
    <TheHeader title-color="brown" :subtitle="authorName" :title="currentRecipe.title" />
    <div grow>
      <img :src="image.getImage(currentRecipe.image)" h-40 w-full object-cover>
      <RecipeFooter :id="id" @like="isLiked = !isLiked" @save="isSaved = !isSaved" />
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
      <component :is="recipeTabData[ui.activeRecipeTab].component" :is-editing="false" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
