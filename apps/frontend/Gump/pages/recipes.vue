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
  likeCount: 0,
  referenceCount: 1,
  isArchived: false,
  isOriginal: true,
  originalRecipe: 0,
  isPrivate: false,
  forks: [],
}
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('RecipesNav')" />
    <div grow>
      <CreateSubHeader variant="ingredients" />
      <CreateSubHeader variant="steps" />
      <h1 text-crimson-500 text-shadow-crimson>
        {{ $t('RecipesNav') }}
      </h1>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="orangeGradient" shadow="orange">
        <div class="i-fa6-solid-box-tissue" />
      </div>
      <div mt-5 class="crimsonGradient" shadow="crimson">
        <div class="i-fa6-solid-brush" />
      </div>
    </div>
    <RecipeBoxMini :recipe="test" />
    <TheNavbar />
  </ion-page>
</template>
