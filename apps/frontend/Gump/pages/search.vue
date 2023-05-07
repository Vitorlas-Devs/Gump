<script setup lang="ts">
const ui = useUIStore()
</script>

<template>
  <ion-page bg-crimson-50>
    <SearchHeader z-2 />
    <div grow />
    <TheNavbar z-2 />
    <div
      v-if="ui.dropdownToggled"
      absolute left-0 top-0 h-full w-full bg-brown-900 opacity-20
    />
  </ion-page>
</template>
