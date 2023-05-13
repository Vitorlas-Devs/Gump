import type { Action, Getter, State, StoreFactory } from './shared/generic'

type CategoryStore = StoreFactory<'category', Category, CategoryState, CategoryGetters, CategoryActions>

type CategoryState = {
  currencySymbol: string
} & State

type CategoryGetters = {
  countWithCurrency(): string
} & Getter

type CategoryActions = {
  addEmpty(item: Category): void
} & Action

const state = createState<Category, CategoryStore>(() => ({
  currencySymbol: '$',
}))

const getters = createGetters<Category, CategoryStore>({
  countWithCurrency(state) {
    return `1000 ${state.currencySymbol}`
  },
})

const actions = createActions<Category, CategoryStore>({
  addEmpty(item) {
    this.all?.push(item)
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
