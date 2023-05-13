<script setup lang="ts">
import type { DefineComponent } from 'nuxt/dist/app/compat/vue-demi'

// import { emptyRecipe } from '~/stores/recipe'

const recipe = useRecipeStore()
const category = useCategoryStore()
const ui = useUIStore()

// onMounted, get all categories
onMounted(async () => {
  await category.getAll()
})

type CreateTabType = Record<CreateTab, DefineComponent<{}, {}, any>>

const createTabData: CreateTabType = {
  Info: defineAsyncComponent(() => import('~/components/create/CreateInfoView.vue')),
  Ingredients: defineAsyncComponent(() => import('~/components/recipeview/IngredientsView.vue')),
  Steps: defineAsyncComponent(() => import('~/components/recipeview/StepsView.vue')),
  Details: defineAsyncComponent(() => import('~/components/create/CreateDetailsView.vue')),
}

function addItem() {
  if (ui.activeCreateTab === 'Ingredients')
    recipe.addEmptyIngredient()
  else if (ui.activeCreateTab === 'Steps')
    recipe.addEmptyStep()
}
</script>

<template>
  <ion-page bg-crimson-50>
    <CreateHeader />
    <CreateSubHeader
      v-if="ui.activeCreateTab === 'Ingredients' || ui.activeCreateTab === 'Steps'"
      :variant="ui.activeCreateTab === 'Ingredients' ? 'ingredients' : 'steps'"
    />
    <div h-50vh grow overflow-y-auto pb-60 pt-5>
      <component :is="createTabData[ui.activeCreateTab]" :is-editing="true" />
    </div>
    <MainButton
      v-if="ui.activeCreateTab === 'Ingredients' || ui.activeCreateTab === 'Steps'"
      fixed color="crimson" :title="$t('CreateItemButton')" @click="addItem"
    />
    <MainButton
      v-else-if="ui.activeCreateTab === 'Details'"
      fixed color="orange" :title="$t('CreateRecipeButton')" @click="recipe.createRecipe()"
    />
    <TheNavbar />
  </ion-page>
</template>
