export const emptyIngredient: Ingredient = {
  name: '',
  value: 0,
  volume: '',
  linkedRecipe: 0,
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
    cachedRecipes: {} as Record<Sort, Recipe[]>,
  }),
  getters: {
    getEmptyIngredients(): Ingredient[] {
      return this.ingredients.filter(ingredient => ingredient.name === '' && !ingredient.value && ingredient.volume === '')
    },
  },
  actions: {
    async getRecipes(sort: Sort): Promise<Recipe[] | undefined> {
      const user = useUserStore()

      const { data, error } = await gumpFetch<Recipe[]>(`recipe/search?sort=${sort}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value) {
        this.cachedRecipes[sort] = data.value
        this.recipes = data.value
        this.recipes.forEach((recipe) => {
          recipe.isLiked = user.current.likes.includes(recipe.id)
          recipe.isSaved = user.current.recipes.includes(recipe.id)
        })
        return this.recipes
      }

      if (error.value)
        return error.value
    },
    addEmptyIngredient(id?: number) {
      this.currentRecipe?.ingredients.push({
        name: '',
        value: 0,
        volume: '',
        linkedRecipe: id || 0,
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
    search(query: string): Recipe[] {
      return this.recipes.filter(recipe => recipe.title.toLowerCase().includes(query.toLowerCase()))
    },
    async likeRecipe(recipeId: number) {
      const { data, error } = await gumpFetch(`recipe/like/${recipeId}`, {
        method: 'PATCH',
      })
      if (data.value)
        return data.value

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
      const recipe = this.recipes.find(r => r.id === recipeId)
      if (recipe) {
        return recipe
      } else {
        const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
          headers: {},
          method: 'GET',
        }).json()
        if (data.value) {
          const user = useUserStore()

          data.value.isLiked = user.current.likes.includes(data.value.id)

          this.recipes.push(data.value)
          return data.value
        }

        if (error.value)
          return error.value
      }
    },
    async getUserRecipes(type: 'owned' | 'liked' | 'saved'): Promise<Recipe[]> {
      const user = useUserStore()
      const recipes: Recipe[] = []
      if (type !== 'liked') {
        for (const recipeId of user.current.recipes) {
          const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
            headers: {},
            method: 'GET',
          }).json()
          if (data.value) {
            if (type === 'owned' && data.value.author === user.current.id)
              recipes.push(data.value)
            else if (type === 'saved' && data.value.author !== user.current.id)
              recipes.push(data.value)
          }
          if (error.value)
            return error.value
        }
      } else {
        for (const recipeId of user.current.likes) {
          const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
            headers: {},
            method: 'GET',
          }).json()
          if (data.value)
            recipes.push(data.value)
          if (error.value)
            return error.value
        }
      }
      return recipes
    },
  },
  persist: true,
})
