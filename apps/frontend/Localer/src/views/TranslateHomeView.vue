<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()

const code = router.currentRoute.value.query.code

if (!localStorage.getItem('access_token')) {
  console.log('no access token, requesting one...')
  axios
    .get('http://46.101.114.190:3000/access_token/', {
      params: {
        code: code
      }
    })
    .then((response) => {
      console.log('response', response)
      const access_token = response.data.split('=')[1].split('&')[0]
      console.log('access token', access_token)
      localStorage.setItem('access_token', access_token)
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
