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
    ingredients: [] as Ingredient[],
    currentRecipe: null as Recipe | null,
  }),
  getters: {
    getEmptyIngredients(): Ingredient[] {
      return this.ingredients.filter(ingredient => ingredient.name === '' && !ingredient.value && ingredient.volume === '')
    },
  },
  actions: {
    async getRecipes() {},
    addEmptyIngredient() {
      this.ingredients.push({
        name: '',
        value: 0,
        volume: '',
        linkedRecipe: this.currentRecipe?.id ?? 0,
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
  },
  persist: true,
})
