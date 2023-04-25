<script setup lang="ts">

</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('CreateNav')" />
    <div grow>
      <CreateSubHeader variant="ingredients" />
      <IngredientsView />
    </div>
    <TheNavbar />
  </ion-page>
</template>
