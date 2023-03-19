<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'

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

// set text area height to fit content
const textArea = document.getElementById('translationArea')

const resize = () => {
  if (!textArea) return
  textArea.style.height = 'auto'
  textArea.style.height = textArea.scrollHeight + 'px'
}

</script>

<template>
  <div class="w-4/5">
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
        class="rounded flex-grow p-3 shadow-inner bg-crimson-50 rounded-3xl min-h-12 h-max"
        :readonly="locale === 'en_US'"
        @change="checkDirty()"
        @input="resize"
      />
      <div
        :class="{
          'bg-crimson-500':
            translations[locale][selectedKey] !== initialTranslations[locale][selectedKey],
          'bg-crimson-50':
            translations[locale][selectedKey] === initialTranslations[locale][selectedKey]
        }"
        class="w-2 h-8 rounded-full drop-shadow-md"
      ></div>
    </div>
  </div>
</template>

<style scoped></style>
