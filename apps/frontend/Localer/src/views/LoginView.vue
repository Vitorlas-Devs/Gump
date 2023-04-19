<script setup lang="ts">
import { useGumpUserStore } from '@/stores/gumpUserStore'

const gumpUser = useGumpUserStore()

const login = () => {
  const username = (document.getElementById('gumpUsername') as HTMLInputElement).value
  const password = (document.getElementById('gumpPassword') as HTMLInputElement).value
  gumpUser.login(username, password)
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
          bg="crimson-50"
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
          bg="crimson-50"
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
        @click="login"
      >
        Log In
      </button>
    </div>
  </div>
</template>
