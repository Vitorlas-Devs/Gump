<script setup lang="ts">

</script>

<template>
  <ion-page bg-crimson-50>
    <TheHeader :title="$t('CreateNav')" />
    <div grow>
      <h1 text-crimson-500 text-shadow-crimson>
        {{ $t('CreateNav') }}
      </h1>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil voluptatum similique explicabo suscipit eum alias doloremque quidem iure corrupti pariatur. Explicabo neque assumenda ipsa, error accusamus vel recusandae aliquid doloremque.
      <br>
      <div mt-5 class="btnCrimson">
        <div class="i-fa6-solid-box-tissue" />
      </div>
    </div>
    <TheNavbar />
  </ion-page>
</template>
