<script setup lang="ts">
defineProps<{
  title: string
  authorId: number
  imgId: number
}>()

const image = useImageStore()
const user = useUserStore()
const ui = useUIStore()
const recipe = useRecipeStore()

function addRecipe(recipeId: number) {
  recipe.addRecipe(recipeId)
}
</script>

<template>
  <div flex="~" shadow-orangebox m-4 max-h-35 w-auto rounded-2xl bg-orange-50>
    <img :src="image.getImage(imgId)" w="1/3" rounded-s-2xl object-cover>
    <div grow self-center text-shadow>
      <p m-3 text-xl>
        {{ title }}
      </p>
      <p m-3 text-lg>
        {{ user.username }}
      </p>
    </div>
    <img v-if="ui.activeNav === 'Create'" src="~assets/PlusSign.svg" m-3 @click="addRecipe(1)">
  </div>
</template>

<style scoped></style>
