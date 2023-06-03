<script setup lang="ts">
const user = useUserStore()
const localePath = useLocalePath()

const userDto = reactive<UserDto>({
  username: '',
  email: '',
  password: '',
})

async function handleRegister() {
  const error = await user.register(userDto)
  if (error)
    console.log(error)
  else
    await navigateTo(localePath('/home'))
}
</script>

<template>
  <ion-page overflow-y-auto bg-crimson-50>
    <div flex="~ col" items-center justify-center gap-5 font-bold>
      <div crimsonIcon i-shadow:fa6-solid-arrow-left absolute left-2 top-2 cursor-pointer @click="navigateTo(localePath('/'))" />
      <h1 flex-1 text-4xl text-crimson-500 text-shadow-crimson>
        {{ $t('WelcomeSignUp') }}
      </h1>
      <div v-for="(field, index) in Object.keys(userDto)" :key="index" flex="~ col" items-center justify-center gap-2>
        <h2 text-2xl text-crimson-500 text-shadow-crimson>
          {{ $t(`Profile${field.charAt(0).toUpperCase() + field.slice(1)}`) }}
        </h2>
        <input
          v-model="userDto[field as keyof UserDto]"
          :type="field === 'password' ? 'password' : 'text'"
          shadow-innerCrimson h-10 border-0 rounded-full px-3 font-normal
        >
      </div>
      <MainButton
        transform-none
        :title="$t('WelcomeSignUp')"
        color="crimsonGradient"
        @click="handleRegister"
      />
    </div>
  </ion-page>
</template>
