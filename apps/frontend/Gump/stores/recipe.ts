export const emptyIngredient: Ingredient = {
  name: '',
  value: 0,
  volume: '',
  linkedRecipe: null,
}

export const emptyRecipe: Recipe = {
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
  isSaved: false,
  saveCount: 0,
  isLiked: false,
  likeCount: 0,
  referenceCount: 0,
  isArchived: false,
  isOriginal: false,
  originalRecipe: 0,
  isPrivate: false,
  forks: [],
}

export const useRecipeStore = defineStore('recipe', {
  state: () => ({
    recipes: [] as Recipe[],
    searchRecipes: [] as SearchRecipe[],
    ingredients: [] as Ingredient[],
    currentRecipe: null as Recipe | null,
  }),
  getters: {
    getEmptyIngredients(): Ingredient[] {
      return this.ingredients.filter(ingredient => ingredient.name === '' && !ingredient.value && ingredient.volume === '')
    },
  },
  actions: {
    async getSearchRecipes(searchTerm: string): Promise<SearchRecipe[] | undefined> {
      const { data, error } = await gumpFetch<SearchRecipe[]>(`recipe/search?sort=${searchTerm}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value)
        this.searchRecipes = data.value
      if (error.value)
        return error.value
    },
    addEmptyIngredient(id?: number) {
      this.currentRecipe?.ingredients.push({
        name: '',
        value: 0,
        volume: '',
        linkedRecipe: id || null,
      })
    },
    addEmptyStep(index?: number) {
      if (this.currentRecipe) {
        if (index !== undefined)
          this.currentRecipe.steps.splice(index + 1, 0, '')
        else
          this.currentRecipe.steps.push('')
      } else {
        this.currentRecipe = emptyRecipe
        this.currentRecipe.steps.push('')
      }
    },
    checkEmptyIngredients() {
      if (this.getEmptyIngredients.length > 0) {
        this.getEmptyIngredients.forEach((ingredient) => {
          const index = this.ingredients.indexOf(ingredient)
          if (index > -1)
            this.ingredients.splice(index, 1)
        })
      }
    },
    removeStep(index: number) {
      if (this.currentRecipe)
        this.currentRecipe.steps.splice(index, 1)
    },
    removeIngredient(index: number) {
      if (this.currentRecipe)
        this.currentRecipe.ingredients.splice(index, 1)
    },
    addRecipe(recipe: Recipe) {
      this.currentRecipe?.ingredients.push({
        name: recipe.title,
        value: recipe.serves,
        volume: 'adag',
        linkedRecipe: recipe.id,
      })
    },
    searchRecipes(query: string): Recipe[] {
      return this.recipes.filter(recipe => recipe.title.toLowerCase().includes(query.toLowerCase()))
    },
    async likeRecipe(recipeId: number) {
      // const recipeToModify = recipe.recipes.find(r => r.id === props.id)

      // if (recipeToModify) {
      //   if (props.isLiked) {
      //     recipeToModify.likeCount--
      //     user.likes = user.likes.filter(l => l !== props.id)
      //   } else {
      //     recipeToModify.likeCount++
      //     user.likes.push(props.id)
      //   }
      // }
      const user = useUserStore()

      const { data, error } = await gumpFetch(`recipe/like/${recipeId}`, {
        method: 'PATCH',
      })

      if (data.value) {
        const recipeToModify = this.recipes.find(r => r.id === recipeId)
        if (recipeToModify) {
          if (recipeToModify.isLiked) {
            recipeToModify.likeCount--
            recipeToModify.isLiked = false
            user.current.likes = user.current.likes.filter(l => l !== recipeId)
          } else {
            recipeToModify.likeCount++
            recipeToModify.isLiked = true
            user.current.likes.push(recipeId)
          }
        }
        return data.value
      }

      if (error.value)
        return error.value
    },
    async saveRecipe(recipeId: number) {
      const { data, error } = await gumpFetch(`recipe/save/${recipeId}`, {
        method: 'PATCH',
      })
      if (data.value)
        return data.value

      if (error.value)
        return error.value
    },
    async getRecipeById(recipeId: number): Promise<Recipe | undefined> {
      const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value)
        return data.value

      if (error.value)
        return error.value
    },
  },
  persist: true,
})
