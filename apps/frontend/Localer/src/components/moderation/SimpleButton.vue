<script setup lang="ts">
defineProps<{
  type: 'solid' | 'text'
  color: string
  text: string
}>()

const emit = defineEmits<{
  (e: 'click'): void
}>()
</script>

<template>
  <button
    :bg="type === 'solid' ? color : 'transparent'"
    :text="type === 'solid' ? 'white' : color"
    p="2"
    px="4"
    rounded="8px"
    @click="emit('click')"
  >
    {{ text }}
  </button>
</template>
