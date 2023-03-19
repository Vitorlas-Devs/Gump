<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'

const translate = useTranslationStore()

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  translate.saveChanges()
}
</script>

<template>
  <div>
    <div class="flex flex-row gap-4 mx-5 my-2">
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/translate-home">Translate</RouterLink>
    </div>
    <RouterView :key="$route.fullPath" />
  </div>
  <div class="fixed bottom-0 mx-auto left-0 right-0 w-max" v-if="dirty">
    <div
      class="flex flex-row m-5 items-center gap-4 bg-crimson-100 border text-crimson-500 shadow-crimson-500 shadow-md px-4 py-2 rounded-xl relative"
    >
      <strong class="font-bold">You have unsaved changes!</strong>
      <span class="block sm:inline">Click here when you're done.</span>
      <button
        class="bg-crimson-500 hover:bg-crimson-600 transition text-crimson-200 font-bold shadow-crimson-500 shadow-md py-2 px-4 ml-4 rounded-lg"
        @click="saveChanges"
      >
        Save
      </button>
    </div>
  </div>
</template>
