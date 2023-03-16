<script setup lang="ts">
import { allLocales, setLocalee } from '@/i18n'

const handleChange = (event: Event) => {
  const target = event.target as HTMLSelectElement
  setLocalee(target.value)
}
</script>

<template>
  <select v-model="$i18n.locale" class="locale-changer" @change="handleChange">
    <option v-for="locale in allLocales" :key="locale" :value="locale">
      {{ locale }}
    </option>
  </select>
</template>

<style scoped></style>
