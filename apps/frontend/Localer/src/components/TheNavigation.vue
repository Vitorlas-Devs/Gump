<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'

const translate = useTranslationStore()
const { keys, setCurrentLocale } = translate

const setCurrentKey = (key: string) => {
  setCurrentLocale(key)
}
</script>

<template>
  <div class="flex flex-col w-64 h-screen bg-gray-900">
    <custom-scrollbar :style="{ width: '300px', height: '100vh' }" :autoExpand="false">
      <ul>
        <li v-for="key in keys" :key="key" @click="setCurrentKey(key)">
          <RouterLink :to="{ name: 'translate', params: { key } }">
            {{ key }}
          </RouterLink>
        </li>
      </ul>

      <RouterView />
    </custom-scrollbar>
  </div>
</template>
