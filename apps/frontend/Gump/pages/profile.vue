<script setup lang="ts">
const localePath = useLocalePath()
</script>

<template>
  <ion-page bg-crimson-50 p-3>
    <div>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ $t('ProfileNav') }}
      </h1>
      <div flex flex-row gap-4>
        <NuxtLink class="linkOrange" :to="localePath('/')">
          {{ $t('HomeNav') }}
        </NuxtLink>
        <NuxtLink class="linkOrange" :to="localePath('search')">
          {{ $t('SearchNav') }}
        </NuxtLink>
        <NuxtLink class="linkOrange" :to="localePath('profile')">
          {{ $t('ProfileNav') }}
        </NuxtLink>
      </div>
      <br><br>
      Select Language:
      <LanguageSwitcher />
    </div>
  </ion-page>
</template>
