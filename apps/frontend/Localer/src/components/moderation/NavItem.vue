<script setup lang="ts">
import router from '@/router'
import { ref } from 'vue'

const props = defineProps<{
  value: string
}>()

const isSelected = ref(() => {
  return (
    router.currentRoute.value.fullPath.split('/moderation')[1].replace('/', '') ===
    props.value.toLowerCase()
  )
})

const href = ref(() => {
  return `/moderation/${props.value.toLowerCase()}`
})
</script>

<template>
  <RouterLink
    :to="href()"
    class="h-10 w-xs flex items-center font-bold"
    :pl="isSelected() ? '1' : '3'"
  >
    <Transition name="fade" mode="out-in">
      <SvgIcon
        v-if="isSelected()"
        icon="caret-right-solid"
        class="icon-crimson"
        w="5"
        h="9"
        mr="2"
      />
    </Transition>
    <p :class="isSelected() ? 'underline underline-2 underline-offset-5' : ''">
      {{ value }}
    </p>
  </RouterLink>
</template>

<style scoped>
.fade-enter-active {
  transition: opacity 0.5s ease, transform 0.5s ease;
}

.fade-enter-from {
  opacity: 0;
  transform: translateX(-10px);
}
</style>
