<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed, watch } from 'vue'

const router = useRouter()

const translate = useTranslationStore()

const keys = computed(() => translate.keys)

if (router) {
  if (router.currentRoute.value.fullPath.includes('/translate/')) {
    watch(
      () => router.currentRoute.value.params.key.toString(),
      (key) => {
        if (!translate.checkKey(key)) {
          router.push({ name: 'not-found' })
        }
      }
    )
  }
}

</script>

<template>
  <div class="flex flex-col w-auto h-screen bg-gray-900">
    <custom-scrollbar :style="{ width: '250px', height: '100vh' }" :autoExpand="false">
      <ul class="flex flex-col w-full h-full mb-12">
        <li
          v-for="key in keys"
          :key="key"
          :class="{ 'bg-gray-700': $route.params.key === key }"
          class="flex flex-row items-center w-full h-10 px-4 border border-gray-800"
        >
          <RouterLink :to="{ name: 'translate', params: { key } }">
            {{ key }}
          </RouterLink>
        </li>

        <li
          v-for="i in 20"
          :key="i"
          class="flex flex-row items-center w-full h-10 px-4 border border-gray-800"
        >
          <RouterLink :to="{ name: 'translate', params: { key: i } }"> empty {{ i }} </RouterLink>
        </li>
      </ul>
    </custom-scrollbar>
  </div>
</template>

<style scoped></style>
