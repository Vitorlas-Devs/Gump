<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()
const user = useUserStore()
const category = useCategoryStore()

category.get(1)

function checkDone() {
  if (recipe.currentRecipe) {
    if (recipe.currentRecipe.title.length > 0 && recipe.currentRecipe.language.length > 0)
      ui.createHeaderStates[3] = true
    else
      ui.createHeaderStates[3] = false
  }
}

const serves = computed({
  get: () => {
    return recipe.currentRecipe!.serves.toString()
  },
  set: (value: string) => {
    recipe.currentRecipe!.serves = parseInt(value) || 0
  },
})

// return recipe.currentRecipe!.visibleTo
// return as a list of strings (visibleTo is a list of user ids) so get user names from ids
const visibleTo = computed({
  get: () => {
    return recipe.currentRecipe!.visibleTo
  },
  set: (value: number[]) => {
    recipe.currentRecipe!.visibleTo = value
  },
})

// computed to get usernames from ids with user.getAuthorNameById(id)
// for set, use
// const visibleToNames = computed(() => {
//   get: () => {
//     return visibleTo.value.map(id => user.getAuthorNameById(id))
//   },
//   set: (value: string[]) => {
//   },
// })

// const categories = computed({
//   get: () => {
//     // for category ids, get category names
//     return recipe.currentRecipe!.categories.map(id => category.getCategoryNameById(id))
//   },
//   set: (value: string[]) => {
//     recipe.currentRecipe!.categories = value
//   },
// })

const categories = ['0', '1', '2']
</script>

<template>
  <div v-if="recipe.currentRecipe" flex="~ col" mx-4 items-start justify-between gap-7>
    <div flex="~ col" justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateServingsTitle') }}
      </p>
      <p my-1>
        {{ $t('CreateServingsDesc') }}
      </p>
      <TextInput
        v-model:text="serves"
        type="number"
        @update:text="checkDone(); serves = $event"
      />
    </div>
    <div flex="~ col" justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateManageAccessTitle') }}
      </p>
      <p my-1>
        {{ $t('CreateManageAccessDesc') }}
      </p>
      <!-- <SearchSelect
        v-model:model="visibleTo"
        mode="tags"
        @update:model="checkDone()"
      /> -->
    </div>
    <div flex="~ col" justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateCategory') }}
      </p>
      <SearchSelect
        v-model:model="categories"
        :options="['0', '1', '2']"
        mode="single"
        @update:model="checkDone()"
      />
    </div>
  </div>
</template>

<style scoped>

</style>
