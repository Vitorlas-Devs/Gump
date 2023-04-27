interface IUserState {
  username: string
  token: string
}

export const useUserStore = defineStore('user', () => {
  // state
  const state = reactive<IUserState>({
    username: '',
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
  // ...

  return {
    ...toRefs(state),
    getUser,
    getToken,
  }
},
{
  persist: true,
})
