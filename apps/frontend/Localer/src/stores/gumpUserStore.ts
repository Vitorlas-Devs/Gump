import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

interface ILoginData {
  token: string
  id: number
}

interface IGumpUser {
  id: number
  username: string
  profilePicture: number
  language: string
  recipes: number[]
  likes: number[]
  following: number[]
  follower: number[]
  badges: number[]
  isModerator: boolean
}

export const useGumpUserStore = defineStore(
  'gumpUser',
  () => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const state = reactive({
      id: 0,
      username: '',
      password: '',
      pfpUrl: '',
      sessionToken: ''
    })

    const login = (username: string, password: string): boolean => {
      const http = new XMLHttpRequest()

      http.open('POST', `${backendUrl}/auth/login`, false)
      http.setRequestHeader('Content-type', 'application/json')
      http.send(JSON.stringify({ username, password }))
      const login: ILoginData = JSON.parse(http.responseText)

      state.sessionToken = login.token

      http.open('GET', `${backendUrl}/user/me`, false)
      http.setRequestHeader('Authorization', `Bearer ${state.sessionToken}`)
      http.send()
      if (http.status !== 200) {
        state.sessionToken = ''
        return false
      }
      const user: IGumpUser = JSON.parse(http.responseText)

      if (!user.isModerator) {
        state.sessionToken = ''
        return false
      }

      state.id = login.id
      state.username = username
      state.password = password
      state.pfpUrl = `${backendUrl}/image/${user.profilePicture}`

      return true
    }

    const logout = () => {
      state.id = 0
      state.username = ''
      state.pfpUrl = ''
      state.sessionToken = ''
    }

    return {
      ...toRefs(state),
      login,
      logout
    }
  },
  {
    persist: true
  }
)
