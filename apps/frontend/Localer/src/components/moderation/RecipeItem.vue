<script setup lang="ts">
import type { IBriefRecipe } from '@/stores/recipeStore'
import { onMounted, ref } from 'vue'
import SimpleButton from './SimpleButton.vue'
import { useGumpUserStore } from '@/stores/gumpUserStore'

const user = useGumpUserStore()

const props = defineProps<{
  recipe: IBriefRecipe
}>()

const imageUrl = ref(`${import.meta.env.VITE_BACKEND_URL}/image/${props.recipe.image}`)
const authorName = ref('')

onMounted(async () => {
  authorName.value = await user.getUser(props.recipe.author).then((u) => u.username)
})

const click = () => {
  console.log('click')
}
</script>

<template>
  <div flex="~" p="4" w="max" bg="orange-100" rounded="20px">
    <img :src="imageUrl" rounded="8px" />
    <div pl="4" h="full" flex="~ col" justify="between">
      <div>
        <p text="xl" font="bold" w="xs" mb="2">{{ recipe.title }}</p>
        <p>By: {{ authorName }}</p>
      </div>
      <div flex="~" justify="end">
        <SimpleButton color="orange-500" text="Modify" @click="click()" />
        <SimpleButton color="crimson-500" text="Delete" ml="4" />
      </div>
    </div>
  </div>
</template>
