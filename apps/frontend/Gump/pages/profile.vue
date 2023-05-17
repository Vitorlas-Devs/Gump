<script setup lang="ts">
const user = useUserStore()
const image = useImageStore()
const { open, onChange } = useFileDialog()
const ui = useUIStore()
const i18n = useI18n()

const profilePicture = ref('')

const badges = await user.getBadgesByUser(user.current.id)

const showModal = ref(false)
const follows = ref<User[]>([])
const followsTitle = ref('')

const userDto = reactive<UserDto>({
  username: user.current.username,
  email: user.current.email,
  password: '',
})

async function modifyUser() {
  await user.updateUser(userDto, ui.uploadedImage, user.current.language)
  user.current.username = userDto.username ?? ''
  user.current.email = userDto.email ?? ''
  user.current.profilePicture = ui.uploadedImage
}

onChange((fileList) => {
  if (fileList && fileList.length > 0)
    image.uploadImage(fileList)
})

watch(() => [ui.uploadedImage, user.current.language], async () => {
  profilePicture.value = image.getImageUrl(ui.uploadedImage)
  await modifyUser()
})

onMounted(async () => {
  profilePicture.value = image.getImageUrl(user.current.profilePicture)
  ui.uploadedImage = user.current.profilePicture
})

async function openModal(type: FollowsSort) {
  follows.value = await user.getFollows(type) as User[]
  followsTitle.value = type === 'Followers' ? i18n.t('ProfileFollowers') : i18n.t('ProfileFollowing')
  showModal.value = true
}
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :show-moderator="user.current.isModerator" :title="$t('ProfileNav')" />
    <div mx-4 grow overflow-y-auto>
      <div flex="~ row" my-7 justify-around>
        <img :src="profilePicture" h-37 w-37 cursor-pointer b-rd-xl bg-grey-900 @click="open({ multiple: false, accept: 'image/png' })">
        <div flex="~ col" justify-between text-center text-6>
          <div cursor-pointer @click="openModal('Followers')">
            {{ $t('ProfileFollowers') }}
            <div font-bold>
              {{ user.current.follower.length }}
            </div>
          </div>
          <div cursor-pointer @click="openModal('Following')">
            {{ $t('ProfileFollowing') }}
            <div font-bold>
              {{ user.current.following.length }}
            </div>
          </div>
        </div>
      </div>
      <div flex="~ col">
        <div v-for="(field, index) in Object.keys(userDto)" :key="index" flex="~ col" justify-center gap-2>
          <h2 text-2xl font-bold>
            {{ $t(`Profile${field.charAt(0).toUpperCase() + field.slice(1)}`) }}
          </h2>
          <input v-model="userDto[field as keyof UserDto]" :type="field === 'password' ? 'password' : 'text'" h-10 border-0 rounded-full px-3 font-normal shadow-inner @change="modifyUser()">
        </div>
        <h2 text-2xl font-bold>
          {{ $t('ProfileLanguage') }}
        </h2>
        <LanguageSwitcher />
        <h2 text-2xl font-bold>
          {{ $t('ProfileBadges') }}
        </h2>
        <Badge v-for="badge of badges" :key="badge.name" :badge="badge" my-3 />
      </div>
      <Modal :show="showModal" :title="followsTitle" :users="follows" @close="showModal = false" />
    </div>
    <TheNavbar />
  </ion-page>
</template>
