<script setup lang="ts">
const user = useUserStore()
const title = 'Nagyon Fincsi pogácsa'
const authorId = 1
const imgId = 4
</script>

<template>
  <ion-page bg-crimson-50>
    <RecipeBoxMini :title="title" :author-id="authorId" :img-id="imgId" />
    <RecipeBox :title="title" :author-id="authorId" :img-id="imgId" />
    <TheNavbar />
  </ion-page>
</template>
