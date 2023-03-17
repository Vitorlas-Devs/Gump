<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useTranslationStore } from '@/stores/translationStore'
import { computed } from 'vue'

const translate = useTranslationStore()

const dirty = computed(() => translate.dirty)

const saveChanges = () => {
  console.log('Saving changes...')
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
      class="flex flex-row m-5 items-center gap-4 bg-emerald-100 border border-emerald-400 text-emerald-700 px-4 py-2 rounded-xl relative"
    >
      <strong class="font-bold">You have unsaved changes!</strong>
      <span class="block sm:inline">Please save your changes.</span>
      <button
        class="bg-emerald-500 hover:bg-emerald-600 transition text-white font-bold py-2 px-4 ml-4 rounded-lg"
        @click="saveChanges"
      >
        Save
      </button>
    </div>
  </div>
</template>
