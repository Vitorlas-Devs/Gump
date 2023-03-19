<script setup lang="ts">
import { onMounted, computed, ref, type Ref } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useTextareaAutosize } from '@vueuse/core'

const router = useRouter()

const translate = useTranslationStore()

const { locales, checkDirty, initialTranslations } = translate
const selectedKey = computed(() => router.currentRoute.value.params.key.toString())
const { translations } = storeToRefs(translate)

onMounted(() => {
  if (!translate.checkKey(selectedKey.value)) {
    router.push({ name: 'not-found' })
  }
})

const textareas: { el: Ref<HTMLTextAreaElement>; value: string; locale: string }[] = []

console.log('textareas', textareas)
console.log('translations', translations.value)

locales.forEach((locale) => {
  textareas.push({
    el: useTextareaAutosize().textarea,
    value: translations.value[locale][selectedKey.value],
    locale: locale
  })
})

// const setValues = () => {
//   textareas.forEach((el) => {
//     el.value = translations.value[el.locale][selectedKey.value]
//   })
// }
</script>

<template>
  <div class="w-4/5">
    <textarea
      v-for="textarea in textareas"
      :key="textarea.locale"
      v-model="textarea.value"
      class="flex flex-row gap-4 my-6 place-items-center"
    />
    <div
      v-for="locale in locales"
      :key="locale"
      class="flex flex-row gap-4 my-6 place-items-center"
    >
      <label class="w-16 text-xl"> {{ locale }}</label>
      <textarea
        id="translationArea"
        v-model="translations[locale][selectedKey]"
        type="text"
        class="rounded flex-grow p-3 shadow-inner bg-crimson-50 rounded-3xl min-h-12 h-max resize-none"
        :readonly="locale === 'en_US'"
        @change="checkDirty()"
      />
      <div
        :class="{
          'bg-crimson-500':
            translations[locale][selectedKey] !== initialTranslations[locale][selectedKey],
          'bg-crimson-50':
            translations[locale][selectedKey] === initialTranslations[locale][selectedKey]
        }"
        class="w-2 h-2 rounded-full drop-shadow-md"
      ></div>
    </div>
  </div>
</template>

<style scoped></style>
