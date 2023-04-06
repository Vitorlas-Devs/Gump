<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'
import SvgIcon from './components/SvgIcon.vue'
import { CreateBranch, createFileAndCommit, createPullRequest, getBranch } from './octokit'
import { useUserStore } from '@/stores/userStore'
import { useUIStore } from '@/stores/uiStore'
import { storeToRefs } from 'pinia'
import router from './router'

const translate = useTranslationStore()
const user = useUserStore()
const ui = useUIStore()
const { loadTranslations } = useTranslationStore()
const { token } = storeToRefs(user)
const { username, languages, loggedIn } = storeToRefs(user)

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  ;(async () => {
    const { translations, initialTranslations } = translate

    const changedLocales = languages.value.filter((locale) => {
      return JSON.stringify(translations[locale]) !== JSON.stringify(initialTranslations[locale])
    })

    const contents = changedLocales.map((locale) => {
      const filteredTranslations = Object.keys(translations[locale])
        .filter((key) => translations[locale][key] !== '')
        .reduce((obj: any, key) => {
          obj[key] = translations[locale][key]
          return obj
        }, {})
      return JSON.stringify(filteredTranslations, null, 4)
    })

    const { sha } = await getBranch()
    await CreateBranch(username.value, sha)

    contents.forEach(async (content, index) => {
      await createFileAndCommit(username.value, changedLocales[index], content)
    })

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
    ;(async () => {
    await loadTranslations()
  })()
  }
  loggedIn.value = true
}
</script>

<template>
  <div>
    <div flex="~ row" justify="between" h="12" place="items-center" text="lg" font="bold">
      <div flex="~ row" gap="3 md:4" mx="3 md:5">
        <svg-icon
          class="icon-orange"
          icon="bars-solid"
          w="8"
          h="8"
          mr="2"
          cursor="pointer"
          @click="ui.toggleNavbar"
        />
        <RouterLink to="/">Home</RouterLink>
        <p cursor="pointer" @click="authenticate">Translate</p>
      </div>
      <div v-if="user.username" flex="~ row" gap="4" mx="5" place="items-center">
        <p>
          {{ user.username }}
        </p>
        <img :src="user.avatarUrl" rounded="full" w="10" h="10" />
      </div>
    </div>
    <RouterView :key="$route.fullPath" />
  </div>
  <Transition name="bounce">
    <div v-if="dirty" fixed="~" bottom="0" left="0" right="0" mx="auto" w="max" z="50">
      <div
        flex="~ col md:row"
        gap="2 md:4"
        m="5"
        place="items-center"
        bg="crimson-50"
        shadow="crimsonDown"
        rounded="xl"
        px="4"
        py="2"
      >
        <p font="bold">You have unsaved changes!</p>
        <span class="hidden md:block">Click here when you're done.</span>
        <div flex="~ row" gap="8 md:4" place="items-center">
          <button
            transition="~"
            text="crimson-500 shadow-crimson"
            font="bold"
            rounded="lg"
            @click="translate.resetChanges"
          >
            Cancel
          </button>
          <button
            bg="crimson-500 hover:bg-crimson-600"
            transition="~"
            text="crimson-50 shadow-white"
            font="bold"
            shadow="crimsonDown"
            py="2"
            px="4"
            rounded="lg"
            @click="saveChanges"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  </Transition>
</template>

<style scoped>
.bounce-enter-active {
  animation: bounce-in 0.4s;
}
.bounce-leave-active {
  animation: bounce-in 0.4s reverse;
}
@keyframes bounce-in {
  0% {
    transform: scale(0);
  }
  50% {
    transform: scale(1.25);
  }
  100% {
    transform: scale(1);
  }
}
</style>
