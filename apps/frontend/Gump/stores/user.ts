interface IUserState {
  username: string
}

export const useUserStore = defineStore('user', () => {
  // state
  const state = reactive<IUserState>({
    username: '',
  })

  // in a setup store, we can also just define username as a ref
  // const username = ref('')
  // the reactive state is useful when we want to expose the whole state object
  // reactive needs to be exported with toRefs, which makes every property a ref

  // getters
  // ...

  // actions
  // ...

  return {
    ...toRefs(state),
  }
},
{
  persist: true, // put that motherfucker into a cookie
})
