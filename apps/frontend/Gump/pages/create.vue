<script setup lang="ts">
// import { emptyRecipe } from '~/stores/recipe'

const recipe = useRecipeStore()

function addItem() {
  recipe.addEmptyIngredient()
  recipe.addEmptyStep()
  // recipe.recipes.push(emptyRecipe)
}
</script>

<template>
  <ion-page bg-crimson-50>
    <CreateHeader />
    <CreateSubHeader variant="ingredients" />
    <div h-50vh grow overflow-y-auto>
      <IngredientsView :is-edting="true" />
    </div>
    <MainButton fixed color="crimson" :title="$t('CreateItemButton')" @click="addItem" />
    <TheNavbar />
  </ion-page>
</template>
