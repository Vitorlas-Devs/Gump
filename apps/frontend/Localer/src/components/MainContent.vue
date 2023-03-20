<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'

const translate = useTranslationStore()
const router = useRouter()

const { locales, checkDirty, initialTranslations } = translate
const selectedKey = computed(() => router.currentRoute.value.params.key.toString())
const { translations } = storeToRefs(translate)

const resize = (event: Event) => {
  const target = event.target as HTMLTextAreaElement
  target.style.height = 'auto'
  target.style.height = target.scrollHeight + 'px'
}

onMounted(() => {
  if (!translate.checkKey(selectedKey.value)) {
    router.push({ name: 'not-found' })
  }
  locales.forEach((locale) => {
    resize({ target: document.getElementById(locale) } as Event)
  })
})

const inputFuncs = (e: Event) => {
  checkDirty()
  resize(e)
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
        :id="locale"
        v-model="translations[locale][selectedKey]"
        type="text"
        class="rounded flex-grow p-3 shadow-inner bg-crimson-50 rounded-3xl min-h-12 h-max"
        :readonly="locale === 'en_US'"
        @input="inputFuncs($event)"
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
