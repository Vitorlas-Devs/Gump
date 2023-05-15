<script setup lang="ts">
const props = defineProps<{
  currentRecipe?: Recipe
}>()

const category = useCategoryStore()

const categories = computed(() => {
  return props.currentRecipe?.categories.map(id => category.getCategoryNameById(id))
})
</script>

<template>
  <div v-if="currentRecipe" flex="~ col" rounded-b-4 px-4 pb-2 shadow-inner>
    <div flex="~ row" items-center justify-between>
      <p my-1>
        {{ `${$t('CreateVisibility')}:` }}
      </p>
      <p my-1>
        {{ currentRecipe.isPrivate ? $t('CreateVisibilityPrivateTitle') : $t('CreateVisibilityPublicTitle') }}
      </p>
    </div>
    <div flex="~ row" justify-between>
      <p my-1>
        {{ `${$t('CreateLanguage')}:` }}
      </p>
      <p my-1>
        {{ currentRecipe.language }}
      </p>
    </div>
    <div flex="~ col">
      <p>
        {{ `${$t('CreateCategory')}:` }}
      </p>
      <div flex="~ row" flex-wrap gap-2>
        <p v-for="cat in categories" :key="cat" my-1>
          {{ cat }}
        </p>
      </div>
    </div>
    <div flex="~ col">
      <p>
        {{ `${$t('CreateTags')}:` }}
      </p>
      <div flex="~ row" flex-wrap gap-2>
        <p v-for="tag in currentRecipe.tags" :key="tag" my-1>
          {{ `#${tag}` }}
        </p>
      </div>
    </div>
  </div>
</template>
