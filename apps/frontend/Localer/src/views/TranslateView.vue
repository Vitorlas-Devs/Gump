<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import MainContent from '@/components/MainContent.vue'
import { getContent } from '@/octokit'
import { useTranslationStore } from '@/stores/translationStore'

const translate = useTranslationStore()
const { locales, initialTranslations } = translate

const refetchTranslations = () => {
  // foreach locales, call getContent
  // ;(async () => {
  //   const username = "Rettend"
  //   locales.forEach(async (locale) => {
  //     const content = await getContent(username, locale)
  //     translate.initialTranslations[locale] = JSON.parse(content)
  //   })
  // })()
  console.log('refetchTranslations')
}
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-6 gap-4">
      <div class="justify-between flex flex-row">
        <h1 class="text-2xl">
          {{
            $route.params.key
              .toString()
              .replace(/([A-Z])/g, ' $1')
              .trim()
              .replace(/^./, (str) => str.toUpperCase())
          }}
        </h1>
        <font-awesome-icon
          icon="fa-solid fa-rotate"
          class="fa-orange text-4xl mx-5 cursor-pointer text-orange-500 shadow-orangeFA"
          @click="refetchTranslations"
        />
      </div>
      <MainContent />
    </div>
  </main>
</template>

<style scoped></style>
