<script setup lang="ts">
const user = useUserStore()
const localePath = useLocalePath()

const canLogin = ref(false)

async function skipLogin() {
  user.current = {
    id: 0,
    username: '',
    password: '',
    profilePicture: 0,
    recipes: [],
    likes: [],
    following: [],
    follower: [],
    badges: [],
    language: '',
    isModerator: false,
    token: 'offline',
  }
  await navigateTo(localePath('/home'))
}

onMounted(async () => {
  if (user.current.token) {
    await navigateTo(localePath('/home', user.current.language ?? 'en_US'))
  } else {
    canLogin.value = true
    await navigateTo(localePath('/', user.current.language ?? 'en_US'))
  }
})
</script>

<template>
  <ion-page bg-crimson-50>
    <div>
      <img src="~assets/bg-portrait-trimmed.svg" w-full>
      <img src="~assets/gump-logo.svg" w="1/2" absolute bottom-0 left-0 right-0 top-0 m-auto transition-all :transform="canLogin ? 'translate-y--20' : 'translate-y-0'">
      <div v-if="canLogin" w="4/5" flex="~ row" absolute bottom-20 left-0 right-0 m-auto h-max items-center justify-evenly rounded-full bg-crimson-50 py-4 text-2xl font-bold>
        <NuxtLink :to="localePath('/login', user.current.language ?? 'en_US')" text-crimson-500 decoration-none text-shadow-crimson>
          {{ $t('WelcomeSignIn') }}
        </NuxtLink>
        <NuxtLink :to="localePath('/register', user.current.language ?? 'en_US')" border-3 border-crimson-500 rounded-full border-solid px-3 py-2 text-crimson-500 decoration-none text-shadow-crimson>
          {{ $t('WelcomeSignUp') }}
        </NuxtLink>
      </div>
      <div w="4/5" flex="~ row" absolute bottom-4 left-0 right-0 m-auto h-max items-center justify-evenly rounded-full py-4 text-xl font-bold>
        <p :to="localePath('/home')" m-0 cursor-pointer text-crimson-100 decoration-none @click="skipLogin">
          Skip
        </p>
      </div>
    </div>
  </ion-page>
</template>
