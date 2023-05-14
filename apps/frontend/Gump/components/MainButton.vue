<script setup lang="ts">
defineProps<{
  title?: string
  iconType?: 'create' | 'delete'
  color: 'orange' | 'crimson'
}>()

defineEmits<{
  (event: 'click'): void
}>()
</script>

<template>
  <div
    left="1/2" transform="translate-x--1/2" flex="~ row" :shadow="`${color}`" :class="`${color}Gradient`" bottom-26 z-20 h-max w-max cursor-pointer items-center gap-2 rounded-full p-2 px-4
    @click="$emit('click')"
  >
    <div
      v-if="iconType === 'create'"
      class="i-fa6-solid-plus whiteIcon"
    />
    <div
      v-else-if="iconType === 'delete'"
      class="i-fa6-solid-trash-can whiteIcon"
    />
    <h1 v-if="title" m-a text-xl font-bold text-white-500 text-shadow-white>
      {{ title }}
    </h1>
  </div>
</template>
