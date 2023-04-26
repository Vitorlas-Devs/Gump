import { recipeEmpty } from './recipe'
import type { IRecipe } from './recipe'

export const tabs = [
  'Home',
  'Search',
  'Create',
  'Recipes',
  'Profile',
] as const

export type Tab = typeof tabs[number]

export const tabData = tabs.map(tab => ({
  tab: tab as Tab,
  name: `${tab}Nav`,
  path: `/${tab.toLowerCase()}`,
}))

export const sorts = ['hot', 'new', 'top'] as const

export type Sort = typeof sorts[number]

interface IUIState {
  activeNav: Tab
  activeSort: Sort
  searchToggled: boolean
  dropdownToggled: boolean
  searchValue: string
  searchHistory: string[]
  createHeaderIndex: number
  createHeaderStates: boolean[]
  createMode: 'raw' | 'design'
  createYScroll: number
  currentRecipe: IRecipe
}

export const useUIStore = defineStore('ui', () => {
  // state
  const state = reactive<IUIState>({
    activeNav: 'Home',
    activeSort: 'hot',
    searchToggled: false,
    dropdownToggled: false,
    searchValue: '',
    searchHistory: [],
    createHeaderIndex: 0,
    createHeaderStates: [false, false, false, false],
    createMode: 'design',
    createYScroll: 0,
    currentRecipe: recipeEmpty,
  })

  // ingredient functions
  const ingredientFunctions = () => {
    // getter
    const getIngredientList = computed(() => {
      return state.currentRecipe.ingredients.map(ingredient => ingredient.name)
    })

    const emptyIngredients = computed(() => {
      return state.currentRecipe.ingredients.filter(ingredient => ingredient.name === '' && !ingredient.value && ingredient.volume === '')
    })

    // action
    const addEmptyIngredient = () => {
      if (emptyIngredients.value.length === 0) {
        state.currentRecipe.ingredients.push({
          name: '',
          value: 0,
          volume: '',
          linkedRecipe: state.currentRecipe.id,
        })
      }
    }

    const checkEmptyIngredients = () => {
      if (emptyIngredients.value.length > 0) {
        emptyIngredients.value.forEach((ingredient) => {
          const index = state.currentRecipe.ingredients.indexOf(ingredient)
          if (index > -1)
            state.currentRecipe.ingredients.splice(index, 1)
        })
      }
    }

    return {
      getIngredientList,
      addEmptyIngredient,
      checkEmptyIngredients,
    }
  }

  // getters
  const getSearchHistory = computed(() => {
    if (state.searchHistory.length > 5)
      return state.searchHistory.slice(state.searchHistory.length - 5).reverse()
    else
      return state.searchHistory.slice().reverse()
  })

  // actions
  const setActiveNav = (nav: Tab) => {
    state.activeNav = nav
  }

  const setActiveSort = (sort: Sort) => {
    state.activeSort = sort
  }

  const setCreateHeaderIndex = (value: boolean) => {
    state.createHeaderStates[state.createHeaderIndex] = value
  }

  const addSearchHistory = (value: string) => {
    if (state.searchHistory.includes(value) || value === '') {
      // if it includes the value, push it to the end of the array
      const index = state.searchHistory.indexOf(value)
      if (index > -1) {
        state.searchHistory.splice(index, 1)
        state.searchHistory.push(value)
      }
      return
    }

    if (state.searchHistory.length >= 5)
      state.searchHistory.shift()

    state.searchHistory.push(value)
  }

  return {
    ...toRefs(state),
    ...ingredientFunctions(),
    getSearchHistory,
    setActiveNav,
    setActiveSort,
    addSearchHistory,
    setCreateHeaderIndex,
  }
},
{
  persist: true,
})
