import type { Store } from 'pinia'
import type { ExtractStoreType, PiniaActionTree, PiniaActions, PiniaGetterTree, PiniaGetters, PiniaState, PiniaStateTree } from './shared/piniaTypes'
import type { BaseStore } from './shared/generic'

type CategoryStore = Store<'category', CategoryState, CategoryGetters, CategoryActions<Category>>

type PartialCategoryStore = Store<
  'category',
  CategoryState & Partial<ExtractStoreType<BaseStore<Category>>['state']>,
  CategoryGetters & Partial<ExtractStoreType<BaseStore<Category>>['getters']>,
  CategoryActions<Category> & Partial<ExtractStoreType<BaseStore<Category>>['actions']>
>

type CategoryState = {
  currencySymbol: string
} & PiniaStateTree

type CategoryGetters = {
  countWithCurrency(): string
} & PiniaGetterTree

type CategoryActions<T> = {
  addEmpty(item: T): void
} & PiniaActionTree

const state: PiniaState<PartialCategoryStore> = () => {
  return {
    currencySymbol: '$',
  }
}

const getters: PiniaGetters<PartialCategoryStore> = {
  countWithCurrency(state) {
    return `1000 ${state.currencySymbol}`
  },
}

const actions: PiniaActions<PartialCategoryStore> = {
  addEmpty(item) {
    this.all?.push(item)
  },
}

export const useCategoryStore = useStore<Category, CategoryStore>(
  'category',
  state,
  getters,
  actions,
  true,
)
