<script setup lang="ts">
defineProps<{
  color: string
  text: string
}>()

const emit = defineEmits<{
  (e: 'click'): void
}>()
</script>

<template>
  <button :bg="color" text="orange-50" p="2" px="4" rounded="8px" @click="emit('click')">
    {{ text }}
  </button>
</template>
