<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useGumpUserStore, type IGumpUserData } from '@/stores/gumpUserStore'
import SimpleButton from './SimpleButton.vue'

const userStore = useGumpUserStore()
const state = ref('default')

const props = defineProps<{
  user: IGumpUserData
}>()

const emit = defineEmits<{
  (e: 'delete'): void
}>()

const pfpUrl = computed(
  () => `${import.meta.env.VITE_BACKEND_URL}/image/${props.user.profilePicture}`
)
const modified = reactive<IGumpUserData>({
  id: 0,
  username: '',
  password: '',
  profilePicture: 0,
  pfpUrl: '',
  sessionToken: '',
  users: []
})

const modifyButtonClick = async () => {
  state.value = 'modify'
  modified.id = props.user.id
  modified.username = props.user.username
  modified.password = props.user.password
  modified.profilePicture = props.user.profilePicture
}

const finalizeModify = async () => {
  await userStore.updateUser(modified)
  props.user.profilePicture = modified.profilePicture
  state.value = 'default'
}

const deleteButtonClick = async () => {
  await userStore.deleteUser(userStore.id)
  emit('delete')
}
</script>

<template>
  <div>
    <div v-if="state === 'default'" flex="~" p="4" bg="orange-100" rounded="20px">
      <img :src="pfpUrl" w="60" h="40" object="cover" rounded="8px" />
      <div pl="4" h="40" flex="~ col" justify="between">
        <div>
          <p text="xl" font="bold" w="80" mb="2">{{ user.username }}</p>
        </div>
        <div flex="~" justify="end">
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
            ml="4"
            @click="deleteButtonClick()"
          />
        </div>
      </div>
    </div>
    <div v-if="state === 'modify'" flex="~ col" p="4" w="152" bg="orange-100" rounded="20px">
      <p text="xl" font="bold" w="80" mb="4">{{ user.username }}</p>
      <div flex="~" justify="between" items="center" mb="4">
        <div w="25" align="right">
          <label for="modifyImage" text="20px">Image</label>
        </div>
        <div flex="~" w="115">
          <input
            id="modifyImage"
            v-model="modified.profilePicture"
            type="text"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
            disabled
          />
          <SimpleButton
            type="text"
            color="crimson-500"
            text="Delete"
            ml="4"
            @click="modified.profilePicture = 1"
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
          text="Modify"
          ml="4"
          @click="finalizeModify()"
        />
      </div>
    </div>
  </div>
</template>
