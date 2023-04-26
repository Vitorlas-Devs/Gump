<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

const ui = useUIStore()

function addItem() {
  ui.addEmptyIngredient()
}
</script>

<template>
  <ion-page bg-crimson-50>
    <CreateHeader />
    <CreateSubHeader variant="ingredients" />
    <div h-50vh grow overflow-y-auto>
      <IngredientsView :is-edting="true" />
    </div>
    <MainButton color="crimson" :title="$t('CreateItemButton')" @click="addItem" />
    <TheNavbar />
  </ion-page>
</template>
