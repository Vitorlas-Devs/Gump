<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import MainContent from '@/components/MainContent.vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useUIStore } from '@/stores/uiStore'
import { useRequestErrorStore, type IRequestError } from '@/stores/requestErrorStore'
import { useRouter } from 'vue-router'
import { computed, onBeforeMount, ref, watch } from 'vue'
import { onBeforeRouteLeave } from 'vue-router'
import { storeToRefs } from 'pinia'

const translate = useTranslationStore()
const re = useRequestErrorStore()
const requestErrors = ref(re.$state)
const ui = useUIStore()
const router = useRouter()

const { loadTranslations } = translate
const { dirty } = storeToRefs(translate)

re.resetErrors()

const fetchTranslations = () => {
  if (dirty.value) {
    const answer = window.confirm(
      'You have unsaved changes, are you sure you want to fetch new translations?'
    )
    if (answer) {
      ;(async () => {
        await loadTranslations()
        re.resetErrors()
      })()
    }
  } else {
    ;(async () => {
      await loadTranslations()
      re.resetErrors()
    })()
  }
}

onBeforeRouteLeave((to, from, next) => {
  if (translate.dirty && to.name !== 'translate-home') {
    const answer = window.confirm('You have unsaved changes, are you sure you want to leave?')
    if (answer) {
      next()
    } else {
      next(false)
    }
  } else {
    next()
  }
})

const selectedKey = computed(() => router.currentRoute.value.params.key.toString())

onBeforeMount(() => {
  if (!translate.keys.includes(selectedKey.value)) {
    router.push({ name: 'translate', params: { key: translate.keys[0] } })
  }
})

const navigateKey = (key: string, direction: 'next' | 'previous') => {
  const index = translate.keys.indexOf(key)
  if (direction === 'next') {
    if (index === translate.keys.length - 1) {
      // if last key reached, go to first key
      router.push({ name: 'translate', params: { key: translate.keys[0] } })
    } else {
      router.push({ name: 'translate', params: { key: translate.keys[index + 1] } })
    }
  } else {
    if (index === 0) {
      // if first key reached, go to last key
      router.push({ name: 'translate', params: { key: translate.keys[translate.keys.length - 1] } })
    } else {
      router.push({ name: 'translate', params: { key: translate.keys[index - 1] } })
    }
  }
}

const litUp = ref(false)

const lightUpLights = () => {
  litUp.value = true
  setTimeout(() => {
    litUp.value = false
  }, 3000)
}

watch(
  litUp,
  () => {
    Object.keys(requestErrors.value).forEach((key) => {
      const el = document.getElementById(key)
      if (el) {
        if (litUp.value) {
          el.classList.add('bg-green', 'shadow-green')
          el.classList.remove('bg-grey-700', 'shadow-grey')
        } else {
          el.classList.remove('bg-green', 'shadow-green')
          el.classList.add('bg-grey-700', 'shadow-grey')
        }
      }
    })
  },
  { immediate: true }
)

const lightClasses = (requestError: IRequestError) => {
  const bg =
    requestError.status === null
      ? 'bg-grey-700 shadow-grey'
      : requestError.error
      ? 'bg-red shadow-red'
      : 'bg-green shadow-green'
  const text =
    requestError.status === null
      ? 'text-transparent'
      : requestError.error
      ? 'text-red'
      : 'text-green'
  return { bg, text }
}
</script>

<template>
  <main flex="~ col md:row" w="full" h="full">
    <TheNavigation v-if="ui.navbarOpen" z="10" />
    <div flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
      <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="40">
        <div flex="~ col" justify="between">
          <h1 text="xl md:3xl" font="bold">
            {{
              $route.params.key
                .toString()
                .replace(/([A-Z])/g, ' $1')
                .trim()
                .replace(/^./, (str) => str.toUpperCase())
            }}
          </h1>
          <div flex="~ col" gap="4" place="items-end">
            <div flex="~ row" place="items-center">
              <p font="bold" text="lg md:2xl orange-500" class="text-shadow-orange">
                Fetch your data:
              </p>
              <svg-icon
                icon="rotate-left-solid"
                class="icon-orange"
                w="8 md:12"
                mx="5"
                cursor="pointer"
                @click="fetchTranslations"
              />
            </div>
            <div
              flex="~ row-reverse md:col"
              gap="5 md:10"
              pos="relative md:absolute"
              top="1 md:30"
              right="5"
            >
              <div
                flex="~ row"
                gap="5"
                px="5"
                py="2"
                rounded="full"
                bg="orange-500"
                shadow="orange"
              >
                <SvgIcon
                  icon="chevron-left-solid"
                  class="icon-white"
                  w="6"
                  cursor="pointer"
                  @click="navigateKey(selectedKey, 'previous')"
                />
                <SvgIcon
                  icon="chevron-right-solid"
                  class="icon-white"
                  w="6"
                  cursor="pointer"
                  @click="navigateKey(selectedKey, 'next')"
                />
              </div>

              <div flex="~ row-reverse md:col" gap="5" ml="2">
                <div v-for="requestError in requestErrors" :key="requestError.id">
                  <div flex="~ col md:row" gap="2" place="items-center">
                    <div
                      :id="requestError.id"
                      rounded="full"
                      w="5 md:6"
                      h="5 md:6"
                      text="lg"
                      :class="lightClasses(requestError).bg"
                    />
                    <p font="bold" :class="lightClasses(requestError).text">
                      {{ requestError.status ?? 100 }}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <MainContent @update="lightUpLights" />
        </div>
      </custom-scrollbar>
    </div>
  </main>
</template>

<style scoped></style>
