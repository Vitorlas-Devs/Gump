export interface IIngredient {
  name: string
  value: number
  volume: string
  linkedRecipe: number
}

export interface IRecipe {
  id: number
  title: string
  author: number
  image: number
  language: string
  serves: number
  categories: string[]
  tags: string[]
  ingredients: IIngredient[]
  steps: string[]
  viewCount: number
  saveCount: number
  isLiked: boolean
  likeCount: number
  referenceCount: number
  isArchived: boolean
  isOriginal: boolean
  originalRecipe: number
  isPrivate: boolean
  forks: number[]
}

interface IRecipeState {
  recipe: IRecipe
}

export const useRecipeStore = defineStore('recipe', () => {
  // state
  const state = reactive<IRecipeState>({
    recipe: {
      id: 0,
      title: '',
      author: 0,
      image: 0,
      language: '',
      serves: 0,
      categories: [],
      tags: [],
      ingredients: [],
      steps: [],
      viewCount: 0,
      saveCount: 0,
      isLiked: false,
      likeCount: 0,
      referenceCount: 0,
      isArchived: false,
      isOriginal: false,
      originalRecipe: 0,
      isPrivate: false,
      forks: [],
    },
  })

  // getters
  // ...

  // actions
  // ...

  return {
    ...toRefs(state),
  }
},
// {
//   persist: true,
// }
)
