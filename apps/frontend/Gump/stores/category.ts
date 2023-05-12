// export const useCategoryStore = useStore<Category>('category', true)

import type { Store } from 'pinia'
import type { PiniaActionTree, PiniaActions, PiniaGetterTree, PiniaGetters, PiniaState, PiniaStateTree } from './shared/piniaTypes'

type CategoryStore = Store<'category', CategoryState, CategoryGetters, CategoryActions<Category>>

type CategoryState = {
  currencySymbol: string
} & PiniaStateTree

type CategoryGetters = {
  countWithCurrency(): string
} & PiniaGetterTree

type CategoryActions<T> = {
  addEmpty(item: T): void
} & PiniaActionTree

const state: PiniaState<CategoryStore> = () => {
  return {
    currencySymbol: '$',
  }
}

const getters: PiniaGetters<CategoryStore> = {
  countWithCurrency(state) {
    return `1000 ${state.currencySymbol}`
  },
}

const actions: PiniaActions<CategoryStore> = {
  addEmpty(item) {
    console.log('addEmpty', item)
  },
}

export const useCategoryStore = useStore<Category, CategoryStore>(
  'category',
  state,
  getters,
  actions,
  true,
)
