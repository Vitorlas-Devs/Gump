<script setup lang="ts">
import type { DefineComponent } from 'nuxt/dist/app/compat/vue-demi'

const ui = useUIStore()
const recipe = useRecipeStore()
const user = useUserStore()
const image = useImageStore()
const localePath = useLocalePath()

const id = ui.params.recipe
ui.activeNav = 'Recipes'

const currentRecipe = await recipe.getRecipeById(id)

const authorName = await user.getAuthorById(currentRecipe?.author as number)

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
    recipe.$patch({
      searchRecipes: recipe.searchRecipes.map(r => r.id === id ? currentRecipe : r),
    })
  }
}

function handleSaved() {
  if (currentRecipe) {
    currentRecipe.isSaved = !currentRecipe.isSaved
    currentRecipe.saveCount += currentRecipe.isSaved ? 1 : -1
    recipe.$patch({
      searchRecipes: recipe.searchRecipes.map(r => r.id === id ? currentRecipe : r),
    })
  }
}

async function addRecipe() {
  if (recipe.currentRecipe && currentRecipe) {
    recipe.currentRecipe.ingredients.push({
      name: currentRecipe.title,
      value: 1,
      volume: '',
      linkedRecipe: currentRecipe.id,
    })
    ui.activeNav = 'Create'
    ui.activeRecipeTab = 'Ingredients'
    await navigateTo(localePath('/create'))
  }
}
</script>

<template>
  <ion-page v-if="currentRecipe" bg-crimson-50>
    <TheHeader title-color="brown" :subtitle="authorName" :title="currentRecipe.title" />
    <img :src="image.getImageUrl(currentRecipe.image)" h-40 w-full object-cover>
    <RecipeFooter :recipe="currentRecipe" @like="handleLiked" @save="handleSaved" />
    <div m-2 grow overflow-y-auto>
      <div
        flex="~ row" absolute left-2 right-2 z-5 m-a items-center justify-center bg-crimson-50
      >
        <div
          v-for="(tab, key, index) in recipeTabData"
          :key="index"
          flex-1 cursor-pointer px-5 py-1 text-center text-xl font-bold
          :class="ui.activeRecipeTab === key
            ? index === 0
              ? 'shadow-leftActive'
              : index === Object.keys(recipeTabData).length - 1
                ? 'shadow-rightActive'
                : 'shadow-midActive'
            : 'shadow-inactive'"

          :rounded="ui.activeRecipeTab === key
            ? 't-5' : index === 0
              ? ui.activeRecipeTab === Object.keys(recipeTabData)[1]
                ? 'br-5'
                : ''
              : index === Object.keys(recipeTabData).length - 1
                ? ui.activeRecipeTab === Object.keys(recipeTabData)[index - 1]
                  ? 'bl-5'
                  : ''
                : ui.activeRecipeTab === Object.keys(recipeTabData)[index - 1]
                  ? 'bl-5'
                  : ui.activeRecipeTab === Object.keys(recipeTabData)[index + 1]
                    ? 'br-5'
                    : ''"
          @click="ui.activeRecipeTab = key"
        >
          {{ $t(tab.translation) }}
        </div>
      </div>
      <component :is="recipeTabData[ui.activeRecipeTab].component" :is-editing="false" mb-10 pt-15 :current-recipe="currentRecipe" />
    </div>
    <MainButton fixed color="orange" icon-type="create" :title="$t('RecipeViewUseRecipe')" @click="addRecipe" />
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
