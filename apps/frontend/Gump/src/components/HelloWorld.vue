<script setup lang="ts">
import LocaleSelect from '../components/LocaleSelect.vue'

const usermame = 'Rettend'
</script>

<template>
  <div class="greetings">
    <div display="flex" justify-content="space-between" gap="10px">
      <h1 class="title-secondary">{{ $t('ui.HomeButton') }}:</h1>
      <h1 class="title-welcome" v-html="$t('ui.Welcome', { username: usermame })"></h1>
    </div>
    <h3 class="title">$i18n.availableLocales: {{ $i18n.availableLocales }}</h3>
    <LocaleSelect />
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
