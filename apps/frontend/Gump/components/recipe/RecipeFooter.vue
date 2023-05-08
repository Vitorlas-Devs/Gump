<script setup lang="ts">
const props = defineProps<{
  id: number
}>()

const emit = defineEmits<{
  (event: 'like'): void
  (event: 'save'): void
}>()

const recipe = useRecipeStore()
const user = useUserStore()

const currentRecipe = computed(() => recipe.recipes.find(r => r.id === props.id))

const isLiking = ref(false)
const isSaving = ref(false)

async function likeClick() {
  await recipe.likeRecipe(props.id)
  // const recipeToModify = recipe.recipes.find(r => r.id === props.id)

  // if (recipeToModify) {
  //   if (props.isLiked) {
  //     recipeToModify.likeCount--
  //     user.likes = user.likes.filter(l => l !== props.id)
  //   } else {
  //     recipeToModify.likeCount++
  //     user.likes.push(props.id)
  //   }
  // }

  isLiking.value = true
  setTimeout(() => {
    isLiking.value = false
  }, 820)
  emit('like')
}

async function saveClick() {
  await recipe.saveRecipe(props.id)
  const recipeToModify = recipe.recipes.find(r => r.id === props.id)

  if (recipeToModify) {
    if (currentRecipe.value?.isSaved) {
      recipeToModify.saveCount--
      user.recipes = user.recipes.filter(r => r !== props.id)
    } else {
      recipeToModify.saveCount++
      user.recipes.push(props.id)
    }
  }

  isSaving.value = true
  setTimeout(() => {
    isSaving.value = false
  }, 820)
  emit('save')
}
</script>

<template>
  <div v-if="currentRecipe" flex="~ row" mx-3 justify-between font-mono text-xl>
    <div flex="~ row" items-center>
      <div class="i-fa6-solid-eye orangeIcon" />
      <div ml-1 text-orange-500>
        {{ formatNumber(currentRecipe.viewCount) }}
      </div>
    </div>
    <div id="likeButton" :class="{ heartbeat: isLiking }" flex="~ row" cursor-pointer items-center @click="likeClick">
      <div class="crimsonIcon" :class="currentRecipe.isLiked ? 'i-ph-heart-fill' : 'i-ph-heart-bold'" />
      <div ml-1 text-crimson-500>
        {{ formatNumber(currentRecipe.likeCount) }}
      </div>
    </div>
    <div id="saveButton" :class="{ heartbeat: isSaving }" flex="~ row" cursor-pointer items-center @click="saveClick">
      <div shadow-blue class="blueIcon" :class="currentRecipe.isSaved ? 'i-ph-bookmark-simple-fill' : 'i-ph-bookmark-simple-bold'" />
      <div ml-1 text-blue-500>
        {{ formatNumber(currentRecipe.saveCount) }}
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
