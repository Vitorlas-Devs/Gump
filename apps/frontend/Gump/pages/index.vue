<script setup lang="ts">
// import { useUserStore } from '~/stores/user'

// const user = useUserStore()

// reroute the use to /home
navigateTo('/home')
</script>

<template>
  <ion-page bg-crimson-50 p-2>
    <div>
      <!-- Welcome page -->
      <!-- if no user: route to login -->
      <!-- if user: route to home -->
    </div>
  </ion-page>
</template>
