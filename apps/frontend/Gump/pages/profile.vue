<script setup lang="ts">
import { useUserStore } from '~/stores/user'

const user = useUserStore()
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('ProfileNav')" />
    <div grow>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ $t('ProfileNav') }}
      </h1>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br><br>
      Select Language:
      <LanguageSwitcher />
      <br><br>
      Change username:
      <input v-model="user.username">
    </div>
    <TheNavbar />
  </ion-page>
</template>
