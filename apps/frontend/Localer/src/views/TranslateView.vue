<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import MainContent from '@/components/MainContent.vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useUIStore } from '@/stores/uiStore'

const translate = useTranslationStore()
const ui = useUIStore()

const { loadTranslations } = translate

const fetchTranslations = () => {
  ;(async () => {
    await loadTranslations()
  })()
}
</script>

<template>
  <main flex="~ col md:row" w="full" h="full">
    <TheNavigation v-if="ui.navbarOpen" z="10" />
    <div flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2">
      <div flex="~ row" justify="between">
        <h1 text="xl md:3xl" font="bold">
          {{
            $route.params.key
              .toString()
              .replace(/([A-Z])/g, ' $1')
              .trim()
              .replace(/^./, (str) => str.toUpperCase())
          }}
        </h1>
        <div flex="~ row" place="items-center">
          <p font="bold" text="lg md:2xl orange-500" class="text-shadow-orange">Fetch your data</p>
          <svg-icon
            icon="rotate-left-solid"
            class="icon-orange"
            w="8 md:12"
            mx="5"
            cursor="pointer"
            @click="fetchTranslations"
          />
        </div>
      </div>
      <MainContent />
    </div>
  </main>
</template>

<style scoped></style>
