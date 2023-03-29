import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

export const useUIStore = defineStore('ui', () => {
  const state = reactive({
    navbarOpen: true
  })

  const toggleNavbar = () => {
    state.navbarOpen = !state.navbarOpen
  }

  return {
    ...toRefs(state),
    toggleNavbar
  }
})
