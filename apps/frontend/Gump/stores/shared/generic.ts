// import type { UnwrapRef } from 'nuxt/dist/app/compat/capi'
import type {
  Store, StoreDefinition,
} from 'pinia'
import { PiniaStore } from './piniaTypes';

type ExtendedCounterStore = Store<'extendedCounterStore', ExtendedCounterState, ExtendedCounterGetters, CounterActions>;
type ExtendedCounterStoreDefinition = StoreDefinition<'extendedCounterStore', ExtendedCounterState, ExtendedCounterGetters, CounterActions>;

// ⚡ Two ways to define the generic state:

// 1️⃣ Whole state is T:
// state: () => ({} as T),

// 2️⃣ You want to add more properties to the state:
// state: () => ({
//   id: 0,
//   data: {} as T,
// }),

// ⚠️ this causes a weird bug so you have to cast data to UnwrapRef<T> in the actions
// ⚠️ another weird bug is with lists where instead of ({} as T) you have to use (ref<T[]>([]) as Ref<T[]>) 🤷‍♂️

// define a store factory that takes a generic type and a name and returns a store
export function useStore<
  // just a safeguard to make sure the generic type has an id property
  T extends { 'id': number },
>(
  name: string,
  extensionStore: Partial<Store>,
  // extensionStore: Store,
  persist?: boolean,
) {



  const extendedStore: PiniaStore<ExtendedCounterStore> = {
    
  }
}

// return defineStore(name, {
//   state: () => ({
//     current: null as T | null,
//     all: ref<T[]>([]) as Ref<T[]>,
//   }),
//   actions: {
//     addEmpty(item: T) {
//       this.all.push(item)
//     },
//     async get(id: number): Promise<UnwrapRef<T> | undefined> {
//       if (this.all.find(item => item.id === id))
//         return this.all.find(item => item.id === id) as UnwrapRef<T>

//       const { data, error } = await gumpFetch<T>(`${name}/${id}`, {
//         method: 'GET',
//       })
//       if (data.value) {
//         this.current = data.value as UnwrapRef<T> | null
//         return data.value as UnwrapRef<T>
//       }
//       if (error.value)
//         return error.value
//     },
//     async getAll(): Promise<T[] | undefined> {
//       const { data, error } = await gumpFetch<T[]>(`${name}`, {
//         method: 'GET',
//       })
//       if (data.value) {
//         this.all = data.value as T[]
//         return data.value
//       }
//       if (error.value)
//         return error.value
//     },
//   },
//   persist,
// })