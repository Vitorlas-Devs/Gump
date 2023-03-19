<!-- <script setup lang="ts">
import { computed } from 'vue'
import { useTranslationStore } from '@/stores/translationStore'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'


import { useTextareaAutosize } from '@vueuse/core'
const { textarea } = useTextareaAutosize()

const router = useRouter()

const translate = useTranslationStore()

const { locales, checkDirty, initialTranslations } = translate
const selectedKey = computed(() => router.currentRoute.value.params.key.toString())
const { translations } = storeToRefs(translate)



// const CustomTextarea = defineComponent({
//   name: 'CustomTextarea',
//   emits: ['update:modelValue'],
//   setup(props, { emit }) {
//     const { textarea } = useTextareaAutosize()

//     const input = props.modelValue
//     const updateInput = (value) => {
//       emit('update:modelValue', value)
//     }

//     return {
//       textarea,
//       input,
//       updateInput,
//     }
//   },
// })
</script>
        
<template>
    <div>
        <textarea
        id="translationArea"
        v-model="translations[locale][selectedKey]"
        type="text"
        class="rounded flex-grow p-3 shadow-inner bg-crimson-50 rounded-3xl min-h-12 h-max"
        :readonly="locale === 'en_US'"
        @change="checkDirty()"
      ></textarea>
    </div>
</template>

<style scoped>

</style> -->