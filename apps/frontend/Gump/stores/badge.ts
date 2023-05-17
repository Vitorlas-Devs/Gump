export const useBadgeStore = defineStore('badge', {
  actions: {
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
      const user = useUserStore()
      const currentUser = await user.getUserById(id)
      const badges: Badge[] = []
      if (currentUser) {
        for (const badgeId of currentUser.badges) {
          const badge = await this.getBadgeById(badgeId)
          if (badge)
            badges.push(badge)
        }
      }
      return badges
    },
  },
  persist: true,
})
