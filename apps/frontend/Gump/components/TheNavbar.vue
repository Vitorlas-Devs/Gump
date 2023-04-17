<script setup lang="ts">
const localePath = useLocalePath()

const tabs = [
  {
    name: 'HomeNav',
    path: '/',
    icon: 'i-fa6-solid-house',
  },
  {
    name: 'SearchNav',
    path: '/search',
    icon: 'i-fa6-solid-magnifying-glass',
  },
  {
    name: 'ProfileNav',
    path: '/profile',
    icon: 'i-fa6-solid-user',
  },
]
</script>

<template>
  <div relative bottom-0 h-18 w-full flex flex-row gap-4 bg-crimson-50 px-3>
    <RouterLink v-for="tab in tabs" :key="tab.name" :to="localePath(tab.path)" w-full flex flex-col items-center gap-2 decoration-none>
      <div bg-crimson-200 p-2>
        <div :class="tab.icon" bg-crimson-500 text-2xl />
      </div>
      <div
        text-1xl text-center font-bold text-crimson-500 text-shadow-crimson
      >
        {{ $t(tab.name) }}
      </div>
    </RouterLink>
  </div>
</template>

<style scoped></style>
