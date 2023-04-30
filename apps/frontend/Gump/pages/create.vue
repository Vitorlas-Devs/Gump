<script setup lang="ts">
const recipe = useRecipeStore()

function addItem() {
  // recipe.addEmptyIngredient()
  recipe.addEmptyStep()
}
</script>

<template>
  <ion-page bg-crimson-50>
    <CreateHeader />
    <CreateSubHeader variant="steps" />
    <div h-50vh grow overflow-y-auto>
      <StepsView :is-edting="true" />
    </div>
    <MainButton fixed color="crimson" :title="$t('CreateItemButton')" @click="addItem" />
    <TheNavbar />
  </ion-page>
</template>
