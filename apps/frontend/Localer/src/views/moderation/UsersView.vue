<script setup lang="ts">
import UserItem from '@/components/moderation/UserItem.vue'
import { onMounted } from 'vue'
import SimpleButton from '@/components/moderation/SimpleButton.vue'
import { useGumpUserStore } from '@/stores/gumpUserStore'

const user = useGumpUserStore()

onMounted(async () => {
  await user.loadUsers()
})
</script>

<template>
  <main flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
    <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="25">
      <h1 text="3xl" mb="4" font="bold">Users</h1>
      <div flex="~ wrap">
        <UserItem
          v-for="u of user.users"
          :key="u.id"
          :user="u"
          mb="4"
          mr="4"
          @delete="user.users = user.users.filter((i) => i.id !== u.id)"
        />
      </div>
      <div flex="~" w="full" justify="center">
        <SimpleButton
          type="solid"
          color="crimson-500"
          text="Load more"
          @click="user.fetchUsers()"
        />
      </div>
    </custom-scrollbar>
  </main>
</template>
