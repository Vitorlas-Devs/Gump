<script setup lang="ts">
defineProps<{ title: string }>()
</script>

<template>
  <div h-16 w-full flex="~ row" bg-crimson-50 p-2 px-3>
    <h1 text-2xl font-bold text-orange-500 text-shadow-orange>
      {{ title }}
    </h1>
  </div>
</template>

<style scoped>

</style>
