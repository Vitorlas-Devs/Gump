type UserDto = {
  username: string
  password: string
  email?: string
}

type User = {
  id: number
  username: string
  profilePicture: number
  recipes: number[]
  followingCount: number
  followerCount: number
  badges: number[]
}

type CurrentUser = Omit<User, 'followingCount' | 'followerCount'> & {
  language: string
  likes: number[]
  following: number[]
  follower: number[]
  isModerator: boolean
  token?: string
}

type SearchUser = Pick<User, 'id' | 'username' | 'profilePicture'>
