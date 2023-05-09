export const emptyCurrentUser: CurrentUser = {
  id: 0,
  username: '',
  profilePicture: 0,
  recipes: [],
  likes: [],
  following: [],
  follower: [],
  badges: [],
  language: '',
  isModerator: false,
}

export const useUserStore = defineStore('user', {
  state: () => ({
    current: emptyCurrentUser,
    all: [] as User[],
  }),
  getters: {},
  actions: {
    async register(userDto: UserDto) {
      const { data, error } = await gumpFetch('user/create', {
        headers: {},
        body: JSON.stringify(userDto),
      }).text().post()
      if (data.value)
        this.current.id = parseInt(data.value, 10)
      if (error.value)
        return error.value
    },
    async login(userDto: UserDto) {
      const { data, error } = await gumpFetch<{ token: string; id: number }>('auth/login', {
        headers: {},
        method: 'POST',
        body: JSON.stringify(userDto),
      }).json()
      if (data.value)
        this.current.token = data.value.token

      if (error.value)
        return error.value
    },
    async getAuthorNameById(id: number): Promise<string | undefined> {
      const { data, error } = await gumpFetch<User>(`user/${id}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value)
        return data.value.username
      if (error.value)
        return '¯⁠\\_(⁠ツ⁠)_/⁠¯'
    },
    async getUserData() {
      const { data, error } = await gumpFetch<CurrentUser>('user/me', {
        headers: {
          Authorization: `Bearer ${this.current.token}`,
        },
        method: 'GET',
      }).json()
      if (data.value)
        this.current = data.value

      if (error.value)
        return error.value
    },
  },
  persist: true,
})
