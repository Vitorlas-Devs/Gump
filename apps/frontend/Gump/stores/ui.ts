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

interface IUIState {
  activeNav: Tab
}

export const useUIStore = defineStore('ui', () => {
  // state
  const state = reactive<IUIState>({
    activeNav: 'Home',
  })

  // getters
  // ...

  // actions
  const setActiveNav = (nav: Tab) => {
    state.activeNav = nav
  }

  return {
    ...toRefs(state),
    setActiveNav,
  }
},
{
  persist: true,
})
