export const emptyCurrentUser: CurrentUser = {
  id: 0,
  username: '',
  email: '',
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
      if (data.value)
        this.current.token = data.value.token

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
    async updateUser(userDto: UserDto, profilePicture: number, language: string): Promise<User | undefined> {
      const requestBody = {
        ...userDto,
        id: this.current.id,
        profilePicture,
        language,
      }
      const { data, error } = await gumpFetch('user/update', {
        method: 'PATCH',
        body: JSON.stringify(requestBody),
      }).json()
      if (data.value)
        return data.value
      if (error.value)
        return error.value
    },
    async getBadgeById(id: number): Promise<Badge | undefined> {
      const { data, error } = await gumpFetch<Badge>(`badge/${id}`, {
        headers: {},
        method: 'GET',
      }).json()
      if (data.value)
        return data.value
      if (error.value)
        return error.value
    },
    async getBadgesByUser(id: number): Promise<Badge[] | undefined> {
      const user = await this.getUserById(id)
      const badges: Badge[] = []
      if (user) {
        for (const badgeId of user.badges) {
          const badge = await this.getBadgeById(badgeId)
          if (badge)
            badges.push(badge)
        }
      }
      return badges
    },
    async getFollows(type: FollowsSort): Promise<User[] | undefined> {
      const users: User[] = []

      const { data, error } = await gumpFetch<CurrentUser>('user/me', {
        method: 'GET',
      }).json()
      if (data.value) {
        if (type === 'Followers') {
          for (const userId of data.value.follower) {
            const user = await this.getUserById(userId)
            if (user)
              users.push(user)
          }
          return users
        } else {
          for (const userId of data.value.following) {
            const user = await this.getUserById(userId)
            if (user)
              users.push(user)
          }
          return users
        }
      }
      if (error.value)
        return error.value
    },
  },
  persist: true,
})
