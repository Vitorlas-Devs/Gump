<script setup lang="ts">
const route = useRoute()
const localePath = useLocalePath()
</script>

<template>
  <div>
    <h1 text-crimson-500 text-shadow-crimson>
      {{ $t('EnglishLanguageName') }}
    </h1>
    <p>Current route: {{ route.path }}</p>
    <NuxtLink :to="localePath('search')" class="linkOrange">
      {{ $t('SearchNav') }}
    </NuxtLink>
    <div mt-5 class="btnCrimson">
      ASD
    </div>
    <div mt-5 class="btnOrange">
      ASD
    </div>
  </div>
</template>
