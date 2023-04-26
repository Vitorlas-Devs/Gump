<script setup lang="ts">
import router from '@/router'
import { onMounted } from 'vue'
import { useGumpUserStore } from '@/stores/gumpUserStore'
import NavItem from '@/components/moderation/NavItem.vue'

const gumpUser = useGumpUserStore()

onMounted(() => {
  if (!gumpUser.sessionToken) {
    router.push('/login')
  }
})
</script>

<template>
  <div class="w-xs h-100vh pt-2" w="full md:w-72" rounded="none md:tr-xl" shadow="inner">
    <NavItem value="Recipes" />
    <NavItem value="Users" />
    <NavItem value="Partners" />
    <NavItem value="Adverts" />
  </div>
</template>
