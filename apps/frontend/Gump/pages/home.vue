<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()
const user = useUserStore()

watch(
  () => ui.activeSort,
  async () => await recipe.getRecipes(ui.activeSort),
  { immediate: true },
)

ui.activeNav = 'Home'

onMounted(async () => {
  await user.getUserData()
})
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader show-icons :title="$t('HomeNav')" />
    <div v-if="recipe.recipes" grow overflow-y-auto pb-30>
      <RecipeBox v-for="r of recipe.recipes" :id="r.id" :key="r.id" :title="r.title" :author-id="r.author" :img-id="r.image" :views="r.viewCount" :likes="r.likeCount" :saves="r.saveCount" />
      <br><br>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="crimsonBtn">
        <div class="i-fa6-solid-box-tissue" />
      </div>
      <MainButton fixed color="orange" icon-type="create" title="Create recipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped></style>
