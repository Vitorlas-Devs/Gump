<script setup lang="ts">
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'
import { CreateBranch, createFilesAndCommit, createPullRequest, getBranch } from './octokit'

const router = useRouter()

const translate = useTranslationStore()

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  ;(async () => {
    const { locales, translations, initialTranslations } = translate

    const changedLocales = locales.filter((locale) => {
      return JSON.stringify(translations[locale]) !== JSON.stringify(initialTranslations[locale])
    })

    let username = 'Rettend'
    username = username.replace(/ /g, '-')

    const filenames = changedLocales

    const contents = changedLocales.map((locale) => {
      return JSON.stringify(translations[locale], null, 4)
    })

    const { sha } = await getBranch()
    await CreateBranch(username, sha)
    await createFilesAndCommit(username, filenames, contents)
    await createPullRequest(username)
  })()

  translate.saveChanges()
}

const authenticate = () => {
  const clientId = import.meta.env.VITE_CLIENT_ID

  const authUrl = `https://github.com/login/oauth/authorize?client_id=${clientId}`

  window.location.href = authUrl
}
</script>

<template>
  <div>
    <div class="flex flex-row gap-4 mx-5 my-2">
      <RouterLink to="/">Home</RouterLink>
      <!-- <RouterLink to="/translate">Translate</RouterLink> -->
      <!-- a button insead with even Authenticate -->
      <p class="cursor-pointer" @click="authenticate">Translate</p>
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
