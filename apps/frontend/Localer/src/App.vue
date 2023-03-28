<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'
import { CreateBranch, createFilesAndCommit, createPullRequest, getBranch } from './octokit'
import { useUserStore } from '@/stores/userStore'
import { storeToRefs } from 'pinia'
import router from './router'

const translate = useTranslationStore()
const user = useUserStore()
const { token } = storeToRefs(user)
const { username } = storeToRefs(user)

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  ;(async () => {
    const { locales, translations, initialTranslations } = translate

    const changedLocales = locales.filter((locale) => {
      return JSON.stringify(translations[locale]) !== JSON.stringify(initialTranslations[locale])
    })

    const filenames = changedLocales

    const contents = changedLocales.map((locale) => {
      return JSON.stringify(translations[locale], null, 4)
    })

    const { sha } = await getBranch()
    await CreateBranch(username.value, sha)
    await createFilesAndCommit(username.value, filenames, contents)
    await createPullRequest(username.value)
  })()

  translate.saveChanges()
}

const authenticate = () => {
  if (!token.value) {
    const clientId = import.meta.env.VITE_CLIENT_ID

    const authUrl = `https://github.com/login/oauth/authorize?client_id=${clientId}`

    window.location.href = authUrl
  } else {
    router.push('/translate')
  }
}

// watch the user store and update the profile name and avatar
</script>

<template>
  <div>
    <div class="flex flex-row justify-between h-12 place-items-center text-lg font-bold">
      <div class="flex flex-row gap-4 mx-5">
        <RouterLink to="/">Home</RouterLink>
        <p class="cursor-pointer" @click="authenticate">Translate</p>
      </div>
      <div v-if="user.username" class="flex flex-row gap-4 mx-5 place-items-center">
        <p>
          {{ user.username }}
        </p>
        <img :src="user.avatarUrl" class="rounded-full w-10 h-10" />
      </div>
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
