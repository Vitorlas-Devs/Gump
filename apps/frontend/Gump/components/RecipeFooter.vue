<script setup lang="ts">
defineProps<{
  viewCount: number
  likeCount: number
  saveCount: number
  isLiked: boolean
  isSaved: boolean
}>()

defineEmits<{
  (event: 'like'): void
  (event: 'save'): void
}>()

const likeButton = ref<HTMLElement>()
const saveButton = ref<HTMLElement>()
</script>

<template>
  <div flex="~ row" justify-between font-mono text-2xl>
    <div flex="~ row" items-center>
      <div h-8 w-8 class="i-fa6-solid-eye orangeIcon" />
      <div ml-1 text-orange-500>
        {{ formatNumber(viewCount) }}
      </div>
    </div>
    <div ref="likeButton" flex="~ row" cursor-pointer items-center @click="$emit('like')">
      <div h-8 w-8 class="crimsonIcon" :class="isLiked ? 'i-ph-heart-fill' : 'i-ph-heart-bold'" />
      <div ml-1 text-crimson-500>
        {{ formatNumber(likeCount) }}
      </div>
    </div>
    <div ref="saveButton" flex="~ row" cursor-pointer items-center @click="$emit('save')">
      <div h-8 w-8 class="blueIcon" :class="isSaved ? 'i-ph-bookmark-simple-fill' : 'i-ph-bookmark-simple-bold'" />
      <div ml-1 text-blue-500>
        {{ formatNumber(saveCount) }}
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
