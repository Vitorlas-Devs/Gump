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
      <RecipeBox title="Nagyon nagyon nagyon naon fincsi pogi" :author-id="ui.currentRecipe.author" :img-id="ui.currentRecipe.image" />
      <RecipeBox title="Nagyon nagyon nagyon naon fincsi pogi" :author-id="ui.currentRecipe.author" :img-id="ui.currentRecipe.image" />
      <br><br>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="crimsonBtn">
        <div class="i-fa6-solid-box-tissue" />
      </div>
      <TextInput v-model:text="ui.currentRecipe.title" />
      <SearchSelect
        v-model:model="ui.currentRecipe.tags"
        :options="ui.currentRecipe.tags"
        mode="tags"
      />
      <MainButton fixed bottom-26 left="1/2" transform="translate-x--1/2" color="orange" icon-type="create" title="Create recipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
