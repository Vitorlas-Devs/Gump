<script setup lang="ts">
const props = defineProps<{
  title?: string
  iconType?: 'create' | 'delete'
  color: 'orangeGradient' | 'crimsonGradient' | 'blueGradient'
}>()

defineEmits<{
  (event: 'click'): void
}>()

const shadowColor = computed(() => {
  switch (props.color) {
    case 'orangeGradient':
      return 'orange'
    case 'crimsonGradient':
      return 'crimson'
    case 'blueGradient':
      return 'blue'
  }
})
</script>

<template>
  <div
    left="1/2" transform="translate-x--1/2" flex="~ row" :shadow="shadowColor"
    :class="color" bottom-26 z-20 h-max w-max cursor-pointer items-center gap-2 rounded-full p-2 px-4
    @click="$emit('click')"
  >
    <div
      v-if="iconType === 'create'"
      whiteIcon i-shadow:fa6-solid-plus
    />
    <div
      v-else-if="iconType === 'delete'"
      whiteIcon i-shadow:fa6-solid-trash-can
    />
    <h1 v-if="title" m-a text-xl font-bold text-white-500 text-shadow-white>
      {{ title }}
    </h1>
  </div>
</template>
