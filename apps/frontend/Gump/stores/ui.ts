export type navState = 'home' | 'search' | 'create' | 'recipes' | 'profile'

interface IUIState {
  activeNav: navState
}

export const useUIStore = defineStore('ui', () => {
  // state
  const state = reactive<IUIState>({
    activeNav: 'home',
  })

  // getters
  // ...

  // actions
  const setActiveNav = (nav: navState) => {
    state.activeNav = nav
  }

  return {
    ...toRefs(state),
    setActiveNav,
  }
},
{
  persist: true, // put that motherfucker into a cookie
})
