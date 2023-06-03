<script setup lang="ts">
const user = useUserStore()
const localePath = useLocalePath()

user.$reset()

const userDto = reactive<UserDto>({
  username: 'TestUser',
  password: 'secret',
})

async function handleLogin() {
  const response = await user.login(userDto)
  if (response) {
    if (response.error) {
      console.log(response.error)
    } else if (response.token) {
      await user.getUserData()
      await navigateTo(localePath('/home', user.current.language))
    }
  }
}
</script>

<template>
  <ion-page overflow-y-auto bg-crimson-50>
    <div flex="~ col" items-center justify-center gap-5 font-bold>
      <div crimsonIcon i-shadow:fa6-solid-arrow-left absolute left-2 top-2 cursor-pointer @click="navigateTo(localePath('/'))" />
      <h1 flex-1 text-4xl text-crimson-500 text-shadow-crimson>
        {{ $t('WelcomeSignIn') }}
      </h1>
      <div v-for="(field, index) in Object.keys(userDto)" :key="index" flex="~ col" items-center justify-center gap-2>
        <h2 text-2xl text-crimson-500 text-shadow-crimson>
          {{ $t(`Profile${field.charAt(0).toUpperCase() + field.slice(1)}`) }}
        </h2>
        <input v-model="userDto[field as keyof UserDto]" :type="field === 'password' ? 'password' : 'text'" shadow-innerCrimson h-10 border-0 rounded-full px-3 font-normal>
      </div>
      <MainButton transform-none :title="$t('WelcomeSignIn')" color="crimsonGradient" @click="handleLogin" />
    </div>
  </ion-page>
</template>
