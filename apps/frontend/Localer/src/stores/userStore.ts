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
      openPullRequest: false,
      prUrl : '',
      prNumber: 0,
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
      state.token = ''
      state.openPullRequest = false
      state.prUrl = ''
      state.prNumber = 0
      state.languages = []
    }

    const addLanguage = (language: string) => {
      state.languages.push(language)
    }

    return {
      ...toRefs(state),
      login,
      logout,
      addLanguage
    }
  },
  {
    persist: true
  }
)
