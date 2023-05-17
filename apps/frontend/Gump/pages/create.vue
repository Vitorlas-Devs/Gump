<script setup lang="ts">
import type { DefineComponent } from 'nuxt/dist/app/compat/vue-demi'
import { emptyRecipe } from '~/stores/recipe'

const recipe = useRecipeStore()
const category = useCategoryStore()
const ui = useUIStore()
const localePath = useLocalePath()

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

if (!recipe.currentRecipe)
  recipe.currentRecipe = emptyRecipe

const isSuccessful = ref(false)

async function createRecipe() {
  const id = await recipe.createRecipe()
  if (id) {
    isSuccessful.value = true
    setTimeout(async () => {
      isSuccessful.value = false
      ui.activeCreateTab = 'Info'
      ui.createIsEditing = false
      await navigateTo(localePath('/home'))
    }, 2000)
  }
}

onBeforeRouteLeave((to, from, next) => {
  if (ui.createIsEditing) {
    if (recipe.currentRecipe) {
      recipe.currentRecipe = emptyRecipe
      ui.createHeaderStates = [false, false, false, false]
      ui.createHeaderIndex = 0
      ui.createIsEditing = false
    }
  }
  next()
})
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
      fixed color="crimsonGradient" :title="$t('CreateItemButton')" @click="addItem"
    />
    <MainButton
      v-else-if="ui.activeCreateTab === 'Details' && !ui.createIsEditing"
      fixed color="orangeGradient" :title="$t('CreateRecipeButton')" @click="createRecipe()"
    />
    <TheNavbar />
    <Transition>
      <div v-if="isSuccessful" flex="~ row" absolute z-100 h-full w-full items-center justify-center bg-crimson-50>
        <div
          i-shadow:fa6-solid-circle-check
          h-20 w-20 from-crimson-500 to-orange-500 bg-gradient-to-rt
        />
      </div>
    </Transition>
  </ion-page>
</template>
