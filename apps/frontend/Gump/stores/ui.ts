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
}

export const useUIStore = defineStore('ui', () => {
  // state
  const state = reactive<IUIState>({
    activeNav: 'Home',
    activeSort: 'hot',
  })

  // getters
  // ...

  // actions
  const setActiveNav = (nav: Tab) => {
    state.activeNav = nav
  }

  const setActiveSort = (sort: Sort) => {
    state.activeSort = sort
  }

  return {
    ...toRefs(state),
    setActiveNav,
    setActiveSort,
  }
},
{
  persist: true,
})
