<script setup lang="ts">
import { useUIStore } from '~/stores/ui'
import { useUserStore } from '~/stores/user'

const user = useUserStore()
const ui = useUIStore()
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader show-icons :title="$t('HomeNav')" />
    <div grow overflow-y-auto pb-30>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ user.username }}
      </h1>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="crimsonBtn">
        <div class="i-fa6-solid-box-tissue" />
      </div>
      <TextInput v-model:text="ui.currentRecipe.title" />
      <SearchSelect
        v-model:model="ui.currentRecipe.categories"
        :options="ui.currentRecipe.categories"
        mode="multiple"
      />
      <br><br>
      <RecipeBox title="Nagyon nagyon nagyon naon fincsi pogi" :author-id="ui.currentRecipe.author" :img-id="ui.currentRecipe.image" />
      <RecipeBox title="Nagyon nagyon nagyon naon fincsi pogi" :author-id="ui.currentRecipe.author" :img-id="ui.currentRecipe.image" />
      <MainButton fixed bottom-26 left="1/2" transform="translate-x--1/2" color="orange" icon-type="create" title="Create recipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
