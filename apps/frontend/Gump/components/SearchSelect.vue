<script setup lang="ts">
import Multiselect from '@vueform/multiselect'

const prop = defineProps<{
  model: string[]
  options: string[]
  mode: 'single' | 'multiple'
}>()

const emit = defineEmits<{
  (event: 'update:model', ...args: any[]): void
}>()

function addTag(tag: string) {
  if (prop.mode === 'multiple')
    emit('update:model', [...prop.model, tag])
  else if (prop.mode === 'single')
    emit('update:model', [tag])
}

function removeTag(tag: string) {
  if (prop.mode === 'multiple')
    emit('update:model', prop.model.filter(t => t !== tag))
}

function handleBackspace(e: KeyboardEvent) {
  if (e.key === 'Backspace')
    emit('update:model', prop.model.slice(0, -1))
}

const option = ['pizza', 'pasta', 'salad', 'soup', 'dessert', 'drink']
</script>

<template>
  <div mx-2 w-60>
    <Multiselect
      :value="model"
      :options="mode === 'multiple' ? options : option"
      :multiple="mode === 'multiple'"
      :mode="mode === 'multiple' ? 'tags' : 'single'"
      :taggable="mode === 'multiple'"
      :create-option="mode === 'multiple'"
      :show-options="mode === 'single'"
      :searchable="true" :append-new-option="false"
      class="select"
      @tag="addTag"
      @select="addTag"
      @deselect="removeTag"
      @keydown="handleBackspace"
      @clear="$emit('update:model', [])"
    />
  </div>
</template>

<style scoped>
.select {
  --ms-bg: transparent;
  --ms-radius: 10px;
  --ms-border-width: 0px;
  --ms-border-width-active: 0px;
  --ms-ring-color: #F3582730;

  --ms-spinner-color: #912808;
  --ms-caret-color: #912808;
  --ms-clear-color: #912808;
  --ms-clear-color-hover: #2A1700;

  --ms-tag-bg: #FCCAC0;
  --ms-tag-color: #912808;
  --ms-tag-radius: 9999px;

  --ms-dropdown-bg: transparent;
  --ms-dropdown-border-color: #F3582730;
  --ms-dropdown-border-width: 1px;
  --ms-dropdown-radius: 10px;

  --ms-option-bg-pointed: #FEE5E3;
  --ms-option-color-pointed: #912808c2;
  --ms-option-bg-selected: #fccac0c2;
  --ms-option-color-selected: #912808;
  --ms-option-bg-selected-pointed: #fccac0c2;
  --ms-option-color-selected-pointed: #912808c2;

  box-shadow: inset 0px 2px 12px -4px rgb(243, 88, 39);
}
</style>

<style src="@vueform/multiselect/themes/default.css"></style>
