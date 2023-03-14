<script setup lang="ts"></script>

<template>
  <div class="greetings">
    <h1 class="locale-title">
      {{ $t('ui.HomeButton') }}
    </h1>
    <h3 class="title">$i18n.availableLocales: {{ $i18n.availableLocales }}</h3>
    <select v-model="$i18n.locale" class="locale-changer">
      <option v-for="locale in $i18n.availableLocales" :key="locale" :value="locale">
        {{ locale }}
      </option>
    </select>
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

@media (min-width: 1024px) {
  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style>
