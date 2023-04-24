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
  })

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

  const addSearchHistory = (value: string) => {
    if (state.searchHistory.includes(value) || value === '')
      return

    if (state.searchHistory.length >= 5)
      state.searchHistory.shift()

    state.searchHistory.push(value)
  }

  return {
    ...toRefs(state),
    getSearchHistory,
    setActiveNav,
    setActiveSort,
    addSearchHistory,
  }
},
{
  persist: true,
})
