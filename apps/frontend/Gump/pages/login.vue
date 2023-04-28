<script setup lang="ts">
import { type UserData, useUserStore } from '~/stores/user'

const user = useUserStore()

const userData = reactive<UserData>({
  username: '',
  password: '',
  email: '',
})
</script>

<template>
  <ion-page bg-crimson-50>
    <div
      flex="~ col" items-center justify-center font-bold
    >
      <h1 text-4xl text-crimson-500 text-shadow-crimson>
        {{ $t('WelcomeSignIn') }}
      </h1>
      <div>
        <h2 text-3xl text-crimson-500 text-shadow-crimson>
          {{ $t('ProfileUsername') }}
        </h2>
        <input
          v-model="userData.username"
          w="4/5" h="2rem"
          shadow-inner
        >
      </div>
    </div>
  </ion-page>
</template>

<style scoped>

</style>
