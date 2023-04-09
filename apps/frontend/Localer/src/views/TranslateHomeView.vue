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
import SvgIcon from '@/components/SvgIcon.vue'

const translate = useTranslationStore()
const user = useUserStore()
const ui = useUIStore()
const { loadTranslations } = translate
const { token } = storeToRefs(user)
const router = useRouter()

const code = router.currentRoute.value.query.code

if (!token.value) {
  axios
    .get(import.meta.env.VITE_API_URL + 'access_token/', {
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
      <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="40">
        <div flex="~ col" justify="between" gap="6">
          <div>
            <h1 text="3xl orange-500 shadow-orange" font="bold">Translate Home</h1>
            <span flex="~ row" gap="2" place="items-center" mt="1">
              <SvgIcon icon="clock-regular" class="icon" w="4" />
              Reading time: 2 min
            </span>
          </div>
          <div mb="4">
            <h3 text="xl" font="bold">Prerequisites</h3>
            <ol my="2" pl="4" list="inside disc">
              <li>
                Have a <span font="bold">GitHub</span> account and authorize the Localer app (if you
                are reading this, you have already done this).
              </li>
              <li>
                You also need to be added to the
                <a
                  href="https://github.com/14A-A-Lyedlik-Devs/Gump"
                  target="_blank"
                  class="link-orange"
                  >Gump repository</a
                >.
              </li>
            </ol>
          </div>
          <div>
            <h3 text="xl" font="bold">Choose your language</h3>
            <p>Select your languages below. You can also add a new language.</p>
          </div>
          <vue-select
            v-model="user.languages"
            w="72"
            shadow="inner"
            rounded="xl"
            p="2"
            mb="4"
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
          <div>
            <span font="bold">Note:</span> The locale must be in the format of
            <span text="orange-500" font="bold">xx_XX</span>
            <ol my="2" pl="4" list="inside disc">
              <li><span text="orange-500" font="bold">xx</span> is the language code</li>
              <li><span text="orange-500" font="bold">XX</span> is the country code</li>
            </ol>
            For example, en_US for English (United States).
          </div>
          <div flex="~ row" place="items-center" gap="3">
            <SvgIcon icon="clipboard-regular" class="icon-orange" w="6" />
            <span>
              See
              <a
                href="https://www.fincher.org/Utilities/CountryLanguageList.shtml#:~:text=Country%20Code%20Language%20List%20%20%20%20Country,%20%204096%20%2052%20more%20rows%20"
                target="_blank"
                class="link-orange"
                >this site</a
              >
              for the available locales.
            </span>
          </div>

          <div flex="~ row" gap="2" mb="4">
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
          <div>
            <h3 text="xl" font="bold">Layout and workflow</h3>
            <ol class="cool-ol">
              <li>
                The <span font="bold">Navigation bar</span> shows the
                <span text="orange-500" font="bold">keys</span> and your progress. The first
                (EnglishLanguageName) will be used in the language selection menu in the app.
              </li>
              <li>
                Clicking on a key will show its translation page and all the languages, but only the
                languages you selected will be editable.
              </li>
              <li>
                Having made changes, click on the
                <span font="bold" text="crimson-500">Save</span> button and it will create a pull
                request, that we will review and merge.
              </li>
            </ol>
          </div>
          <div>
            <h3 text="xl" font="bold">Special Values</h3>
            Include these strings in your translations the same way that they are in the English
            text.
            <ol class="cool-ol">
              <li>
                <span font="bold">Referencing another key: </span>
                <span text="orange-500" font="bold">@:AnotherKey</span> <br />
                The referenced key's value will be inserted.
              </li>
              <li>
                <span font="bold">Static values: </span>
                <span font="bold" text="crimson-500">{username}</span> <br />
                This will be replaced with the actual value. Other examples: {email}, {password}.
              </li>
            </ol>
          </div>
          <div>
            <h3 text="xl" font="bold">Translation Tips</h3>
            <ol class="cool-ol">
              <li>
                <span text="xl">🤖</span>
                <span font="bold"> Use tools: </span>
                Definitely use apps like
                <a href="https://translate.google.com/" target="_blank" class="link-orange"
                  >Google Translate</a
                >,
                <a href="https://chat.openai.com/chat" target="_blank" class="link-orange"
                  >ChatGPT</a
                >,
                <a href="https://www.bing.com/new" target="_blank" class="link-orange"
                  >Bing Chat (Creative mode)</a
                >.
              </li>
              <li>
                <span text="xl">🪜</span>
                <span font="bold"> Keep the length: </span>
                Try not to exceed the length of the English text too much. This will help us to make
                the UI responsive easier.
              </li>
              <li>
                <span text="xl">📝</span>
                <span font="bold"> Review: </span>
                To ensure that the created pull request includes all of your changes, please
                double-check it on GitHub. For a quick link to it, click on your profile in the top
                right corner.
              </li>
            </ol>
          </div>
          <div>
            <h3 text="xl" font="bold">🚧 Beta version Warning 🚨</h3>
            <ol class="cool-ol">
              <li>
                After hitting <span font="bold" text="crimson-500">Save</span>, 3 lights will signal
                whether the translations were saved successfully on GitHub. <br />
                <span font="bold"> If any of them is red</span>, please notify us
                <span font="bold">with the status codes</span> (numbers next to the lights). You can
                also hit <span font="bold">Ctrl + Shift + J</span> and send the console output.
              </li>
              <li>
                Be gentle with the <span font="bold" text="crimson-500">Save</span> button. Use it
                less often, but often enough that you don't accidentally lose your work, as
                refreshing the page will do just that.
              </li>
              <li>
                The site loads the translations from the <span font="bold">main</span> branch. If
                you have an open pull request that hasn't been merged yet, you will have to use the
                <span font="bold" text="orange-500">Fetch your data</span> button. Your profile
                shows whether you are up to date or not.
              </li>
              <li>
                The <span font="bold">Navigation bar resets</span> its scroll position when
                switching keys. The <span text="orange-500" font="bold">Next key</span> button can
                help with that.
              </li>
              <li>
                <span font="bold">You should not Log out</span>. Your GitHub access token is stored,
                as well as your selected languages.
              </li>
            </ol>
          </div>
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
