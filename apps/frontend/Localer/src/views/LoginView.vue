<script setup lang="ts">
import router from '@/router'
import { useGumpUserStore } from '@/stores/gumpUserStore'
import { reactive, ref } from 'vue'

const gumpUser = useGumpUserStore()
const state = reactive({
  isLoading: false,
  error: ''
})

const login = async () => {
  state.isLoading = true

  const username = document.getElementById('gumpUsername') as HTMLInputElement
  const password = document.getElementById('gumpPassword') as HTMLInputElement
  const success = gumpUser.login(username.value, password.value)

  if (success) {
    router.push('/moderation')
    return
  }

  password.value = ''
  state.isLoading = false
  state.error = 'Invalid username or password'
}
</script>

<template>
  <div flex="~ col" items="center" w="full" h="full">
    <div
      flex="~ col"
      items="center"
      gap="2"
      w="sm"
      m="10"
      p="12"
      rounded="4xl"
      shadow="crimsonDown"
      drop-shadow="xl"
      bg="crimson-100"
    >
      <h1 text="4xl" font="bold" mb="6">Log In</h1>
      <div flex="~ col">
        <label m="2" for="gumpUsername">Username</label>
        <input
          id="gumpUsername"
          type="text"
          shadow="innerCrimson"
          rounded="full"
          w="full"
          p="2"
          :on-submit="login"
          :bg="state.isLoading ? 'crimson-100' : 'crimson-50'"
          :disabled="state.isLoading"
          @keyup.enter="login"
        />
      </div>
      <div flex="~ col">
        <label m="2" for="gumpPassword">Password</label>
        <input
          id="gumpPassword"
          type="password"
          shadow="innerCrimson"
          rounded="full"
          w="full"
          p="2"
          :bg="state.isLoading ? 'crimson-100' : 'crimson-50'"
          :disabled="state.isLoading"
          @keyup.enter="login"
        />
      </div>
      <button
        shadow="crimsonDown"
        bg="crimson-500"
        text="orange-50 shadow-white"
        rounded="full"
        py="2"
        px="4"
        font="bold"
        m="6"
        :disabled="state.isLoading"
        :cursor="state.isLoading ? 'default' : 'pointer'"
        @click="login"
      >
        Log In {{ state.isLoading }}
      </button>
      <p v-if="state.error" text="crimson-500" font="bold">{{ state.error }}</p>
    </div>
  </div>
</template>
