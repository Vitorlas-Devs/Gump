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
import { ref } from 'vue'

const translate = useTranslationStore()
const user = useUserStore()
const ui = useUIStore()
const { loadTranslations } = translate
const { token } = storeToRefs(user)
const router = useRouter()

const code = router.currentRoute.value.query.code

if (!token.value) {
  axios
    .get('https://api.gump.live/github/access_token/', {
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

const isEditing = ref(false)
const newLocale = ref('')

const saveNewLocale = () => {
  if (
    newLocale.value === '' ||
    translate.locales.includes(newLocale.value) ||
    !newLocale.value.match(/^[a-z]{2}_[A-Z]{2}$/)
  ) {
    return
  }
  translate.addLanguage(newLocale.value)
  user.addLanguage(newLocale.value)
  isEditing.value = false
  translate.checkDirty()
}

const input = ref<HTMLInputElement | null>(null)

const toggleEditing = () => {
  isEditing.value = !isEditing.value
  if (isEditing.value) {
    setTimeout(() => {
      if (input.value) {
        input.value.focus()
      }
    }, 0)
  }
}
</script>

<template>
  <main flex="~ col md:row" w="full" h="full">
    <TheNavigation v-if="ui.navbarOpen" z="10" />
    <div flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
      <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="30">
        <div flex="~ col" justify="between" gap="6">
          <h1 text="3xl orange-500 shadow-orange" font="bold" my="2">Translate</h1>
          <div>
            <h3>Choose your language.</h3>
            <p>And select a key from the left sidebar.</p>
          </div>
          <vue-select
            v-model="user.languages"
            w="72"
            shadow="inner"
            rounded="xl"
            p="2"
            font="bold"
            class="select"
            :options="translate.locales"
            :multiple="true"
            :close-on-select="false"
            :clear-on-select="false"
            :preserve-search="true"
            :value="
              user.languages.filter((language) => translate.initialLocales.includes(language))
            "
            placeholder="Select your languages"
          />
          <div flex="~ row" gap="2">
            <input
              v-if="isEditing"
              ref="input"
              v-model="newLocale"
              type="text"
              placeholder="Hit ENTER to save"
              shadow="inner"
              rounded="xl"
              p="2"
              font="bold"
              w="min"
              bg="crimson-50"
              @keyup.enter="saveNewLocale"
            />
            <button
              v-if="!isEditing"
              shadow="orange"
              bg="orange-500"
              text="orange-50 shadow-white"
              rounded="xl"
              py="2"
              px="4"
              font="bold"
              w="max"
              @click="toggleEditing"
            >
              Add a new language
            </button>
          </div>
          <!-- space filler to test scrollbar -->
          <div v-for="i in 100" :key="i">
            <p>Test</p>
          </div>
          <h1 text="3xl orange-500 shadow-orange" font="bold" my="2">
            I'm still on the bottom!
          </h1>
        </div>
      </custom-scrollbar>
    </div>
  </main>
</template>

<style scoped>
.select {
  --vs-search-input-bg: rgb(243, 88, 39);
  --vs-controls-color: rgb(151, 39, 4);
  --vs-selected-bg: transparent;
  --vs-selected-color: rgb(151, 39, 4);
  --vs-selected-border-color: rgba(151, 39, 4, 0.5);
  --vs-border-width: 0px;
  --vs-border-radius: 20px;
  --vs-dropdown-option-bg: rgb(243, 88, 39);
  --vs-dropdown-option--active-bg: rgb(243, 88, 39);
}
</style>
