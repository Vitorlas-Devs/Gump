export const emptyCurrentUser: CurrentUser = {
  id: 0,
  username: '',
  password: '',
  profilePicture: 0,
  recipes: [],
  likes: [],
  following: [],
  follower: [],
  badges: [],
  language: '',
  isModerator: false,
  token: '',
}

export const useUserStore = defineStore('user', {
  state: () => ({
    current: emptyCurrentUser,
    all: [] as User[],
  }),
  getters: {
    getUserNameById() {
      return (id: number) => {
        const foundUser = this.all?.find(user => user.id === id)
        return foundUser?.username || ''
      }
    },
    getUserIdByName() {
      return (name: string) => {
        const foundUser = this.all?.find(user => user.username === name)
        return foundUser?.id || 1
      }
    },
  },
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
      if (data.value) {
        this.current.token = data.value.token
        this.current.password = userDto.password
      }

      if (error.value)
        return error.value
    },
    async getUserData() {
      const { data, error } = await gumpFetch<CurrentUser>('user/me', {
        method: 'GET',
      }).json()
      if (data.value) {
        const { token } = this.current
        this.current = data.value
        this.current.token = token
      }
      if (error.value)
        return error.value
    },
    async getAuthorById(id: number): Promise<string | undefined> {
      const { data, error } = await gumpFetch<User>(`user/${id}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value)
        return data.value.username
      if (error.value)
        return '¯⁠\\_(⁠ツ⁠)_/⁠¯'
    },
    async getUserById(id: number): Promise<User | undefined> {
      const foundUser = this.all.find(user => user.id === id)
      if (foundUser) {
        return foundUser
      } else {
        const { data, error } = await gumpFetch<User>(`user/${id}`, {
          headers: {},
          method: 'GET',
        }).json()
        if (data.value) {
          this.all.push(data.value)
          return data.value
        }
        if (error.value)
          return error.value
      }
    },
    async searchUser(search: string): Promise<SearchUser[] | undefined> {
      const { data, error } = await gumpFetch<SearchUser[]>(`user/search?searchTerm=${search}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value) {
        const promises = data.value.map(async (user: SearchUser) => await this.getUserById(user.id))

        await Promise.all(promises)

        return data.value
      }
      if (error.value)
        return error.value
    },
    async searchUserName(search: string): Promise<string[] | undefined> {
      const { data, error } = await gumpFetch<SearchUser[]>(`user/search?searchTerm=${search}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value) {
        const promises = data.value.map(async (user: SearchUser) => await this.getUserById(user.id))

        await Promise.all(promises)

        return data.value.map((user: SearchUser) => user.username)
      }
      if (error.value)
        return error.value
    },
  },
  persist: true,
})
