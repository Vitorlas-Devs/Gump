<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-4">
      <h1>Translate</h1>
      <h3 class="text-gray-400">Choose your language.</h3>
      <p class="text-gray-400">And select a key from the left sidebar.</p>
    </div>
  </main>
</template>

<style scoped></style>
