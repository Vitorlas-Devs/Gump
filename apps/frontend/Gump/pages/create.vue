<script setup lang="ts">
import type { DefineComponent } from 'nuxt/dist/app/compat/vue-demi'

// import { emptyRecipe } from '~/stores/recipe'

const recipe = useRecipeStore()
const category = useCategoryStore()

// onMounted, get all categories
onMounted(async () => {
  await category.getAll()
})

type CreateTabType = Record<CreateTab, DefineComponent<{}, {}, any>>

const createTabData: CreateTabType = {
  Info: defineAsyncComponent(() => import('~/components/recipeview/CreateInfoView.vue')),
  Ingredients: defineAsyncComponent(() => import('~/components/recipeview/IngredientsView.vue')),
  Steps: defineAsyncComponent(() => import('~/components/recipeview/StepsView.vue')),
  Details: defineAsyncComponent(() => import('~/components/recipeview/CreateDetailsView.vue')),
}

function addItem() {
  recipe.addEmptyIngredient()
  recipe.addEmptyStep()
  // recipe.recipes.push(emptyRecipe)
}
</script>

<template>
  <ion-page bg-crimson-50>
    <CreateHeader />
    <!-- <CreateSubHeader variant="ingredients" /> -->
    <div h-50vh grow overflow-y-auto pb-60 pt-5>
      <CreateDetailsView :current-recipe="recipe.currentRecipe" />
    </div>
    <!-- <MainButton fixed color="crimson" :title="$t('CreateItemButton')" @click="addItem" /> -->
    <MainButton fixed color="orange" :title="$t('CreateRecipeButton')" @click="addItem" />
    <TheNavbar />
  </ion-page>
</template>
