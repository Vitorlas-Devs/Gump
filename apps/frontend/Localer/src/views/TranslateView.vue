<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import MainContent from '@/components/MainContent.vue'
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-6 gap-4">
      <h1 class="text-2xl">{{ $route.params.key.toString().replace(/([A-Z])/g, ' $1').trim().replace(/^./, (str) => str.toUpperCase()) }}</h1>
      <MainContent />
    </div>
  </main>
</template>

<style scoped></style>
