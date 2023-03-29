<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import { getAuthenticatedUser } from '@/octokit'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { useUserStore } from '@/stores/userStore'
import { storeToRefs } from 'pinia'
import { useTranslationStore } from '@/stores/translationStore'
import { useUIStore } from '@/stores/uiStore'
import VueSelect from 'vue-select'

const translate = useTranslationStore()
const user = useUserStore()
const ui = useUIStore()
const { loadTranslations } = translate
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
      router.push({ name: 'translate-home' })
    })
    .catch((error) => {
      console.log(error)
    })
}
</script>

<template>
  <main flex="~ col md:row" w="full" h="full">
    <TheNavigation v-if="ui.navbarOpen" z="10" />
    <div flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" gap="6">
      <h1 text="3xl orange-500 shadow-orange" font="bold" my="2">Translate</h1>
      <div>
        <h3>Choose your language.</h3>
        <p>And select a key from the left sidebar.</p>
      </div>
      <vue-select
        v-model="user.languages"
        w="72"
        :options="translate.locales"
        :multiple="true"
        :close-on-select="false"
        :clear-on-select="false"
        :preserve-search="true"
        :value="user.languages"
        placeholder="Select your languages"
      />
    </div>
  </main>
</template>

<style scoped></style>
