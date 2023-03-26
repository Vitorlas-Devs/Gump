<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()

const code = router.currentRoute.value.query.code

if (!localStorage.getItem('access_token')) {
  console.log('no access token, requesting one...')
  axios
    .post('http://46.101.114.190:9999/authenticate/' + code)
    .then((response) => {
      localStorage.setItem('access_token', response.data.access_token)
      console.log('response', response)
      console.log('access token', response.data.access_token)
      router.push('/translate/Welcome')
    })
    .catch((error) => {
      console.log(error)
    })
}
</script>

<template>
  <main class="flex flex-row w-full h-screen">
    <TheNavigation />
    <div class="flex flex-col w-full h-full p-6">
      <h1 class="text-3xl text-orange-500 text-shadow-orange my-2 font-bold">Translate</h1>
      <h3>Choose your language.</h3>
      <p>And select a key from the left sidebar.</p>
    </div>
  </main>
</template>

<style scoped></style>
