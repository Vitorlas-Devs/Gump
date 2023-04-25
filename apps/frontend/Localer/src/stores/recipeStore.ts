import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'
import type { IGumpUserData } from './gumpUserStore'

export interface IBriefRecipe {
  id: number
  title: string
  author: number
  image: number
  viewCount: number
  saveCount: number
  isPrivate: boolean
}

export interface IRecipe {
  id: number
  image: number
  serves: number
  categories: number[]
  tags: string[]
  ingredients: IIngredient[]
  steps: string[]
  isPrivate: boolean
  visibleTo: number[]
}

export interface IIngredient {
  name: string
  value: number | null
  volume: string | null
  linkedRecipe: number | null
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

    const getRecipe = async (id: number) => {
      const data: IRecipe = await fetch(`${backendUrl}/recipe/${id}`).then((res) => res.json())
      return data
    }

    const updateRecipe = async (recipe: IRecipe) => {
      await fetch(`${backendUrl}/recipe/update`, {
        method: 'PATCH',
        body: JSON.stringify(recipe),
        headers: {
          'Content-type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        }
      })
    }

    const deleteRecipe = async (id: number) => {
      await fetch(`${backendUrl}/recipe/delete/${id}`, {
        method: 'DELETE',
        headers: {
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        }
      })
    }

    return {
      ...toRefs(state),
      fetchRecipes,
      getRecipe,
      updateRecipe,
      deleteRecipe
    }
  },
  {
    persist: true
  }
)
