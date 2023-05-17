import type { Actions, StoreFactory } from './shared/generic'

type BadgeStore = StoreFactory<'badge', Badge, {}, {}, BadgeAcions>

type BadgeAcions = {
  getBadgesByUser(id: number): Promise<Badge[] | undefined>
} & Actions

const actions = createActions<Badge, BadgeStore>({
  async getBadgesByUser(id: number): Promise<Badge[] | undefined> {
    const user = useUserStore()
    const currentUser = user.current.id === id ? user.current : await user.getUserById(id)

    return this.all?.filter((badge: Badge) => currentUser?.badges.includes(badge.id))
  },
})

export const useBadgeStore = useStore<Badge, BadgeStore>(
  'badge',
  {
    actions,
  },
  true,
)
