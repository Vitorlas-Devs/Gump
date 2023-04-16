<script setup lang="ts">
const localePath = useLocalePath()
</script>

<template>
  <ion-page bg-crimson-50 p-3>
    <div>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ $t('HomeNav') }}
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
      <div mt-5 class="btnCrimson">
        <div class="i-fa6-solid-box-tissue" />
      </div>
    </div>
  </ion-page>
</template>
