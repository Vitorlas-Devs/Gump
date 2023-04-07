<script setup lang="ts">
import { onMounted, computed, watch } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useUserStore } from '@/stores/userStore'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'

const translate = useTranslationStore()
const user = useUserStore()
const router = useRouter()

const { checkDirty } = translate
const { translations, locales, initialTranslations } = storeToRefs(translate)
const selectedKey = computed(() => router.currentRoute.value.params.key.toString())
const { languages } = storeToRefs(user)

window.onbeforeunload = () => {
  if (translate.dirty) {
    return 'You have unsaved changes, are you sure you want to leave?'
  }
}

const resize = (event: Event) => {
  if (!event.target) {
    return
  }
  const target = event.target as HTMLTextAreaElement
  target.style.height = 'auto'
  target.style.height = target.scrollHeight + 'px'
}

watch(
  () => translate.$state.translations,
  () => {
    locales.value.forEach((locale) => {
      resize({ target: document.getElementById(locale) } as Event)
    })
  },
  { deep: true }
)

onMounted(() => {
  if (!translate.checkKey(selectedKey.value)) {
    router.push({ name: 'not-found' })
  }
  locales.value.forEach((locale) => {
    resize({ target: document.getElementById(locale) } as Event)
  })
})

const changesClasses = (locale: string, key: string) =>
  computed(() => {
    const classes: any = {}
    if (!initialTranslations.value[locale]) {
      classes['bg-crimson-500'] = true
      return classes
    }
    if (translations.value[locale][key] !== initialTranslations.value[locale][key]) {
      classes['bg-crimson-500'] = true
    } else if (translations.value[locale][key] === initialTranslations.value[locale][key]) {
      classes['bg-crimson-50'] = true
    }
    return classes
  })
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
        overflow="hidden"
        :readonly="!languages.includes(locale)"
        @input="checkDirty"
      />
      <div :class="changesClasses(locale, selectedKey).value" w="2" h="8" rounded="full"></div>
    </div>
  </div>
</template>

<style scoped></style>
