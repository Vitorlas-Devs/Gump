interface IUserState {
  username: string
  password: string
}

export const useUserStore = defineStore('user', () => {
  // state
  const state = reactive<IUserState>({
    username: '',
      password: '',
  })

  // getters
  // ...

  // actions
  // ...

  return {
    ...toRefs(state),
  }
},
{
  persist: true,
})
