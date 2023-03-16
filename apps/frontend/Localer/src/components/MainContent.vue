<script setup lang="ts">
import { watch, reactive, onMounted, computed } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useRouter } from 'vue-router'

const router = useRouter()

const translate = useTranslationStore()

const { locales, translationsForKey, translations } = translate
const selectedKey = computed(() => router.currentRoute.value.params.key.toString())

const state = reactive({
  translations: {} as Record<string, Record<string, string>>
})

state.translations = {
  [selectedKey.value]: translationsForKey(selectedKey.value)
}

watch(
  () => selectedKey,
  (key) => {
    state.translations = {
      [key.value]: translationsForKey(key.value)
    }
  }
)

onMounted(() => {
  if (!translate.checkKey(selectedKey.value)) {
    router.push({ name: 'not-found' })
  }
})
</script>

<template>
  <div class="w-3/4">
    <div v-for="locale in locales" :key="locale" class="flex flex-row gap-4">
      <label class="w-12"> {{ locale }}</label>
      <input
        type="text"
        v-model="translations[locale][selectedKey]"
        class="bg-gray-800 border rounded flex-grow"
        :readonly="locale === 'en_US'"
      />
    </div>
  </div>
</template>

<style scoped></style>
