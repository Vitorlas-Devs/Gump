<script setup lang="ts">
const props = defineProps<{
  recipe: SearchRecipe | Recipe
}>()

const image = useImageStore()
const ui = useUIStore()
const recipeStore = useRecipeStore()
const user = useUserStore()
const localePath = useLocalePath()

const authorName = ref('')

function addRecipe(recipe: Recipe) {
  recipeStore.addRecipe(recipe)
}

async function viewRecipe(recipeId: number) {
  // check if the current user is the owner of the recipe
  if (user.current.id === props.recipe.author) {
    ui.activeNav = 'Create'
    ui.createHeaderStates = [true, true, true, true]
    ui.createIsEditing = true
    if ('ingredients' in props.recipe)
      recipeStore.currentRecipe = props.recipe
    else
      recipeStore.currentRecipe = await recipeStore.getRecipeById(props.recipe.id)

    await navigateTo(localePath('/create'))
  } else {
    ui.activeNav = 'Recipes'
    ui.setParams('recipe', recipeId)
    await navigateTo(localePath(`/recipe/${recipeId}`))
  }
}

onMounted(async () => {
  authorName.value = await user.getAuthorById(props.recipe.author) ?? ''
})
</script>

<template>
  <div flex="~ row" shadow-orangeBox max-h-25 w-full cursor-pointer justify-between rounded-2xl bg-orange-50>
    <div w-full flex="~ row" @click="viewRecipe(recipe.id)">
      <img :src="image.getImageUrl(recipe.image)" w="1/3" rounded-s-2xl object-cover>
      <div grow self-center text-shadow>
        <p m-3 text-xl>
          {{ recipe.title }}
        </p>
        <p m-3 text-lg>
          {{ authorName }}
        </p>
      </div>
    </div>
    <img v-if="ui.activeNav === 'Create'" src="~assets/PlusSign.svg" m-3 @click="addRecipe(recipe as Recipe)">
  </div>
</template>
