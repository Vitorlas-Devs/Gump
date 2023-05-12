<script setup lang="ts">
const test: Recipe = {
  id: 2,
  title: 'Főtt kockatészta',
  author: 2,
  image: 32,
  language: 'hu_HU',
  serves: 4,
  categories: [
    'd',
  ],
  tags: [
    'főtt',
    'kockatészta',
    'kocka',
    'tészta',
  ],
  ingredients: [
    {
      name: 'Kockatészta',
      value: 250,
      volume: 'g',
      linkedRecipe: 0,
    },
    {
      name: 'Só',
      value: 2,
      volume: 'tk',
      linkedRecipe: 0,
    },
    {
      name: 'Víz',
      value: 2,
      volume: 'l',
      linkedRecipe: 0,
    },
  ],
  steps: [
    'Öntsük bele a vizet egy kellően nagy fazékba.',
    'Tegyük bele a vízbe a sót.',
    'Gyújtsunk alá, és forrásig melegítsük a vizet.',
    'Szórjuk bele a tésztát a vízbe, majd vegyük lejjebb a hőfokot, hogy csak halkan bugyogjon.',
    '5-10 percig főzzük.',
    'Amint elkészült, szűrjük le és már fogyasztható is.',
  ],
  viewCount: 1,
  saveCount: 0,
  isLiked: false,
  isSaved: false,
  likeCount: 0,
  referenceCount: 1,
  isArchived: false,
  isOriginal: true,
  originalRecipe: 0,
  isPrivate: false,
  forks: [],
}

const recipeSort = ref('owned')
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('RecipesNav')" />
    <div flex="~ row" items-center justify-around>
      <div :class="recipeSort === 'owned' ? 'bg-orange-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-orange-500' : 'text-orange-500 text-shadow-orange'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'owned'">
        Owned
      </div>
      <div :class="recipeSort === 'likes' ? 'bg-crimson-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-crimson-500' : 'text-crimson-500 text-shadow-crimson'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'likes'">
        Likes
      </div>
      <div :class="recipeSort === 'saved' ? 'bg-blue-500 text-white-500 drop-shadow-sm b-rd-25 drop-shadow-color-blue-500' : 'text-blue-500 text-shadow-blue'" cursor-pointer px-4 py-1 text-6 @click="recipeSort = 'saved'">
        Saved
      </div>
    </div>
    <RecipeBoxMini :recipe="test" />
    <TheNavbar />
  </ion-page>
</template>
