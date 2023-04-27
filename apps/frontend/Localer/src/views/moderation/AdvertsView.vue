<script setup lang="ts">
import { useAdvertStore, type IAdvert } from '@/stores/advertStore'
import { onMounted, reactive, ref } from 'vue'
import AdvertItem from '@/components/moderation/AdvertItem.vue'
import SimpleButton from '@/components/moderation/SimpleButton.vue'

const advert = useAdvertStore()
const isNew = ref(false)
const newAdvert = reactive<IAdvert>({
  id: 0,
  partner: 0,
  title: '',
  image: 0
})

onMounted(() => {
  advert.fetchAllAdverts()
})
</script>

<template>
  <main flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
    <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="25">
      <div flex="~" gap="4" mb="4">
        <h1 text="3xl" font="bold">Adverts</h1>
        <SimpleButton type="solid" color="green" text="Add" @click="isNew = true" />
      </div>
      <div flex="~ wrap" gap="4">
        <AdvertItem
          v-if="isNew"
          :advert="newAdvert"
          @done="isNew = false"
          @cancel="isNew = false"
        />
        <AdvertItem
          v-for="p of advert.adverts"
          :key="p.id"
          :advert="p"
          @delete="advert.adverts = advert.adverts.filter((x) => x.id !== p.id)"
        />
      </div>
    </custom-scrollbar>
    <div bg="green" display="none" />
  </main>
</template>
