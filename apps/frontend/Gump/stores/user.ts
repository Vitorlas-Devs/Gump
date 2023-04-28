interface IUserState {
  id: number
  username: string
  password: string
  email: string
  token: string
}

export type UserData = Omit<IUserState, 'token' | 'id'>

export const useUserStore = defineStore('user', () => {
  // state
  const state = reactive<IUserState>({
    id: 0,
    username: '',
    password: '',
    email: '',
    token: '',
  })

  // getters
  const getUser = () => {
    // ...
  }

  const getToken = () => {
    return state.token
  }

  // actions
  const register = async (userData: UserData) => {
    const { data, error } = await gumpFetch('user/create', {
      headers: {},
      body: JSON.stringify(userData),
    }).text().post()
    if (data.value)
      state.id = parseInt(data.value, 10)
    if (error.value)
      return error.value
  }

  return {
    ...toRefs(state),
    getUser,
    getToken,
    register,
  }
},
{
  persist: true,
})
