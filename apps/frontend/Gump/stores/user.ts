interface IUserState {
  username: string
}

export const useUserStore = defineStore('user', () => {
  const state = reactive<IUserState>({
    username: '',
  })

  const getUser = async (id: number) => {
    return 'Bartók Béla'
  }

  return {
    ...toRefs(state),
    getUser,
  }
},
{
  persist: true,
})
