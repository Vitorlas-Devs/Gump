<script setup lang="ts">
import { useRecipeStore } from '~/stores/recipe'
import { useUserStore } from '~/stores/user'

const user = useUserStore()
const recipe = useRecipeStore()
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader show-icons :title="$t('HomeNav')" />
    <div grow>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ user.username }}
      </h1>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="crimsonBtn">
        <div class="i-fa6-solid-box-tissue" />
      </div>
      <TextInput v-model:text="recipe.recipe.title" />
      <SearchSelect
        v-model:model="recipe.recipe.categories"
        :options="recipe.recipe.categories"
        mode="single"
      />
    </div>
    <MainButton mb-2 self-center color="orange" icon-type="create" title="Create recipe" />
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
