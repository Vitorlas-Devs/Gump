import type { Actions, Getters, State, StoreFactory } from './shared/generic'

type CategoryStore = StoreFactory<'category', Category, CategoryState, CategoryGetters, CategoryActions>

type CategoryState = {
} & State

type CategoryGetters = {
  getCategoryNameById(): (id: number) => string
  getCategoryIdByName(): (name: string) => number
} & Getters

type CategoryActions = {
  getAllString(): Promise<string[] | undefined>
} & Actions

const state = createState<Category, CategoryStore>(() => ({
}))

const getters = createGetters<Category, CategoryStore>({
  getCategoryNameById() {
    return (id: number) => {
      const category = this.all?.find(category => category.id === id)
      return category?.name || ''
    }
  },
  getCategoryIdByName() {
    return (name: string) => {
      const category = this.all?.find(category => category.name === name)
      return category?.id || 0
    }
  },
})

const actions = createActions<Category, CategoryStore>({
  async getAllString() {
    const categories = await this.getAll?.()
    return categories?.map(category => category.name) || []
  },
})

export const useCategoryStore = useStore<Category, CategoryStore>(
  'category',
  {
    state,
    getters,
    actions,
  },
  true,
)
