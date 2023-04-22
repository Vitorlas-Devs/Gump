<script setup lang="ts">
import { useRecipeStore } from '@/stores/recipeStore'
import RecipeItem from '@/components/moderation/RecipeItem.vue'
import { onMounted } from 'vue'

const recipe = useRecipeStore()

onMounted(async () => {
  await recipe.fetchRecipes()
})
</script>

<template>
  <main flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
    <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="25">
      <h1 text="3xl" mb="4" font="bold">Recipes</h1>
      <div flex="~ wrap">
        <RecipeItem v-for="r of recipe.recipes" :key="r.id" :recipe="r" mb="4" mr="4" />
      </div>
    </custom-scrollbar>
  </main>
</template>
