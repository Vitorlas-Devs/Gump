<script setup lang="ts"></script>

<template>
  <select v-model="$i18n.locale" class="locale-changer">
    <option v-for="locale in $i18n.availableLocales" :key="locale" :value="locale">
      {{ locale }}
    </option>
  </select>
</template>

<style scoped></style>
