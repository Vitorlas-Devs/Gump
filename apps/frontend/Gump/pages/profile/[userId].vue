<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

const route = useRoute()
const ui = useUIStore()

const id = route.params.userId

ui.activeNav = 'Profile'
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader title-color="brown" subtitle="Bartók Béla" title="Profile" />
    <div grow>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ id }}
      </h1>
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
