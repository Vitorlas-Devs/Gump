<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { useUIStore } from '@/stores/uiStore'
import { useUserStore } from '@/stores/userStore'
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import SvgIcon from './SvgIcon.vue'

const translate = useTranslationStore()
const ui = useUIStore()
const user = useUserStore()
const router = useRouter()

const keys = computed(() => translate.keys)
const { translations } = storeToRefs(translate)

const currentKey = router.currentRoute.value.params.key

const toggleNavbar = () => {
  if (window.innerWidth < 768) {
    ui.toggleNavbar()
  }
}

const liClasses = (key: string) =>
  computed(() => {
    const classes: any = {
      'font-bold': currentKey === key
    }
    user.languages.forEach((language) => {
      if (!translations.value[language]) {
        user.languages = user.languages.filter((lang) => lang !== language)
        return
      }
      if (translations.value[language][key] !== '') {
        classes['text-orange-500'] = true
      } else {
        classes['text-brown-500'] = true
      }
    })
    if (emptyValuesCount(key) === user.languages.length) {
      classes['bg-gradient-to-r from-orange-100 to-transparent'] = true
    }

    return classes
  })

const emptyValuesCount = (key: string) => {
  let count = 0
  user.languages.forEach((language) => {
    if (translations.value[language][key] !== '') {
      count++
    }
  })
  return count
}

const isEditing = ref(false)
const newKey = ref('')

const saveNewKey = () => {
  if (newKey.value === '' || keys.value.includes(newKey.value)) {
    return
  }
  translate.addKey(newKey.value)
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
  <div
    flex="~ col"
    shadow="inner"
    h="100vh"
    w="full md:w-72"
    bg="crimson-50"
    rounded="none md:tr-xl"
    class="<md:fixed md:block"
  >
    <custom-scrollbar :auto-expand="false" h="100vh" w="full md:w-72">
      <ul flex="~ col" w="full" h="full" mb="24">
        <li flex="~ row" w="full" h="10" my="2" px="4" cursor="pointer">
          <span
            v-if="!isEditing"
            text="orange-500 shadow-orange lg"
            font="bold"
            flex="~ row"
            items="center"
            mx="2"
            w="full"
            justify="between"
            @click="toggleEditing"
          >
            Add new key
            <SvgIcon icon="plus-solid" class="icon-orange" w="7" />
          </span>
          <div v-if="isEditing" flex="~ row" w="full" items="center" justify="between" gap="3">
            <input
              ref="input"
              v-model="newKey"
              type="text"
              border="1 solid orange-500"
              shadow="orange"
              rounded="md"
              bg="crimson-50"
              p="2"
              placeholder="Hit ENTER to save"
              @keyup.enter="saveNewKey"
            />
          </div>
        </li>
        <li
          v-for="key in keys"
          :key="key"
          ref="li"
          :class="liClasses(key).value"
          flex="~ row"
          items="center"
          w="full"
          h="10"
          :pl="currentKey === key ? '2' : '4'"
          pr="5"
        >
          <Transition name="fade">
            <SvgIcon
              v-if="currentKey === key"
              icon="caret-right-solid"
              class="icon-crimson"
              w="5"
              mr="2"
          /></Transition>
          <div flex="~ row" w="full" items="center" justify="between" gap="3">
            <RouterLink :to="{ name: 'translate', params: { key } }" w="full" @click="toggleNavbar">
              {{ key }}
            </RouterLink>
            <div
              text="sm"
              class="whitespace-nowrap"
              :class="
                emptyValuesCount(key) < user.languages.length ? 'text-brown-500' : 'text-orange-500'
              "
            >
              {{ emptyValuesCount(key) }} / {{ user.languages.length }}
            </div>
          </div>
        </li>
        <li v-for="i in 20" :key="i" flex="~ row" w="full" h="10" px="4" items="center">
          <RouterLink
            :to="{ name: 'translate', params: { key: i } }"
            w="full"
            @click="toggleNavbar"
          >
            empty {{ i }}
          </RouterLink>
        </li>
      </ul>
    </custom-scrollbar>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease, transform 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateX(-10px);
}
</style>
