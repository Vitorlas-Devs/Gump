<script setup lang="ts">
defineProps<{
  text: string
  type?: 'email' | 'password'
}>()

defineEmits<{
  (event: 'update:text', ...args: any[]): void
}>()
</script>

<template>
  <input
    :value="text"
    :type="type || 'text'"
    h-min-12
    border-0 rounded-full bg-transparent p-3 shadow-inner
    @input="$emit('update:text', ($event.target as HTMLInputElement).value)"
  >
</template>
