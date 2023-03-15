<script setup lang="ts">
import { ref, watch } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'

const { locales, translationsForKey, translations } = useTranslationStore()
const selectedKey = ref('')

watch(selectedKey, (key) => {
  translations.value = translationsForKey(key)
})
</script>

<template>
  <div class="w-3/4">
    <h3>{{ selectedKey }}</h3>

    <div class="flex flex-row gap-4">
      <label class="w-12">en_US</label>
      <input
        type="text"
        v-model="translations.en_US"
        class="bg-gray-800 border rounded flex-grow"
        readonly
      />
    </div>

    <div
      v-for="locale in locales.filter((locale) => locale !== 'en_US')"
      :key="locale"
      class="flex flex-row gap-4"
    >
      <label class="w-12"> {{ locale }}</label>
      <input
        type="text"
        v-model="translations[locale]"
        class="bg-gray-800 border rounded flex-grow"
      />
    </div>
  </div>
</template>

<style scoped></style>
