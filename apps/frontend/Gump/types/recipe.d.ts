type Recipe = {
  id: number
  title: string
  author: number
  image: number
  language: string
  serves: number
  categories: string[]
  tags: string[]
  ingredients: Ingredient[]
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

type Ingredient = {
  name: string
  value: number
  volume: string
  linkedRecipe: number
}

type IngredientCreate = Omit<Ingredient, 'linkedRecipe'>
