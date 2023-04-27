<script setup lang="ts">
import { useUserStore } from '~/stores/user'

const user = useUserStore()
const localePath = useLocalePath()

const canLogin = ref(false)

onMounted(() => {
  if (user.token)
    navigateTo('/home')
  else
    canLogin.value = true
})
</script>

<template>
  <ion-page bg-crimson-50>
    <div>
      <img src="~assets/bg-portrait-trimmed.svg" w-full>
      <img
        src="~assets/gump-logo.svg" w="1/2"
        absolute bottom-0 left-0 right-0 top-0 m-auto transition-all
        :transform="canLogin ? 'translate-y--20' : 'translate-y-0'"
      >
      <div
        v-if="canLogin"
        w="4/5" flex="~ row"
        absolute bottom-10 left-0 right-0 m-auto h-max items-center justify-evenly rounded-full bg-crimson-50 py-4 text-2xl font-bold
      >
        <RouterLink :to="localePath('/login')" text-crimson-500 decoration-none text-shadow-crimson>
          Sign in
        </RouterLink>
        <RouterLink :to="localePath('/register')" border-3 border-crimson-500 rounded-full border-solid px-3 py-2 text-crimson-500 decoration-none text-shadow-crimson>
          Register
        </RouterLink>
      </div>
    </div>
  </ion-page>
</template>
