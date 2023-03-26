<script setup lang="ts">
import { defineAsyncComponent } from 'vue'

const props = defineProps<{
  icon: string
  class: string
}>()

const icon = defineAsyncComponent(() => import(`@/components/icons/${props.icon}.svg`))
</script>

<template>
  <component :is="icon" :class="props.class" />
</template>

<style scoped></style>
