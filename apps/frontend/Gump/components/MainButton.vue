<script setup lang="ts">
defineProps<{
  title?: string
  iconType?: 'create' | 'delete'
  color: 'orange' | 'crimson'
}>()
</script>

<template>
  <div>
    <div flex="~ row" h-max w-max items-center gap-2 rounded-full p-2 :shadow="`${color}`" :class="`${color}Gradient`">
      <div
        v-if="iconType === 'create'"
        class="i-fa6-solid-plus whiteIcon"
      />
      <div
        v-else-if="iconType === 'delete'"
        class="i-fa6-solid-trash-can whiteIcon"
        mx="2"
      />
      <h1 v-if="title" m-a text-xl font-bold text-white-500 text-shadow-white>
        {{ title }}
      </h1>
    </div>
  </div>
</template>

<style scoped>

</style>
