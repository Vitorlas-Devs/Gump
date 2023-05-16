<script setup lang="ts">
defineProps<{
  badge: Badge
}>()

const image = useImageStore()
</script>

<template>
  <div flex="~ col">
    <div flex="~ row" items-center>
      <img :src="image.getImageUrl(badge.image)" h-20 w-20>
      <div ml-5 text-xl font-bold>
        {{ $t(badge.name) }}
      </div>
    </div>
    <p>
      {{ $t(badge.description) }}
    </p>
  </div>
</template>
