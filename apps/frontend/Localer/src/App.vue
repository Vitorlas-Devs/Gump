<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed, onBeforeMount, ref } from 'vue'
import { CreateBranch, createOrUpdateFiles, createPullRequest, getBranch } from './octokit'
import { useUserStore } from '@/stores/userStore'
import { useUIStore } from '@/stores/uiStore'
import { useRequestErrorStore } from '@/stores/requestErrorStore'
import { storeToRefs } from 'pinia'
import { useRouter } from 'vue-router'

const translate = useTranslationStore()
const user = useUserStore()
const ui = useUIStore()
const router = useRouter()
const re = useRequestErrorStore()
const { loadTranslations } = useTranslationStore()
const { username, languages, loggedIn, token } = storeToRefs(user)

onBeforeMount(() => {
  ;(async () => {
    await loadTranslations()
  })()
})

if (window.innerWidth < 768) {
  ui.navbarOpen = false
}

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  re.resetErrors()
  ;(async () => {
    const { translations, initialTranslations } = translate

    const locales = [...languages.value, 'notes']

    const changedLocales = locales.filter((locale) => {
      return JSON.stringify(translations[locale]) !== JSON.stringify(initialTranslations[locale])
    })

    const contents = changedLocales.map((locale) => {
      const filteredTranslations = Object.keys(translations[locale])
        .filter((key) => translations[locale][key] !== '')
        .reduce((obj: any, key) => {
          obj[key] = translations[locale][key]
          return obj
        }, {})
      return JSON.stringify(filteredTranslations, null, 2)
    })

    contents.forEach((content, index) => {
      contents[index] = content.replace(/&nbsp;/g, ' ')
    })

    const { sha } = await getBranch()
    await CreateBranch(username.value, sha)

    await createOrUpdateFiles(username.value, changedLocales, contents)

    const { prUrl, prNumber } = await createPullRequest(username.value)

    if (prUrl && prNumber) {
      user.openPullRequest = true
      user.prUrl = prUrl
      user.prNumber = prNumber
    }
  })()

  translate.saveChanges()
}

const resetChanges = () => {
  const locales = [...languages.value, 'notes']
  locales.forEach((language) => {
    if (Object.values(translate.translations[language]).every((value) => value === '')) {
      languages.value = locales.filter((lang) => lang !== language)
    }
  })
  window.location.reload()
}

const authenticate = () => {
  if (!token.value) {
    const clientId = import.meta.env.VITE_CLIENT_ID

    const authUrl = `https://github.com/login/oauth/authorize?client_id=${clientId}`

    window.location.href = authUrl
  } else {
    router.push('/translate')
  }
  loggedIn.value = true
}

const openModal = ref(false)
</script>

<template>
  <div>
    <div flex="~ row" justify="between" h="12" place="items-center" text="lg" font="bold">
      <div flex="~ row" gap="3 md:4" mx="3 md:5" place="items-center">
        <SvgIcon
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
        <a
          href="https://github.com/14A-A-Lyedlik-Devs/Gump"
          place="self-center"
          target="_blank"
          rel="noopener noreferrer"
          flex="~ row"
          gap="2"
        >
          <span class="hidden md:inline-block">Gump repo</span>
          <SvgIcon icon="up-right-from-square-solid" class="icon" w="4" />
        </a>
      </div>
      <div
        v-if="user.username"
        flex="~ row"
        gap="4"
        mx="5"
        place="items-center"
        cursor="pointer"
        @click="openModal = !openModal"
      >
        <p>
          {{ user.username }}
        </p>
        <img :src="user.avatarUrl" rounded="full" w="10" h="10" />
      </div>
    </div>
    <div>
      <div
        v-if="openModal"
        pos="absolute"
        flex="~ col"
        gap="4"
        top="12"
        z="30"
        right="5"
        bg="crimson-50"
        rounded="xl"
        p="5"
        shadow="orange"
        font="bold"
      >
        Pull request:
        <span
          flex="~ row"
          place="self-center"
          :text="user.openPullRequest ? 'green lg' : 'purple lg'"
          gap="2"
          ><SvgIcon
            icon="code-pull-request-solid"
            w="6"
            :class="user.openPullRequest ? 'icon-green' : 'icon-purple'"
          />{{ user.openPullRequest ? 'Open' : 'Closed' }}</span
        >
        <a
          v-if="user.prNumber && user.prUrl"
          class="text-center link-orange"
          :href="user.prUrl"
          target="_blank"
        >
          Link: {{ user.prNumber }}
        </a>
        <p v-else text="center">Up to date</p>
        <div mt="3" text="center" cursor="pointer" @click="user.logout">Log out</div>
      </div>
    </div>
    <div flex="~ col md:row" w="full" h="full">
      <TheNavigation
        v-if="
          ui.navbarOpen &&
          (router.currentRoute.value.path === '/translate' ||
            router.currentRoute.value.path.includes('/translate/'))
        "
        z="10"
      />
      <RouterView :key="$route.fullPath" />
    </div>
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
            @click="resetChanges"
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
