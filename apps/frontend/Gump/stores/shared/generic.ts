import type { UnwrapRef } from 'nuxt/dist/app/compat/capi'
import type {
  Store, StoreDefinition,
} from 'pinia'
import type {
  PiniaActionTree,
  PiniaActions, PiniaGetterTree, PiniaGetters,
  PiniaState, PiniaStateTree, PiniaStore, StoreId,
} from './piniaTypes'

// ⚡ Two ways to define the generic state:

// 1️⃣ Whole state is T:
// state: () => ({} as T),

// 2️⃣ You want to add more properties to the state:
// state: () => ({
//   id: 0,
//   data: {} as T,
// }),

// ⚠️ this means that you have to cast data to UnwrapRef<T> in the actions
// ⚠️ and when using lists, instead of ({} as T) you have to use (ref<T[]>([]) as Ref<T[]>) 🤷‍♂️

// 👀 This Generic Store is also extendable!

export type BaseStore<T> = Store<'Base', BaseState<T>, BaseGetters, BaseActions<T>>
type ExtendedBaseStore<T, S extends Store> = Store<StoreId<S>, BaseState<T>, BaseGetters, BaseActions<T>>
type ExtendedBaseStoreDefinition<T, S extends Store> = StoreDefinition<StoreId<S>, BaseState<T>, BaseGetters, BaseActions<T>>

// types for Base State, Getters, Actions

export type BaseState<T> = {
  current: T | null
  all: Ref<T[]>
} & PiniaStateTree

export type BaseGetters = {
  getLength(): number
  getName(): string | undefined
} & PiniaGetterTree

export type BaseActions<T> = {
  addEmpty(item: T): void
  get(id: number): Promise<UnwrapRef<T> | undefined>
  getAll(): Promise<T[] | undefined>
} & PiniaActionTree

// type ExtractGeneric<T> = T extends Store<infer G> ? G : never

// define a store factory that takes a generic type and a name and returns a store
export function useStore<
  T extends { id: number }, S extends Store,
>(
  name: string,
  state: PiniaState<S>,
  getters: PiniaGetters<S>,
  actions: PiniaActions<S>,
  persist?: boolean,
) {
  const storeName: StoreId<S> = name as StoreId<S>

  const baseState: PiniaState<BaseStore<T>> = () => ({
    current: null,
    all: ref<T[]>([]) as Ref<T[]>,
  })

  const baseGetters: PiniaGetters<BaseStore<T>> = {
    getLength(state) {
      return (<T[]>state.all).length
    },
    getName(state) {
      if (state.current && hasName(state.current))
        return state.current.name
    },
  }

  const baseActions: PiniaActions<BaseStore<T>> = {
    addEmpty(item: T) {
      (<T[]> this.all).push(item)
    },
    async get(id: number) {
      if ((<T[]> this.all).find(item => item.id === id))
        return (<T[]> this.all).find(item => item.id === id) as UnwrapRef<T>

      const { data, error } = await gumpFetch<T>(`${name}/${id}`, {
        method: 'GET',
      }).json()
      if (data.value) {
        this.current = data.value as UnwrapRef<T> | null
        return data.value as UnwrapRef<T>
      }
      if (error.value)
        return error.value
    },
    async getAll() {
      const { data, error } = await gumpFetch<T[]>(`${name}`, {
        method: 'GET',
      }).json()
      if (data.value) {
        this.all = data.value as T[]
        return data.value
      }
      if (error.value)
        return error.value
    },
  }

  const extendedStore: PiniaStore<ExtendedBaseStore<T, S>> = {
    state: () => ({
      ...baseState(),
      ...state(),
    }),
    getters: {
      ...baseGetters,
      ...getters,
    },
    actions: {
      ...baseActions,
      ...actions,
    },
    persist,
  }

  const extended = defineStore(storeName, extendedStore) as ExtendedBaseStoreDefinition<T, S>

  return extended
}
