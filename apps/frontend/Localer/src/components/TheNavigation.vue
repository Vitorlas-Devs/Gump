<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'

const translate = useTranslationStore()

const keys = computed(() => translate.keys)
</script>

<template>
  <div class="flex flex-col w-auto h-screen shadow-inner">
    <custom-scrollbar :style="{ width: '250px', height: '100vh' }" :auto-expand="false">
      <ul class="flex flex-col w-full h-full mb-12">
        <li
          v-for="key in keys"
          :key="key"
          :class="{ 'font-bold': $route.params.key === key }"
          class="flex flex-row items-center w-full h-10 px-4"
        >
          <RouterLink :to="{ name: 'translate', params: { key } }">
            {{ key }}
          </RouterLink>
        </li>

        <li
          v-for="i in 20"
          :key="i"
          class="flex flex-row items-center w-full h-10 px-4"
        >
          <RouterLink :to="{ name: 'translate', params: { key: i } }"> empty {{ i }} </RouterLink>
        </li>
      </ul>
    </custom-scrollbar>
  </div>
</template>

<style scoped></style>
