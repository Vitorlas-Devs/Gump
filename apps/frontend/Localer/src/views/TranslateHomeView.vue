<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import { getAuthenticatedUser } from '@/octokit'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { useUserStore } from '@/stores/userStore'
import { storeToRefs } from 'pinia'
import { useTranslationStore } from '@/stores/translationStore'

const translate = useTranslationStore()
const { loadTranslations } = translate
const user = useUserStore()
const { token } = storeToRefs(user)
const router = useRouter()

const code = router.currentRoute.value.query.code

if (!token.value) {
  axios
    .get('http://46.101.114.190:3000/access_token/', {
      params: {
        code: code
      }
    })
    .then((response) => {
      const accessToken = response.data.split('=')[1].split('&')[0]
      token.value = accessToken
      ;(async () => {
        const { name, avatar } = await getAuthenticatedUser()
        user.login(name, avatar)
        await loadTranslations()
      })()
      router.push('/translate')
    })
    .catch((error) => {
      console.log(error)
    })
}
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-6">
      <h1 class="text-3xl text-orange-500 text-shadow-orange my-2 font-bold">Translate</h1>
      <h3>Choose your language.</h3>
      <p>And select a key from the left sidebar.</p>
    </div>
  </main>
</template>

<style scoped></style>
