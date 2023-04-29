<script setup lang="ts">
const { currentRecipe } = toRefs(useRecipeStore())
</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader show-icons :title="$t('HomeNav')" />
    <div v-if="currentRecipe" grow overflow-y-auto pb-30>
      <RecipeBox title="Nagyon nagyon nagyon naon fincsi pogi" :author-id="currentRecipe.author" :img-id="currentRecipe.image" />
      <RecipeBox title="Nagyon nagyon nagyon naon fincsi pogi" :author-id="currentRecipe.author" :img-id="currentRecipe.image" />
      <br><br>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="crimsonBtn">
        <div class="i-fa6-solid-box-tissue" />
      </div>
      <TextInput v-model:text="currentRecipe.title" />
      <SearchSelect
        v-model:model="currentRecipe.tags"
        :options="currentRecipe.tags"
        mode="tags"
      />
      <MainButton fixed color="orange" icon-type="create" title="Create recipe" />
    </div>
    <TheNavbar />
  </ion-page>
</template>

<style scoped>

</style>
