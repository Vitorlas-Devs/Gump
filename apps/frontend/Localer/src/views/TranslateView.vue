<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import MainContent from '@/components/MainContent.vue'
import { useTranslationStore } from '@/stores/translationStore'

const translate = useTranslationStore()

const { loadTranslations } = translate

const fetchTranslations = () => {
  ;(async () => {
    await loadTranslations()
  })()
}
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-6 pl-10 gap-4">
      <div class="justify-between flex flex-row">
        <h1 class="text-3xl font-bold">
          {{
            $route.params.key
              .toString()
              .replace(/([A-Z])/g, ' $1')
              .trim()
              .replace(/^./, (str) => str.toUpperCase())
          }}
        </h1>
        <div class="flex flex-row">
          <p class="text-orange-500 font-bold text-2xl text-shadow-orange">Fetch your data -></p>
          <font-awesome-icon
            icon="fa-solid fa-rotate"
            class="fa-orange text-4xl mx-5 cursor-pointer text-orange-500"
            @click="fetchTranslations"
          />
        </div>
      </div>
      <MainContent />
    </div>
  </main>
</template>

<style scoped></style>
