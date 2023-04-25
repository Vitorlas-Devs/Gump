<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

const ui = useUIStore()
</script>

<template>
  <div h-full w-full flex="~ col">
    <div flex="~ row" items-center justify-between>
      <p my-1>
        {{ `${$t('CreateVisibility')}:` }}
      </p>
      <p my-1>
        {{ ui.currentRecipe.isPrivate ? $t('CreateVisibilityPrivateTitle') : $t('CreateVisibilityPublicTitle') }}
      </p>
    </div>
    <div flex="~ row" justify-between>
      <p my-1>
        {{ `${$t('CreateLanguage')}:` }}
      </p>
      <p my-1>
        {{ ui.currentRecipe.language }}
      </p>
    </div>
    <div flex="~ col">
      <p>
        {{ `${$t('CreateCategory')}:` }}
      </p>
      <div flex="~ row" flex-wrap gap-2>
        <p v-for="category in ui.currentRecipe.categories" :key="category" my-1>
          {{ category }}
        </p>
      </div>
    </div>
    <div flex="~ col">
      <p>
        {{ `${$t('CreateTags')}:` }}
      </p>
      <div flex="~ row" flex-wrap gap-2>
        <p v-for="tag in ui.currentRecipe.tags" :key="tag" my-1>
          {{ `#${tag}` }}
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
