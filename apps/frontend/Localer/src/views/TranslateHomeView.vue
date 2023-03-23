<script setup lang="ts">
import TheNavigation from '@/components/TheNavigation.vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()

// this route gets redirected to if the user completes the github authentication
// we have a code and state parameters in the url from github
// we need to exchange the code for an access token

const code = router.currentRoute.value.query.code
const state = router.currentRoute.value.query.state

// we need to send a post request to github to exchange the code for an access token

const clientId = import.meta.env.VITE_CLIENT_ID
const clientSecret = import.meta.env.VITE_CLIENT_SECRET

axios
  .post(
    'https://github.com/login/oauth/access_token',
    {
      client_id: clientId,
      client_secret: clientSecret,
      code: code,
      state: state
    },
    {
      headers: {
        Accept: 'application/json'
      }
    }
  )
  .then((response) => {
    // we have the access token
    // we can store it in local storage
    // and redirect the user to the translate page

    localStorage.setItem('access_token', response.data.access_token)
    router.push('/translate/Welcome')
  })
  .catch((error) => {
    console.log(error)
  })
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
