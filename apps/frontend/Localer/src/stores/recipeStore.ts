import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'
import { useFetch } from '@vueuse/core'

interface IBriefRecipe {}

export const useRecipeStore = defineStore(
  'recipe',
  () => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const state = reactive({
      recipes: [] as IBriefRecipe[]
    })

    const getRecipes = () => {
      const { data } = useFetch(`${backendUrl}/recipe`)
    }

    return {
      ...toRefs(state)
    }
  },
  {
    persist: true
  }
)
