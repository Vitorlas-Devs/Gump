import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

interface IUIState {
  navbarOpen: boolean
  specialKey: string | null
}

export const useUIStore = defineStore('ui', () => {
  const state = reactive<IUIState>({
    navbarOpen: true,
    specialKey: null
  })

  const toggleNavbar = () => {
    state.navbarOpen = !state.navbarOpen
  }

  return {
    ...toRefs(state),
    toggleNavbar
  }
})
