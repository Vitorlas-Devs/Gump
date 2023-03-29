import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

export const useUserStore = defineStore(
  'user',
  () => {
    const state = reactive({
      username: '',
      loggedIn: false,
      avatarUrl: '',
      token: '',
      languages: [] as string[]
    })

    const login = (username: string, avatarUrl: string) => {
      state.username = username.replace(/ /g, '-')
      state.avatarUrl = avatarUrl
      state.loggedIn = true
    }

    const logout = () => {
      state.username = ''
      state.avatarUrl = ''
      state.loggedIn = false
    }

    const setLanguages = (languages: string[]) => {
      state.languages = languages
    }

    return {
      ...toRefs(state),
      login,
      logout,
      setLanguages
    }
  },
  {
    persist: true
  }
)
