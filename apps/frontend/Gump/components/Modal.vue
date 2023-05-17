<script setup lang="ts">
defineProps<{
  show: boolean
  title: string
  users: User[]
}>()

defineEmits<{
  (event: 'close'): void
}>()

const image = useImageStore()
const localePath = useLocalePath()
const ui = useUIStore()

async function openUser(id: number) {
  ui.activeNav = 'Profile'
  ui.setParams('user', id)
  await navigateTo(localePath(`/user/${id}`))
}
</script>

<template>
  <div v-if="show" shadow-crimsonUp fixed inset-0 z-11 mt-5 items-center justify-center>
    <div flex="~ col" h-screen w-screen overflow-auto bg-crimson-50 p-5>
      <div flex="~ row" mb-5 items-center justify-between>
        <div />
        <div text-2xl font-bold>
          {{ title }}
        </div>
        <div cursor-pointer @click="$emit('close')">
          <img src="~assets/Close.svg">
        </div>
      </div>
      <div v-for="user of users" :key="user.id" my-7>
        <div flex="~ row" cursor-pointer items-center @click="openUser(user.id)">
          <img :src="image.getImageUrl(user.profilePicture)" h-20 w-20 b-rd-xl bg-grey-900 object-cover>
          <div ml-5 text-xl font-bold>
            {{ user.username }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
