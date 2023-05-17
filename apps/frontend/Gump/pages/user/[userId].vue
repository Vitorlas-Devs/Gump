<script setup lang="ts">
const ui = useUIStore()
const user = useUserStore()
const image = useImageStore()
const badge = useBadgeStore()
const localePath = useLocalePath()

const id = ui.params.user
const currentUser = ref(await user.getUserById(id))

let userBadges: Badge[] | undefined

if (currentUser.value)
  userBadges = await badge.getBadgesByUser(currentUser.value.id)

ui.activeNav = 'Profile'

async function openUserRecipes() {
  ui.activeNav = 'Recipes'
  ui.setParams('recipes', id)
  await navigateTo(localePath(`/recipes/${id}`))
}

const isFollowing = computed({
  get: () => {
    if (currentUser.value)
      return user.current.following.includes(currentUser.value.id)
    return false
  },
  set: (value: boolean) => {
    if (currentUser.value) {
      if (!value)
        currentUser.value.followerCount += 1
      else
        currentUser.value.followerCount -= 1
    }
  },
})

async function follow() {
  let response: boolean | undefined

  if (currentUser.value)
    response = await user.followUser(currentUser.value.id)

  if (response !== undefined)
    isFollowing.value = !isFollowing.value
}
</script>

<template>
  <ion-page v-if="currentUser" bg-crimson-50>
    <TheHeader cursor-pointer :subtitle="currentUser.username" :title="$t('ProfileNav')" @click="ui.setParams('user', currentUser.id); navigateTo(localePath(`/user/${currentUser.id}`))" />
    <div mx-4 grow overflow-y-auto>
      <div flex="~ row" my-7 justify-around>
        <img :src="image.getImageUrl(currentUser.profilePicture)" h-35 w-35 rounded-xl bg-grey-900 object-cover>
        <div flex="~ col" justify-between text-center text-lg>
          <div>
            {{ $t('ProfileFollowers') }}
            <div font-bold>
              {{ formatNumber(currentUser.followerCount) }}
            </div>
          </div>
          <div>
            {{ $t('ProfileFollowing') }}
            <div font-bold>
              {{ formatNumber(currentUser.followingCount) }}
            </div>
          </div>
        </div>
      </div>
      <div mx-5 flex="~ row" justify-end>
        <MainButton
          transform-none
          color="blueGradient"
          :title="isFollowing ? 'Following' : 'Follow'"
          @click="follow()"
        />
      </div>
      <div flex="~ col">
        <h2 cursor-pointer text-xl font-bold underline underline-3 underline-offset-5 @click="openUserRecipes()">
          {{ $t('RecipesNav') }}
        </h2>
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
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped></style>
