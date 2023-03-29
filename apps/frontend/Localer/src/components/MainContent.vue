<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'

const translate = useTranslationStore()
const router = useRouter()

const { checkDirty } = translate
const { translations, locales, initialTranslations } = storeToRefs(translate)
const selectedKey = computed(() => router.currentRoute.value.params.key.toString())

const resize = (event: Event) => {
  const target = event.target as HTMLTextAreaElement
  target.style.height = 'auto'
  target.style.height = target.scrollHeight + 'px'
}

onMounted(() => {
  if (!translate.checkKey(selectedKey.value)) {
    router.push({ name: 'not-found' })
  }
  locales.value.forEach((locale) => {
    resize({ target: document.getElementById(locale) } as Event)
  })
})

const inputFuncs = (e: Event) => {
  checkDirty()
  resize(e)
}
</script>

<template>
  <div w="full md:4/5">
    <div v-for="locale in locales" :key="locale" flex="~ row" gap="4" my="6" place="items-center">
      <label w="10 md:16" text="md md:xl" font="bold">{{ locale }}</label>
      <textarea
        :id="locale"
        v-model="translations[locale][selectedKey]"
        type="text"
        flex="grow"
        p="3"
        shadow="inner"
        bg="crimson-50"
        rounded="3xl"
        h="min-12"
        resize="none"
        @input="inputFuncs($event)"
      />
      <div
        :class="{
          'bg-crimson-500':
            translations[locale][selectedKey] !== initialTranslations[locale][selectedKey],
          'bg-crimson-50':
            translations[locale][selectedKey] === initialTranslations[locale][selectedKey]
        }"
        w="2"
        h="8"
        rounded="full"
      ></div>
    </div>
  </div>
</template>

<style scoped></style>
