interface IUserState {
  username: string
}

export const useUserStore = defineStore('user', () => {
  // state
  const state = reactive<IUserState>({
    username: '',
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
