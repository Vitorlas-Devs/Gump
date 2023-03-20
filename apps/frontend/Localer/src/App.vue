<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'
import { createPullRequestFromContent } from './octokit'

const translate = useTranslationStore()

// const { locales } = translate
// const { translations } = storeToRefs(translate)

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  ;(async () => {
    const { locales, translations, initialTranslations } = translate

    // go through all locales and if there is a difference between the initial and current translation, put them in changedLocales
    const changedLocales = locales.filter((locale) => {
      return JSON.stringify(translations[locale]) !== JSON.stringify(initialTranslations[locale])
    })
    

    const username = 'Rettend'
    const filenames = changedLocales
    console.log('filenames', filenames)
    // const content = JSON.stringify(translations[filenames], null, 4)
    const content = changedLocales.map((locale) => {
      return JSON.stringify(translations[locale], null, 4)
    })
    console.log('content', content)
    await createPullRequestFromContent(username, filenames, content)
  })()

  translate.saveChanges()
}
</script>

<template>
  <div>
    <div class="flex flex-row gap-4 mx-5 my-2">
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/translate-home">Translate</RouterLink>
    </div>
    <RouterView :key="$route.fullPath" />
  </div>
  <div v-if="dirty" class="fixed bottom-0 mx-auto left-0 right-0 w-max">
    <div
      class="flex flex-row m-5 items-center gap-4 bg-crimson-50 text-crimson-500 shadow-crimsonDown px-4 py-2 rounded-xl relative"
    >
      <strong class="font-bold">You have unsaved changes!</strong>
      <span class="block sm:inline">Click here when you're done.</span>
      <button
        class="bg-crimson-500 hover:bg-crimson-600 transition text-crimson-50 font-bold shadow-crimsonDown py-2 px-4 ml-4 rounded-lg"
        @click="saveChanges"
      >
        Save
      </button>
    </div>
  </div>
</template>
