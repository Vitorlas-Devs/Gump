import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

interface IUIState {
  navbarOpen: boolean
}

export const useUIStore = defineStore('ui', () => {
  const state = reactive<IUIState>({
    navbarOpen: true,
  })

  const toggleNavbar = () => {
    state.navbarOpen = !state.navbarOpen
  }

  return {
    ...toRefs(state),
    toggleNavbar
  }
})
