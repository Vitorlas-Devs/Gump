<script setup lang="ts">
import router from '@/router'
import { useGumpUserStore } from '@/stores/gumpUserStore'
import { onMounted } from 'vue'

const gumpUser = useGumpUserStore()

onMounted(() => {
  if (!gumpUser.sessionToken) {
    router.push('/login')
  }
})
</script>

<template>
  <main>Moderation</main>
</template>
