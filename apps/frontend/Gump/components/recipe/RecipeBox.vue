<script setup lang="ts">
const props = defineProps<{
  title: string
  authorId: number
  imgId: number
  views: number
  likes: number
  saves: number
  id: number
}>()

const image = useImageStore()
const localePath = useLocalePath()
const ui = useUIStore()
const user = useUserStore()

const isLiked = ref(false)
const isSaved = ref(false)
const authorName = ref('')

async function viewRecipe(recipeId: number) {
  ui.activeNav = 'Recipes'
  await navigateTo(localePath(`/recipe/${recipeId}`))
}

onMounted(async () => {
  authorName.value = await user.getAuthorNameById(props.authorId)
  isLiked.value = user.likes.includes(props.id)
  isSaved.value = user.recipes.includes(props.id)
})
</script>

<template>
  <div flex="~ col" shadow-orangeBox m-4 h-262px rounded-2xl bg-orange-50>
    <img :src="image.getImage(imgId)" h="367/593" w-full cursor-pointer self-center rounded-t-2xl object-cover @click="viewRecipe(id)">
    <div flex="~ col" shadow-orangeImage px-1 text-center text-shadow>
      <div cursor-pointer @click="viewRecipe(id)">
        <p my-1 truncate text-xl>
          {{ title }}
        </p>
        <p my-1 truncate text-lg>
          {{ authorName }}
        </p>
      </div>
      <RecipeFooter :id="id" :view-count="views" :like-count="likes" :save-count="saves" :is-liked="isLiked" :is-saved="isSaved" @like="isLiked = !isLiked" @save="isSaved = !isSaved" />
    </div>
  </div>
</template>

<style scoped></style>
