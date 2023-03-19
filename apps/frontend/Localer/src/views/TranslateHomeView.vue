<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-6">
      <h1 class="text-3xl text-orange-500 text-shadow-orange my-2 font-bold">Translate</h1>
      <h3>Choose your language.</h3>
      <p>And select a key from the left sidebar.</p>
    </div>
  </main>
</template>

<style scoped></style>
