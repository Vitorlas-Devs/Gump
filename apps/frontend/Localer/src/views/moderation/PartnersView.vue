<script setup lang="ts">
import { usePartnerStore, type IPartner } from '@/stores/partnerStore'
import { onMounted, reactive, ref } from 'vue'
import PartnerItem from '@/components/moderation/PartnerItem.vue'
import SimpleButton from '@/components/moderation/SimpleButton.vue'

const partner = usePartnerStore()
const isNew = ref(false)
const newPartner = reactive<IPartner>({
  id: 0,
  name: '',
  contactUrl: '',
  apiUrl: '',
  ads: []
})

onMounted(() => {
  partner.fetchAllPartners()
})
</script>

<template>
  <main flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
    <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="25">
      <div flex="~" gap="4" mb="4">
        <h1 text="3xl" font="bold">Partners</h1>
        <SimpleButton type="solid" color="green" text="Add" @click="isNew = true" />
      </div>
      <div flex="~ wrap" gap="4">
        <PartnerItem
          v-if="isNew"
          :partner="newPartner"
          @done="isNew = false"
          @cancel="isNew = false"
        />
        <PartnerItem
          v-for="p of partner.partners"
          :key="p.id"
          :partner="p"
          @delete="partner.partners = partner.partners.filter((x) => x.id !== p.id)"
        />
      </div>
    </custom-scrollbar>
    <div bg="green" display="none" />
  </main>
</template>
