<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import MainContent from '@/components/MainContent.vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useUIStore } from '@/stores/uiStore'
import { useRequestErrorStore } from '@/stores/requestErrorStore'
import { useRouter } from 'vue-router'
import { computed, ref } from 'vue'
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

const nextKey = (key: string) => {
  const index = translate.keys.indexOf(key)
  if (index === translate.keys.length - 1) {
    router.push({ name: 'translate', params: { key: translate.keys[0] } })
  } else {
    router.push({ name: 'translate', params: { key: translate.keys[index + 1] } })
  }
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
              gap="10"
              pos="relative md:absolute"
              top="0 md:30"
              right="5"
            >
              <button
                w="26"
                h="12"
                rounded="full"
                bg="orange-500"
                shadow="orange"
                text="crimson-50 shadow-white"
                font="bold"
                @click="nextKey(selectedKey)"
              >
                Next key
              </button>
              <div flex="~ row-reverse md:col" gap="5" ml="2">
                <div v-for="requestError in requestErrors" :key="requestError.status ?? 0">
                  <div flex="~ col md:row" gap="2" place="items-center">
                    <div
                      rounded="full"
                      w="5 md:6"
                      h="5 md:6"
                      :bg="
                        requestError.status === null
                          ? 'grey-700'
                          : requestError.error
                          ? 'red'
                          : 'green'
                      "
                      :shadow="
                        requestError.status === null ? 'grey' : requestError.error ? 'red' : 'green'
                      "
                    />
                    <p
                      font="bold"
                      :text="
                        requestError.status === null
                          ? 'transparent lg'
                          : requestError.error
                          ? 'red lg'
                          : 'green lg'
                      "
                    >
                      {{ requestError.status ?? 100 }}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <MainContent />
        </div>
      </custom-scrollbar>
    </div>
  </main>
</template>

<style scoped></style>
