export const useRecipeStore = defineStore('recipe', {
  state: () => ({
    recipes: [] as Recipe[],
    ingredients: [] as Ingredient[],
    currentRecipe: null as Recipe | null,
  }),
  getters: {
    getEmptyIngredients(): Ingredient[] {
      return this.ingredients.filter(ingredient => ingredient.name === '' && !ingredient.value && ingredient.volume === '')
    },
  },
  actions: {
    async getRecipes(searchTerm: string) {
      const { data, error } = await gumpFetch<Recipe[]>(`recipe/search?sort=${searchTerm}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value)
        this.recipes = data.value
      if (error.value)
        return error.value
    },
    addEmptyIngredient() {
      this.ingredients.push({
        name: '',
        value: 0,
        volume: '',
        linkedRecipe: this.currentRecipe?.id ?? 0,
      })
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
    addRecipe(recipe: Recipe) {
      this.ingredients.push({
        name: recipe.title,
        value: recipe.serves,
        volume: 'adag',
        linkedRecipe: recipe.id,
      })
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
    async getRecipeById(recipeId: number) {
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
