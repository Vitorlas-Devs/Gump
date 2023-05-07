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
  likes: number[]
}
