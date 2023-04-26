import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

interface ILoginData {
  token: string
  id: number
}

export interface IGumpUser {
  id: number
  username: string
  email: string
  profilePicture: number
  language: string
  recipes: number[]
  likes: number[]
  following: number[]
  follower: number[]
  badges: number[]
  isModerator: boolean
}

export interface IGumpUserData {
  id: number
  username: string
  password: string
  profilePicture: number
  pfpUrl: string
  sessionToken: string
  users: IGumpUserData[]
}

export const useGumpUserStore = defineStore(
  'gumpUser',
  () => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const state = reactive({
      id: 0,
      username: '',
      password: '',
      profilePicture: 0,
      pfpUrl: '',
      sessionToken: '',
      users: []
    } as IGumpUserData)

    const login = (username: string, password: string): boolean => {
      const http = new XMLHttpRequest()

      http.open('POST', `${backendUrl}/auth/login`, false)
      http.setRequestHeader('Content-type', 'application/json')
      http.send(JSON.stringify({ username, password }))
      const loginData: ILoginData = JSON.parse(http.responseText)

      state.sessionToken = loginData.token

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

      state.id = loginData.id
      state.username = username
      state.password = password
      state.profilePicture = user.profilePicture
      state.pfpUrl = `${backendUrl}/image/${user.profilePicture}`

      return true
    }

    const logout = () => {
      state.id = 0
      state.username = ''
      state.pfpUrl = ''
      state.sessionToken = ''
    }

    const getUser = async (id: number): Promise<IGumpUser> => {
      const data: IGumpUser = await fetch(`${backendUrl}/user/${id}`).then((res) => res.json())
      return data
    }

    const loadUsers = async () => {
      const data: IGumpUserData[] = await fetch(`${backendUrl}/user/search?sort=new`).then((res) =>
        res.json()
      )
      data.forEach((user) => {
        user.pfpUrl = `${backendUrl}/image/${user.profilePicture}`
      })
      state.users = data
    }

    const fetchUsers = async () => {
      const data: IGumpUserData[] = await fetch(
        `${backendUrl}/user/search?sort=new&offset=${state.users.length}`
      ).then((res) => res.json())
      data.forEach((user) => {
        user.pfpUrl = `${backendUrl}/image/${user.profilePicture}`
      })
      state.users.push(...data)
    }

    const updateUser = async (user: IGumpUserData) => {
      await fetch(`${backendUrl}/user/update`, {
        method: 'PATCH',
        body: JSON.stringify(user),
        headers: {
          'Content-type': 'application/json',
          authorization: `Bearer ${state.sessionToken}`
        }
      })
    }

    const deleteUser = async (id: number) => {
      await fetch(`${backendUrl}/user/delete/${id}`, {
        method: 'DELETE',
        headers: {
          authorization: `Bearer ${state.sessionToken}`
        }
      })
    }

    return {
      ...toRefs(state),
      login,
      logout,
      getUser,
      loadUsers,
      fetchUsers,
      updateUser,
      deleteUser
    }
  },
  {
    persist: true
  }
)
