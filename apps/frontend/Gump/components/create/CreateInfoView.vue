<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()

const visibilityData = computed(() => {
  return {
    Public: {
      icon: 'i-ph-users-three-fill',
      active: recipe.currentRecipe?.isPrivate === false,
    },
    Private: {
      icon: 'i-ph-lock-bold',
      active: recipe.currentRecipe?.isPrivate === true,
    },
  }
})

function checkDone() {
  if (recipe.currentRecipe) {
    if (recipe.currentRecipe.title.length > 0 && recipe.currentRecipe.language.length > 0)
      ui.createHeaderStates[0] = true
    else
      ui.createHeaderStates[0] = false
  }
}
</script>

<template>
  <div v-if="recipe.currentRecipe" flex="~ col" items-center justify-between gap-7>
    <div flex="~ col" items-center justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateTitle') }}
      </p>
      <TextInput
        v-model:text="recipe.currentRecipe.title"
        @update:text="checkDone()"
      />
    </div>
    <div flex="~ col" items-center justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateVisibility') }}
      </p>
      <div flex="~ col" gap-2>
        <div
          v-for="(visibility, key) in visibilityData"
          :key="key"
          flex="~ row" ml-2 items-center gap-3
        >
          <div
            flex="~ col" cursor-pointer items-center justify-between
            @click="recipe.currentRecipe.isPrivate = !recipe.currentRecipe.isPrivate"
          >
            <div
              h-10 w-10
              :class="visibility.active ? `${visibility.icon} text-orange-500` : `${visibility.icon} text-brown-500`"
            />
            <div
              :class="visibility.active ? 'visible' : 'invisible'"
              h-2 w-9 rounded-full bg-orange-500
            />
          </div>
          <div flex="~ col" justify-between>
            <p my-1 text-lg font-bold>
              {{ $t(`CreateVisibility${key}Title`) }}
            </p>
            <p my-1>
              {{ $t(`CreateVisibility${key}Desc`) }}
            </p>
          </div>
        </div>
      </div>
    </div>
    <div flex="~ col" items-center justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateLanguage') }}
      </p>
      <SearchSelect
        v-model:model="recipe.currentRecipe.language"
        :options="['English', 'French', 'Spanish']"
        mode="single"
        @update:model="checkDone()"
      />
    </div>
  </div>
</template>

<style scoped>

</style>
