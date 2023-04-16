<script setup lang="ts">
import { useUserStore } from '~/stores/user'

// deconstruct a variable from a store (with storeToRefs to keep reactivity)
const { username } = storeToRefs(useUserStore())
</script>

<template>
  <ion-page bg-crimson-50 p-2>
    <TheHeader :title="$t('HomeNav')" />
    <div grow>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ username }}
      </h1>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="btnCrimson">
        <div class="i-fa6-solid-box-tissue" />
      </div>
    </div>
    <TheNavbar />
  </ion-page>
</template>
