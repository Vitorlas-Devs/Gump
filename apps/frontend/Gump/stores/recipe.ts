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
  visibleTo: [],
}

export const useRecipeStore = defineStore('recipe', {
  state: () => ({
    recipes: [] as Recipe[],
    searchRecipes: [] as SearchRecipe[],
    ingredients: [] as Ingredient[],
    currentRecipe: undefined as Recipe | undefined,
    cachedRecipes: {} as Record<Sort, Recipe[]>,
  }),
  getters: {
    getEmptyIngredients(): Ingredient[] {
      return this.ingredients.filter(ingredient => ingredient.name === '' && !ingredient.value && ingredient.volume === '')
    },
  },
  actions: {
    async getRecipesBySort(sort: Sort): Promise<Recipe[] | undefined> {
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
    async searchRecipes(searchTerm: string): Promise<Recipe[] | undefined> {
      const { data, error } = await gumpFetch<Recipe[]>(`recipe/search?searchTerm=${searchTerm}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value) {
        this.recipes = data.value
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
        value: 1,
        volume: 'piece',
        linkedRecipe: recipe.id,
      })
    },
    search(query: string): Recipe[] {
      return this.recipes.filter(recipe => recipe.title.toLowerCase().includes(query.toLowerCase()))
    },
    async likeRecipe(recipeId: number) {
      for (let i = 0; i < 2; i++) {
        const { data, error, statusCode } = await gumpFetch(`recipe/like/${recipeId}`, {
          method: 'PATCH',
        })
        if (data.value)
          return data.value

        const user = useUserStore()
        if (i === 0 && user.current.token !== 'offline' && statusCode.value === 401) {
          await user.login({
            username: user.current.username,
            password: user.current.password,
          })
          continue
        }

        if (error.value)
          return error.value
      }
    },
    async saveRecipe(recipeId: number) {
      for (let i = 0; i < 2; i++) {
        const { data, error, statusCode } = await gumpFetch(`recipe/save/${recipeId}`, {
          method: 'PATCH',
        })
        if (data.value)
          return data.value

        const user = useUserStore()
        if (i === 0 && user.current.token !== 'offline' && statusCode.value === 401) {
          await user.login({
            username: user.current.username,
            password: user.current.password,
          })
          continue
        }

        if (error.value)
          return error.value
      }
    },
    async getRecipeById(recipeId: number): Promise<Recipe | undefined> {
      const recipe = this.recipes.find(r => r.id === recipeId)
      if (recipe && recipe.ingredients) {
        return recipe
      } else {
        const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
          headers: {},
          method: 'GET',
        }).json()
        if (data.value) {
          const user = useUserStore()

          data.value.isLiked = user.current.likes.includes(data.value.id)
          data.value.isSaved = user.current.recipes.includes(data.value.id)
          data.value.visibleTo = []

          if (recipe)
            this.recipes.splice(this.recipes.indexOf(recipe), 1, data.value)
          else
            this.recipes.push(data.value)

          // for every ingredient in the recipe, check if it has a non-zero linkedRecipe and call getRecipeById on them
          data.value.ingredients.forEach(async (ingredient: Ingredient) => {
            if (ingredient.linkedRecipe)
              await this.getRecipeById(ingredient.linkedRecipe)
          })

          return data.value
        }

        if (error.value)
          return error.value
      }
    },
    async getCurrentUserRecipes(type: RecipesSort): Promise<Recipe[]> {
      const user = useUserStore()
      const recipes: Recipe[] = []
      if (type !== 'Liked') {
        for (const recipeId of user.current.recipes) {
          const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
            headers: {},
            method: 'GET',
          }).json()
          if (data.value) {
            if (type === 'Owned' && data.value.author === user.current.id)
              recipes.push(data.value)
            else if (type === 'Saved' && data.value.author !== user.current.id)
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
    async getUserRecipes(userId: number): Promise<Recipe[]> {
      const user = useUserStore()
      const currentUser = await user.getUserById(userId)
      const recipes: Recipe[] = []
      if (currentUser) {
        for (const recipeId of currentUser.recipes) {
          const { data, error } = await gumpFetch<Recipe>(`recipe/${recipeId}`, {
            headers: {},
            method: 'GET',
          }).json()
          if (data.value && data.value.author === userId)
            recipes.push(data.value)
          if (error.value)
            return error.value
        }
      }
      return recipes
    },
    async createRecipe(recipe?: Optional<Recipe, 'id'>): Promise<number | undefined> {
      const user = useUserStore()
      const ui = useUIStore()
      if (ui.createHeaderStates.some(state => !state))
        return

      const thisRecipe = recipe || this.currentRecipe

      if (thisRecipe)
        delete thisRecipe.id

      if (thisRecipe) {
        thisRecipe.author = user.current.id

        for (let i = 0; i < 2; i++) {
          const { data, error, statusCode } = await gumpFetch('recipe/create', {
            body: JSON.stringify(thisRecipe),
          }).text().post()
          if (data.value) {
            const id = parseInt(data.value, 10)
            this.recipes.unshift({
              id,
              ...thisRecipe,
            })
            this.currentRecipe = emptyRecipe

            user.current.recipes.push(id)
            ui.createHeaderIndex = 0
            ui.createHeaderStates = [false, false, false, false]
            return id
          }

          if (i === 0 && user.current.token !== 'offline' && statusCode.value === 401) {
            await user.login({
              username: user.current.username,
              password: user.current.password,
            })
            continue
          }

          if (error.value)
            return error.value
        }
      }
    },
    async updateRecipe(id: number, recipe: Partial<Recipe>): Promise<void> {
      if (!recipe.id)
        recipe.id = id
      else if (recipe.id !== id)
        return

      for (let i = 0; i < 2; i++) {
        const { error, statusCode } = await gumpFetch('recipe/update', {
          method: 'PATCH',
          body: JSON.stringify(recipe),
        })

        const user = useUserStore()
        if (i === 0 && user.current.token !== 'offline' && statusCode.value === 401) {
          await user.login({
            username: user.current.username,
            password: user.current.password,
          })
          continue
        }

        if (!error.value) {
          const foundRecipe = this.recipes.find(r => r.id === id)
          if (foundRecipe) {
          // update each changed property of the recipe with the new values
            Object.keys(recipe).forEach((prop) => {
              const key = prop as keyof Recipe
              if (foundRecipe[key] !== recipe[key])
                setValues(foundRecipe, key, recipe[key] as Recipe[keyof Recipe])
            })
          }
        } else {
          return error.value
        }
      }
    },
    async deleteRecipe(recipeId: number): Promise<void> {
      for (let i = 0; i < 2; i++) {
        const { error, statusCode } = await gumpFetch(`recipe/delete/${recipeId}`, {
          method: 'DELETE',
        })

        const user = useUserStore()
        if (i === 0 && user.current.token !== 'offline' && statusCode.value === 401) {
          await user.login({
            username: user.current.username,
            password: user.current.password,
          })
          continue
        }

        if (!error.value) {
          const recipe = this.recipes.find(r => r.id === recipeId)
          if (recipe)
            this.recipes.splice(this.recipes.indexOf(recipe), 1)
        } else {
          return error.value
        }
      }
    },
  },
  persist: true,
})
