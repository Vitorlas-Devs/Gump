<script setup lang="ts">
defineProps<{
  badge: Badge
}>()

const badgeColors: Record<number, string> = {
  1: 'bg-orange-500',
  2: 'bg-yellow-500',
  3: 'bg-crimson-500',
  5: 'bg-purple-500',
  4: 'bg-blue-500',
  6: 'bg-green-500',
  7: 'bg-blue-200',
  8: 'bg-pink-500',
}
</script>

<template>
  <div flex="~ row" items-center>
    <div i-shadow:badge h-20 w-20 :class="badgeColors[badge.id] || 'bg-gray-500'" />
    <div flex="~ col" flex-1>
      <div text-lg font-bold>
        {{ $t(badge.name) }}
      </div>
      <p my-0>
        {{ $t(badge.description) }}
      </p>
    </div>
  </div>
</template>
