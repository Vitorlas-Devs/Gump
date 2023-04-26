<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { usePartnerStore, type IPartner } from '@/stores/partnerStore'
import SimpleButton from './SimpleButton.vue'

const partnerStore = usePartnerStore()
const state = ref('default')

const props = defineProps<{
  partner: IPartner
}>()

const emit = defineEmits<{
  (e: 'delete'): void
}>()

const modified = reactive<IPartner>({
  id: 0,
  name: '',
  contactUrl: '',
  apiUrl: '',
  ads: []
})

const modifyButtonClick = async () => {
  modified.id = props.partner.id
  modified.name = props.partner.name
  modified.contactUrl = props.partner.contactUrl
  modified.apiUrl = props.partner.apiUrl
  state.value = 'modify'
}

const finalizeModify = async () => {
  await partnerStore.updatePartner(modified)
  const storedPartner = partnerStore.partners.find((p) => p.id === props.partner.id)
  if (storedPartner) storedPartner.name = modified.name
  state.value = 'default'
}

const deleteButtonClick = async () => {
  //   await partnerStore.deletePartner(props.partner.id)
  emit('delete')
}

onMounted(() => {
  if (props.partner.id === 0) state.value = 'modify'
})
</script>

<template>
  <div>
    <div
      v-if="state === 'default'"
      flex="~"
      justify="between"
      items="center"
      w="120"
      p="4"
      bg="orange-100"
      rounded="20px"
    >
      <p text="xl" font="bold" ml="1">{{ partner.id }} - {{ partner.name }}</p>
      <div flex="~" gap="4">
        <SimpleButton type="solid" color="orange-500" text="Modify" @click="modifyButtonClick()" />
        <SimpleButton type="solid" color="crimson-500" text="Delete" @click="deleteButtonClick()" />
      </div>
    </div>
    <div
      v-if="state === 'modify'"
      flex="~ col"
      p="4"
      w="120"
      bg="orange-100"
      rounded="20px"
      gap="4"
    >
      <div flex="~" justify="between" items="center">
        <div w="30" align="right">
          <label for="modifyName" text="20px">Name</label>
        </div>
        <div flex="~" w="78">
          <input
            id="modifyName"
            v-model="modified.name"
            type="text"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
          />
        </div>
      </div>
      <div flex="~" justify="between" items="center">
        <div w="30" align="right">
          <label for="modifyContact" text="20px">ContactUrl</label>
        </div>
        <div flex="~" w="78">
          <input
            id="modifyContact"
            v-model="modified.contactUrl"
            type="text"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
          />
        </div>
      </div>
      <div flex="~" justify="between" items="center">
        <div w="30" align="right">
          <label for="modifyApi" text="20px">ApiUrl</label>
        </div>
        <div flex="~" w="78">
          <input
            id="modifyApi"
            v-model="modified.apiUrl"
            type="text"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
          />
        </div>
      </div>
      <div flex="~" justify="end">
        <SimpleButton
          type="text"
          color="crimson-500"
          text="Cancel"
          ml="4"
          @click="state = 'default'"
        />
        <SimpleButton
          type="solid"
          color="orange-500"
          text="Done"
          ml="4"
          @click="finalizeModify()"
        />
      </div>
    </div>
  </div>
</template>
