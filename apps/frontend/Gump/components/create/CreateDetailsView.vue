<script setup lang="ts">
const recipe = useRecipeStore()
const ui = useUIStore()
const user = useUserStore()
const category = useCategoryStore()
const image = useImageStore()

const { files, open, reset, onChange } = useFileDialog()

onChange((fileList) => {
  if (fileList && fileList.length > 0)
    image.uploadImage(fileList)
})

const serves = computed({
  get: () => {
    return recipe.currentRecipe!.serves.toString()
  },
  set: (value: string) => {
    recipe.currentRecipe!.serves = parseInt(value) || 0
  },
})

const visibleTo = computed({
  get: () => {
    return recipe.currentRecipe!.visibleTo.map(id => user.getUserNameById(id))
  },
  set: (value: string[]) => {
    recipe.currentRecipe!.visibleTo = value.map(name => user.getUserIdByName(name))
  },
})

const categories = computed({
  get: () => {
    return recipe.currentRecipe!.categories.map(id => category.getCategoryNameById(id))
  },
  set: (value: string[]) => {
    recipe.currentRecipe!.categories = value.map(name => category.getCategoryIdByName(name))
  },
})

const categoryOptions = computed(() => {
  return category.all.map(c => c.name)
})

function checkDone() {
  if (recipe.currentRecipe) {
    if (recipe.currentRecipe.serves > 0 && recipe.currentRecipe.categories.length > 0 && recipe.currentRecipe.tags.length > 0 && recipe.currentRecipe.image)
      ui.createHeaderStates[3] = true
    else
      ui.createHeaderStates[3] = false
  }
}

watch(() => recipe.currentRecipe?.image, () => {
  checkDone()
}, { immediate: true, deep: true })
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
    <div
      v-if="recipe.currentRecipe.isPrivate"
      flex="~ col" w-full justify-between gap-2
    >
      <p my-1 text-xl font-bold>
        {{ $t('CreateManageAccessTitle') }}
      </p>
      <p my-1>
        {{ $t('CreateManageAccessDesc') }}
      </p>
      <SearchSelect
        v-model:model="visibleTo"
        :options="user.all.map(u => u.username)"
        mode="multiple"
        :query-function="user.searchUserName"
        @update:model="checkDone()"
      />
    </div>
    <div flex="~ col" w-full justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateCategory') }}
      </p>
      <SearchSelect
        v-model:model="categories"
        :options="categoryOptions"
        mode="multiple"
        :query-function="category.getAllString"
        @update:model="checkDone()"
      />
    </div>
    <div flex="~ col" w-full justify-between gap-2>
      <p my-1 text-xl font-bold>
        {{ $t('CreateTags') }}
      </p>
      <SearchSelect
        v-model:model="recipe.currentRecipe.tags"
        :options="recipe.currentRecipe.tags"
        mode="tags"
        @update:model="checkDone()"
      />
    </div>
    <div flex="~ col" w-full justify-between gap-2>
      <div flex="~ row" items-center space-x-xl>
        <p my-1 text-4xl>
          📸
        </p>
        <p v-if="files && files.length > 0" my-1 mb-2 self-end font-bold>
          {{ files[0].name }}
        </p>
        <p
          v-else-if="recipe.currentRecipe.image" my-1 mb-2 self-end font-bold
        >
          image id: {{ recipe.currentRecipe.image }}
        </p>
      </div>
      <div flex="~ row" w-full space-x-xl>
        <button type="button" class="orangeBtn" @click="open({ multiple: false, accept: 'image/png' })">
          Choose image
        </button>
        <div class="crimsonBtn">
          <div class="i-ph-camera-slash-bold whiteIcon" @click="reset(); recipe.currentRecipe.image = 0; checkDone()" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
