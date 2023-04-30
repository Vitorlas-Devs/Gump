export const useUserStore = defineStore('user', {
  state: () => ({
    id: 0,
    username: '',
    password: '',
    email: '',
    token: '',
  }),
  getters: {},
  actions: {
    async register(userDto: UserDto) {
      const { data, error } = await gumpFetch('user/create', {
        headers: {},
        body: JSON.stringify(userDto),
      }).text().post()
      if (data.value)
        this.id = parseInt(data.value, 10)
      if (error.value)
        return error.value
    },
    async login(userDto: UserDto) {
      const { data, error } = await gumpFetch<{ token: string; id: number }>('auth/login', {
        headers: {},
        method: 'POST',
        body: JSON.stringify(userDto),
      })
      if (data.value)
        this.token = data.value.token
      if (error.value)
        return error.value
    },
  },
  persist: true,
})
