export const tabs = [
  'Home',
  'Search',
  'Create',
  'Recipes',
  'Profile',
] as const

export const tabData = tabs.map(tab => ({
  tab: tab as Tab,
  name: `${tab}Nav`,
  path: `/${tab.toLowerCase()}`,
}))

export const sorts = ['hot', 'new', 'top'] as const

export const recipeTabs = [
  'Info',
  'Ingredients',
  'Steps',
] as const

export const useUIStore = defineStore('ui', {
  state: () => ({
    activeNav: 'Home' as Tab,
    activeSort: 'hot' as Sort,
    activeRecipeTab: 'Info' as RecipeTab,
    searchToggled: false,
    dropdownToggled: false,
    searchValue: '',
    searchHistory: [] as string[],
    createHeaderIndex: 0,
    createHeaderStates: [false, false, false, false],
    createMode: 'design' as 'design' | 'raw',
    createYScroll: 0,
    params: {} as Record<RouteName, number>,
  }),
  getters: {
    getSearchHistory(state) {
      if (state.searchHistory.length > 5)
        return state.searchHistory.slice(state.searchHistory.length - 5).reverse()
      else
        return state.searchHistory.slice().reverse()
    },
  },
  actions: {
    setParams(routeName: RouteName, id: number) {
      this.params[routeName as RouteName] = id
    },
    setActiveNav(nav: Tab) {
      this.activeNav = nav
    },
    setActiveSort(sort: Sort) {
      this.activeSort = sort
    },
    setCreateHeaderIndex(value: boolean) {
      this.createHeaderStates[this.createHeaderIndex] = value
    },
    addSearchHistory(value: string) {
      if (this.searchHistory.includes(value) || value === '') {
        // if it includes the value, push it to the end of the array
        const index = this.searchHistory.indexOf(value)
        if (index > -1) {
          this.searchHistory.splice(index, 1)
          this.searchHistory.push(value)
        }
        return
      }

      if (this.searchHistory.length >= 5)
        this.searchHistory.shift()

      this.searchHistory.push(value)
    },
  },
  persist: true,
})
