<script setup lang="ts">
const user = useUserStore()
const image = useImageStore()
const ui = useUIStore()
const badge = useBadgeStore()

const { open, onChange } = useFileDialog()
const i18n = useI18n()

const profilePicture = ref('')

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
  await badge.getAll()
  profilePicture.value = image.getImageUrl(user.current.profilePicture)
  ui.uploadedImage = user.current.profilePicture
})

async function openModal(type: FollowsSort) {
  follows.value = await user.getFollows(type) as User[]
  followsTitle.value = type === 'Followers' ? i18n.t('ProfileFollowers') : i18n.t('ProfileFollowing')
  showModal.value = true
}

const userBadges = await badge.getBadgesByUser(user.current.id)
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :show-moderator="user.current.isModerator" :title="$t('ProfileNav')" />
    <div mx-4 grow overflow-y-auto>
      <div flex="~ row" my-7 justify-around>
        <img :src="profilePicture" h-35 w-35 cursor-pointer rounded-xl bg-grey-900 object-cover @click="open({ multiple: false, accept: 'image/png' })">
        <div flex="~ col" justify-between text-center text-lg>
          <div cursor-pointer @click="openModal('Followers')">
            {{ $t('ProfileFollowers') }}
            <div font-bold>
              {{ formatNumber(user.current.follower.length) }}
            </div>
          </div>
          <div cursor-pointer @click="openModal('Following')">
            {{ $t('ProfileFollowing') }}
            <div font-bold>
              {{ formatNumber(user.current.following.length) }}
            </div>
          </div>
        </div>
      </div>
      <div flex="~ col" gap-2>
        <div v-for="(field, index) in Object.keys(userDto)" :key="index" flex="~ col" justify-center gap-2>
          <h2 text-xl font-bold>
            {{ $t(`Profile${field.charAt(0).toUpperCase() + field.slice(1)}`) }}
          </h2>
          <input v-model="userDto[field as keyof UserDto]" :type="field === 'password' ? 'password' : 'text'" h-10 border-0 rounded-full px-3 font-normal shadow-inner @change="modifyUser()">
        </div>
        <h2 text-xl font-bold>
          {{ $t('ProfileLanguage') }}
        </h2>
        <LanguageSwitcher />
        <h2 text-xl font-bold>
          {{ $t('ProfileBadges') }}
        </h2>
        <div v-if="userBadges">
          <Badge v-for="b of userBadges" :key="b.id" :badge="b" />
        </div>
        <p v-else>
          No badges yet
        </p>
      </div>
      <Modal :show="showModal" :title="followsTitle" :users="follows" @close="showModal = false" />
    </div>
    <TheNavbar />
  </ion-page>
</template>
