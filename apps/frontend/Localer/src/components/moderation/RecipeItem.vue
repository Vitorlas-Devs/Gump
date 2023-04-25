<script setup lang="ts">
import type { IBriefRecipe } from '@/stores/recipeStore'
import { onMounted, ref } from 'vue'
import { useGumpUserStore } from '@/stores/gumpUserStore'
import SimpleButton from './SimpleButton.vue'
import VueSelect from 'vue-select'

const user = useGumpUserStore()
const state = ref('default')

const props = defineProps<{
  recipe: IBriefRecipe
}>()

const imageUrl = ref(`${import.meta.env.VITE_BACKEND_URL}/image/${props.recipe.image}`)
const authorName = ref('')

onMounted(async () => {
  authorName.value = await user.getUser(props.recipe.author).then((u) => u.username)
})
</script>

<template>
  <div>
    <div v-if="state === 'default'" flex="~" p="4" bg="orange-100" rounded="20px">
      <img :src="imageUrl" w="60" h="40" object="cover" rounded="8px" />
      <div pl="4" h="40" flex="~ col" justify="between">
        <div>
          <p text="xl" font="bold" w="80" mb="2">{{ recipe.title }}</p>
          <p>By: {{ authorName }}</p>
        </div>
        <div flex="~" justify="end">
          <SimpleButton type="solid" color="orange-500" text="Modify" @click="state = 'modify'" />
          <SimpleButton
            type="text"
            color="crimson-500"
            text="Delete"
            ml="4"
            @click="state = 'delete'"
          />
        </div>
      </div>
    </div>
    <div v-if="state === 'modify'" flex="~ col" p="4" w="152" bg="orange-100" rounded="20px">
      <p text="xl" font="bold" w="80" mb="4">{{ recipe.title }}</p>
      <div flex="~" justify="between" items="center" mb="4">
        <div w="25" align="right">
          <label for="modifyImage" text="20px">Image</label>
        </div>
        <div flex="~" w="115">
          <input
            id="modifyImage"
            type="text"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
            disabled
          />
          <SimpleButton type="text" color="crimson-500" text="Delete" ml="4" />
        </div>
      </div>
      <div flex="~" justify="between" items="center" mb="4">
        <div w="25" align="right">
          <label for="modifyTags" text="20px" align="right">Tags</label>
        </div>
        <div flex="~" w="115">
          <VueSelect
            id="modifyTags"
            class="select"
            w="full"
            shadow="inner"
            rounded="8px"
            p="2"
            bg="white"
            :multiple="true"
            :clear-on-select="false"
            :close-on-select="false"
            :taggable="true"
            :no-options="true"
          >
            <template #no-options>Start typing to add tags</template>
          </VueSelect>
        </div>
      </div>
      <div flex="~" justify="between" items="start" mb="4">
        <div flex="~ col" justify="center" w="25" h="40px" align="right">
          <label for="modifyIngredients" text="20px" align="right">Ingredients</label>
        </div>
        <div flex="~ col" gap="2">
          <div flex="~" w="115">
            <input id="modifyIngredients" type="text" w="full" shadow="inner" rounded="8px" p="2" />
            <SimpleButton type="text" color="crimson-500" text="Delete" ml="4" />
          </div>
          <div flex="~" w="115">
            <input id="modifyIngredients" type="text" w="full" shadow="inner" rounded="8px" p="2" />
            <SimpleButton type="text" color="crimson-500" text="Delete" ml="4" />
          </div>
          <div flex="~" w="115">
            <input id="modifyIngredients" type="text" w="full" shadow="inner" rounded="8px" p="2" />
            <SimpleButton type="text" color="crimson-500" text="Delete" ml="4" />
          </div>
        </div>
      </div>
      <div flex="~" justify="between" items="center" mb="4">
        <div w="25" align="right">
          <label for="modifySteps" text="20px" align="right">Steps</label>
        </div>
        <div flex="~" w="115">
          <input id="modifySteps" type="text" w="full" shadow="inner" rounded="8px" p="2" />
          <SimpleButton type="text" color="crimson-500" text="Delete" ml="4" />
        </div>
      </div>
      <div flex="~" justify="end">
        <SimpleButton
          type="text"
          color="crimson-500"
          text="Cancel"
          ml="4"
          @click="state = 'default'"
        />
        <SimpleButton type="solid" color="orange-500" text="Modify" ml="4" />
      </div>
    </div>
    <div v-if="state === 'delete'" flex="~" p="4" w="max" bg="orange-100" rounded="20px"></div>
  </div>
</template>

<style scoped>
.select {
  --vs-search-input-bg: rgb(243, 88, 39);
  --vs-controls-color: rgb(151, 39, 4);
  --vs-selected-bg: transparent;
  --vs-selected-color: rgb(151, 39, 4);
  --vs-selected-border-color: rgba(151, 39, 4, 0.5);
  --vs-border-width: 0px;
  --vs-border-radius: 20px;
  --vs-dropdown-option-bg: rgb(243, 88, 39);
  --vs-dropdown-option--active-bg: rgb(243, 88, 39);
}
</style>
