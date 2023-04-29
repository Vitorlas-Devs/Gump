import type { UnwrapRef } from 'nuxt/dist/app/compat/capi'

// define a store factory that takes a generic type and a name and returns a store
export function useStore<T extends { 'id': number }>(name: string, persist?: boolean) {
  // name the store using the generic type
  return defineStore(`${name}Store`, {
    state: () => ({
      current: null as T | null,
      all: ref<T[]>([]) as Ref<T[]>,
    }),
    getters: {
      get(state) {
        return state
      },
    },
    actions: {
      addEmpty(item: T) {
        this.all.push(item)
      },
      async get(id: number) {
        const { data, error } = await gumpFetch<T>(`${name}/${id}`, {
          method: 'GET',
        })
        if (data.value)
          this.current = data.value as UnwrapRef<T> | null
        if (error.value)
          return error.value
      },
      async getAll() {},
    },
    persist: persist ?? true,
  })
}

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
