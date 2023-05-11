<script setup lang="ts">
defineProps<{
  text: string
  type?: 'email' | 'password' | 'number'
}>()

defineEmits<{
  (event: 'update:text', ...args: any[]): void
}>()
</script>

<template>
  <input
    :value="text"
    :type="type || 'text'"
    :w="type === 'number' ? '20' : 'full'"
    border-0 rounded-full bg-transparent p-2 px-3 shadow-inner
    @input="$emit('update:text', ($event.target as HTMLInputElement).value)"
  >
</template>

<style scoped>

</style>
