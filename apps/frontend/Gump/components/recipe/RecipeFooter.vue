<script setup lang="ts">
const props = defineProps<{
  recipe: SearchRecipe | Recipe
}>()

const emit = defineEmits<{
  (event: 'like'): void
  (event: 'save'): void
}>()

const recipe = useRecipeStore()
const user = useUserStore()

const isLiking = ref(false)
const isSaving = ref(false)

async function likeClick() {
  await recipe.likeRecipe(props.recipe.id)
  const recipeToModify = recipe.recipes.find(r => r.id === props.recipe.id) ?? recipe.recipes.find(r => r.id === props.recipe.id)

  if (recipeToModify) {
    if (props.recipe.isLiked)
      user.current.likes = user.current.likes.filter(l => l !== props.recipe.id)
    else
      user.current.likes.push(props.recipe.id)
  }

  isLiking.value = true
  setTimeout(() => {
    isLiking.value = false
  }, 820)
  emit('like')
}

async function saveClick() {
  await recipe.saveRecipe(props.recipe.id)
  const recipeToModify = recipe.recipes.find(r => r.id === props.recipe.id) ?? recipe.recipes.find(r => r.id === props.recipe.id)

  if (recipeToModify) {
    if (props.recipe.isSaved)
      user.current.recipes = user.current.recipes.filter(r => r !== props.recipe.id)
    else
      user.current.recipes.push(props.recipe.id)
  }

  isSaving.value = true
  setTimeout(() => {
    isSaving.value = false
  }, 820)
  emit('save')
}
</script>

<template>
  <div v-if="props.recipe" flex="~ row" mx-2 justify-between text-left font-mono text-xl>
    <div flex="~ row" items-center>
      <div class="i-fa6-solid-eye orangeIcon" />
      <div ml-1 text-orange-500 text-shadow-orange>
        {{ formatNumber(props.recipe.viewCount) }}
      </div>
    </div>
    <div id="likeButton" :class="{ heartbeat: isLiking }" flex="~ row" cursor-pointer items-center @click="likeClick">
      <div class="crimsonIcon" :class="props.recipe.isLiked ? 'i-ph-heart-fill' : 'i-ph-heart-bold'" />
      <div ml-1 text-crimson-500 text-shadow-crimson>
        {{ formatNumber(props.recipe.likeCount) }}
      </div>
    </div>
    <div id="saveButton" :class="{ heartbeat: isSaving }" flex="~ row" cursor-pointer items-center @click="saveClick">
      <div shadow-blue class="blueIcon" :class="props.recipe.isSaved ? 'i-ph-bookmark-simple-fill' : 'i-ph-bookmark-simple-bold'" />
      <div ml-1 text-blue-500 text-shadow-blue>
        {{ formatNumber(props.recipe.saveCount) }}
      </div>
    </div>
  </div>
</template>

<style scoped>
@keyframes heartbeat {
  0% {
    transform: scale(1);
  }

  14% {
    transform: scale(1.3);
  }

  28% {
    transform: scale(1);
  }

  42% {
    transform: scale(1.3);
  }

  70% {
    transform: scale(1);
  }
}

.heartbeat {
  animation: heartbeat 0.82s cubic-bezier(0.36, 0.07, 0.19, 0.97) both;
}
</style>
