<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { useUIStore } from '@/stores/uiStore';
import { computed } from 'vue'

const translate = useTranslationStore()
const ui = useUIStore()

const keys = computed(() => translate.keys)

const toggleNavbar = () => {
  if (window.innerWidth < 768) {
    ui.toggleNavbar()
  }
}
</script>

<template>
  <div flex="~ col" w="auto" h="screen" shadow="inner" bg="crimson-50" rounded="none md:tr-xl" class="<md:fixed md:block">
    <custom-scrollbar :auto-expand="false" class="h-100vh w-100vh md:w-64 md:h-100vh">
      <ul flex="~ col" w="full" h="full" mb="12">
        <li
          v-for="key in keys"
          :key="key"
          :class="{ 'font-bold': $route.params.key === key }"
          flex="~ row"
          items="center"
          w="full"
          h="10"
          px="4"
        >
          <RouterLink :to="{ name: 'translate', params: { key } }" w="full" @click="toggleNavbar">
            {{ key }}
          </RouterLink>
        </li>

        <li v-for="i in 20" :key="i" flex="~ row" w="full" h="10" px="4" items="center">
          <RouterLink :to="{ name: 'translate', params: { key: i } }" w="full" @click="toggleNavbar">
            empty {{ i }}
          </RouterLink>
        </li>
      </ul>
    </custom-scrollbar>
  </div>
</template>

<style scoped></style>
