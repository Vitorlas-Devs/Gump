interface IUserState {
  username: string
}

export const useUserStore = defineStore('user', () => {
  // state
  const state = reactive<IUserState>({
    username: '',
  })

  // getters
  const getUser = async (id: number) => {
    return state.username
  }

  // actions
  // ...

  return {
    ...toRefs(state),
    getUser,
  }
},
{
  persist: true,
})
