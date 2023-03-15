<script setup lang="ts">
import { useTranslationStore } from '@/stores/translationStore'

const translate = useTranslationStore()
const { keys, translationsForKey, locales } = translate
</script>

<template>
  <div>
    <div v-for="locale in locales" :key="locale">
      <h3>{{ locale }}</h3>
      <div v-for="key in keys" :key="key">
        <label>{{ key }}</label>
        <input type="text" v-model="translationsForKey(key)[locale]" />
      </div>
    </div>
  </div>
</template>
<style scoped></style>
