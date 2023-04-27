<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useAdvertStore, type IAdvert } from '@/stores/advertStore'
import { usePartnerStore } from '@/stores/partnerStore'
import SimpleButton from './SimpleButton.vue'
import { useFileDialog } from '@vueuse/core'
import { type } from 'os'

const advertStore = useAdvertStore()
const partnerStore = usePartnerStore()
const state = ref('default')

const { open, onChange } = useFileDialog()

onChange((files: any) => {
  if (files[0].type !== 'image/png') return

  const reader = new FileReader()
  reader.readAsDataURL(files[0])
  reader.onload = () => {
    console.log('WIP...')
  }
})

const props = defineProps<{
  advert: IAdvert
}>()

const emit = defineEmits<{
  (e: 'done'): void
  (e: 'cancel'): void
  (e: 'delete'): void
}>()

const imageUrl = computed(() => `${import.meta.env.VITE_BACKEND_URL}/image/${props.advert.image}`)
const partner = ref(partnerStore.partners.find((p) => p.id === props.advert.partner))
const modified = reactive<IAdvert>({
  id: 0,
  partner: 0,
  title: '',
  image: 0
})

const modifyButtonClick = async () => {
  modified.id = props.advert.id
  modified.partner = props.advert.partner
  modified.title = props.advert.title
  modified.image = props.advert.image
  state.value = 'modify'
}

const finalizeModify = async () => {
  await advertStore.updateAdvert(modified)
  const storedAdvert = advertStore.adverts.find((p) => p.id === props.advert.id)
  if (storedAdvert) storedAdvert.title = modified.title
  state.value = 'default'
}

const insert = async () => {
  emit('done')
  await advertStore.insertAdvert(modified)
  state.value = 'default'
}

const deleteButtonClick = async () => {
  await advertStore.deleteAdvert(props.advert.id)
  emit('delete')
}

onMounted(() => {
  if (props.advert.id === 0) state.value = 'new'
})
</script>

<template>
  <div>
    <div v-if="state === 'default'" flex="~" w="120" p="4" bg="orange-100" rounded="20px" gap="4">
      <img :src="imageUrl" w="60" h="40" object="cover" rounded="8px" />
      <div flex="~ col" justify="between" w="full">
        <div flex="~ col" gap="2">
          <p text="xl" font="bold">{{ advert.id }} - {{ advert.title }}</p>
          <p>Partner: {{ partner?.name }} ({{ partner?.id }})</p>
        </div>
        <div flex="~" justify="end" gap="4">
          <SimpleButton
            type="solid"
            color="orange-500"
            text="Modify"
            @click="modifyButtonClick()"
          />
          <SimpleButton
            type="solid"
            color="crimson-500"
            text="Delete"
            @click="deleteButtonClick()"
          />
        </div>
      </div>
    </div>
    <div
      v-if="state === 'modify' || state === 'new'"
      flex="~ col"
      p="4"
      w="120"
      bg="orange-100"
      rounded="20px"
      gap="4"
    >
      <div flex="~" justify="between" items="center">
        <div w="30" align="right">
          <label for="modifyImage" text="20px">Image</label>
        </div>
        <div flex="~" w="78" gap="4">
          <input
            id="modifyImage"
            v-model="modified.image"
            type="text"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
            disabled
          />
          <input ref="file" type="file" style="display: none" />
          <SimpleButton
            type="text"
            color="crimson-500"
            text="Select"
            @click="
              open({
                multiple: false,
                accept: 'image/png'
              })
            "
          />
        </div>
      </div>
      <div flex="~" justify="between" items="center">
        <div w="30" align="right">
          <label for="modifyTitle" text="20px">Title</label>
        </div>
        <div flex="~" w="78">
          <input
            id="modifyTitle"
            v-model="modified.title"
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
          <label for="modifyPartner" text="20px">Partner</label>
        </div>
        <div flex="~" w="78">
          <input
            id="modifyPartner"
            v-model="modified.partner"
            type="number"
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
          @click="state === 'new' ? emit('cancel') : (state = 'default')"
        />
        <SimpleButton
          type="solid"
          color="orange-500"
          text="Done"
          ml="4"
          @click="state === 'new' ? insert() : finalizeModify()"
        />
      </div>
    </div>
  </div>
</template>
