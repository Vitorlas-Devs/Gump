<script setup lang="ts">
defineProps<{
  title: string
  authorId: number
  imgId: number
}>()

const image = useImageStore()
const user = useUserStore()

const isLiked = ref(false)
const isSaved = ref(false)
</script>

<template>
  <div flex="~ col" shadow-orangeBox m-4 h-262px rounded-2xl bg-orange-50>
    <img
      :src="image.getImage(imgId)"
      h="367/593" w-full self-center rounded-t-2xl object-cover
    >
    <div flex="~ col" shadow-orangeImage px-1 text-center text-shadow>
      <p my-1 truncate text-xl>
        {{ title }}
      </p>
      <p my-1 truncate text-lg>
        {{ user.username }}
      </p>
      <RecipeFooter
        :view-count="32500000" :like-count="1234" :save-count="123456789"
        :is-liked="isLiked" :is-saved="isSaved"
        @like="isLiked = !isLiked" @save="isSaved = !isSaved"
      />
    </div>
  </div>
</template>

<style scoped>

</style>
