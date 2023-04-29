<script setup lang="ts">
const recipe = useRecipeStore()

function addItem() {
  recipe.addEmptyIngredient()
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
