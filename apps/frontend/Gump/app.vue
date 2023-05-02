<script setup lang="ts">
const user = useUserStore()
const recipe = useRecipeStore()

watch(recipe.recipes,
  async () => await user.getUserData(),
  { deep: true, immediate: true },
)
</script>

<template>
  <ion-app>
    <ion-router-outlet font-sans text-brown-500 :animated="false" />
  </ion-app>
</template>
