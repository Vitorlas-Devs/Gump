import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

export interface IBriefRecipe {
  id: number
  title: string
  author: number
  image: number
  viewCount: number
  saveCount: number
  isPrivate: boolean
}

export interface IDisplayRecipe {
  id: number
  title: string
}

export interface IUser {}

export const useRecipeStore = defineStore(
  'recipe',
  () => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const state = reactive({
      recipes: [] as IBriefRecipe[]
    })

    const fetchRecipes = async () => {
      const data: IBriefRecipe[] = await fetch(
        `${backendUrl}/recipe/search?sort=new&offset=${state.recipes.length}`
      ).then((res) => res.json())
      state.recipes.push(...data)
    }

    return {
      ...toRefs(state),
      fetchRecipes
    }
  },
  {
    persist: true
  }
)
