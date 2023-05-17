<script setup lang="ts">
const ui = useUIStore()
const user = useUserStore()
const image = useImageStore()
const localePath = useLocalePath()

const id = ui.params.user
const currentUser = await user.getUserById(id)
const badges = await user.getBadgesByUser(id)

ui.activeNav = 'Profile'

async function openUserRecipes() {
  ui.activeNav = 'Recipes'
  ui.setParams('recipes', id)
  await navigateTo(localePath(`/recipes/${id}`))
}

console.log(currentUser)
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :subtitle="currentUser?.username" :title="$t('ProfileNav')" />
    <div mx-4 grow overflow-y-auto>
      <div flex="~ row" my-7 justify-around>
        <img :src="image.getImageUrl(currentUser?.profilePicture ?? 1)" h-37 w-37 b-rd-xl bg-grey-900>
        <div flex="~ col" justify-between text-center text-6>
          <div>
            {{ $t('ProfileFollowers') }}
            <div font-bold>
              {{ currentUser?.followerCount }}
            </div>
          </div>
          <div>
            {{ $t('ProfileFollowing') }}
            <div font-bold>
              {{ currentUser?.followingCount }}
            </div>
          </div>
        </div>
      </div>
      <div flex="~ col">
        <h2 text-2xl font-bold underline @click="openUserRecipes()">
          {{ $t('RecipesNav') }}
        </h2>
        <h2 text-2xl font-bold>
          {{ $t('ProfileBadges') }}
        </h2>
        <Badge v-for="badge of badges" :key="badge.name" :badge="badge" my-3 />
      </div>
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped></style>
